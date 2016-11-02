namespace Microsoft.Bot.Builder.Location
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;
    using System.Threading.Tasks;
    using Bing;
    using Channels;
    using Connector;
    using Dialogs;

    /// <summary>
    /// Represents a dialog that handles retrieving a location from the user.
    /// <para>
    /// This dialog provides a location picker conversational UI (CUI) control,
    /// powered by Bing's Geo-spatial API and Places Graph, to make the process 
    /// of getting the user's location easy and consistent across all messaging 
    /// channels supported by bot framework.
    /// </para>
    /// </summary>
    /// <example>
    /// The following examples demonstrate how to use <see cref="LocationDialog"/> to achieve different scenarios:
    /// <list type="bullet">
    /// <item>
    /// <description>
    /// Calling <see cref="LocationDialog"/> with default parameters
    /// <code>
    /// var prompt = "Hi, where would you like me to ship to your widget?";
    /// var locationDialog = new LocationDialog(message.ChannelId, prompt);
    /// context.Call(locationDialog, (dialogContext, result) => {...});
    /// </code>
    /// </description>
    /// </item>
    /// <item>
    /// <description>
    /// Using channel's native location widget if available (e.g. Facebook) 
    /// <code>
    /// var prompt = "Hi, where would you like me to ship to your widget?";
    /// var locationDialog = new LocationDialog(message.ChannelId, prompt, LocationOptions.UseNativeControl);
    /// context.Call(locationDialog, (dialogContext, result) => {...});
    /// </code>
    /// </description>
    /// </item>
    /// <item>
    /// <description>
    /// Using channel's native location widget if available (e.g. Facebook)
    /// and having Bing try to reverse geo-code the provided coordinates to
    ///  automatically fill-in address fields.
    /// For more info see <see cref="LocationOptions.ReverseGeocode"/>
    /// <code>
    /// var prompt = "Hi, where would you like me to ship to your widget?";
    /// var locationDialog = new LocationDialog(message.ChannelId, prompt, LocationOptions.UseNativeControl | LocationOptions.ReverseGeocode);
    /// context.Call(locationDialog, (dialogContext, result) => {...});
    /// </code>
    /// </description>
    /// </item>
    /// <item>
    /// <description>
    /// Specifying required fields to have the dialog prompt the user for if missing from address. 
    /// For more info see <see cref="LocationRequiredFields"/>
    /// <code>
    /// var prompt = "Hi, where would you like me to ship to your widget?";
    /// var locationDialog = new LocationDialog(message.ChannelId, prompt, LocationOptions.None, LocationRequiredFields.StreetAddress | LocationRequiredFields.PostalCode);
    /// context.Call(locationDialog, (dialogContext, result) => {...});
    /// </code>
    /// </description>
    /// </item>
    /// <item>
    /// <description>
    /// Example on how to handle the returned place
    /// <code>
    /// var prompt = "Hi, where would you like me to ship to your widget?";
    /// var locationDialog = new LocationDialog(message.ChannelId, prompt, LocationOptions.None, LocationRequiredFields.StreetAddress | LocationRequiredFields.PostalCode);
    /// context.Call(locationDialog, (context, result) => {
    ///     Place place = await result;
    ///     if (place != null)
    ///     {
    ///         var address = place.GetPostalAddress();
    ///         string name = address != null ?
    ///             $"{address.StreetAddress}, {address.Locality}, {address.Region}, {address.Country} ({address.PostalCode})" :
    ///             "the pinned location";
    ///         await context.PostAsync($"OK, I will ship it to {name}");
    ///     }
    ///     else
    ///     {
    ///         await context.PostAsync("OK, I won't be shipping it");
    ///     }
    /// }
    /// </code>
    /// </description>
    /// </item>
    /// </list>
    /// </example>
    [Serializable]
    public sealed class LocationDialog : LocationDialogBase<Place>
    {
        private readonly string prompt;
        private readonly LocationOptions options;
        private readonly LocationRequiredFields requiredFields;
        private readonly IChannelHandler channelHandler;
        private readonly List<Location> locations;

        /// <summary>
        /// Determines whether this is the root dialog or not.
        /// </summary>
        /// <remarks>
        /// This is used to determine how the dialog should handle special commands
        /// such as reset.
        /// </remarks>
        protected override bool IsRootDialog => true;

        /// <summary>
        /// Initializes a new instance of the <see cref="LocationDialog"/> class.
        /// </summary>
        /// <param name="channelId">The channel identifier.</param>
        /// <param name="prompt">The prompt posted to the user when dialog starts.</param>
        /// <param name="options">The location options used to customize the experience.</param>
        /// <param name="requiredFields">The location required fields.</param>
        /// <param name="resourceAssembly">The resource assembly. If not specified, the default resources will be used.</param>
        /// <param name="resourceName">Name of the resource. If not specified, the default resources will be used.</param>
        public LocationDialog(
            string channelId,
            string prompt,
            LocationOptions options = LocationOptions.None,
            LocationRequiredFields requiredFields = LocationRequiredFields.None,
            Assembly resourceAssembly = null,
            string resourceName = null) : base(resourceAssembly, resourceName)
        {
            this.prompt = prompt;
            this.options = options;
            this.requiredFields = requiredFields;
            this.locations = new List<Location>();
            this.channelHandler = ChannelHandlerFactory.CreateChannelHandler(channelId);
        }

        /// <summary>
        /// Starts the dialog.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <returns>The asynchronous task</returns>
        public override async Task StartAsync(IDialogContext context)
        {
            this.locations.Clear();

            if (this.options.HasFlag(LocationOptions.UseNativeControl) && this.channelHandler.HasNativeLocationControl)
            {
                var nativeDialog = this.channelHandler.CreateNativeLocationDialog(this.prompt, this.ResourceManager);
                context.Call(nativeDialog, this.ResumeAfterNativeLocationDialogAsync);
            }
            else
            {
                await context.PostAsync(this.prompt);
                context.Wait(this.MessageReceivedAsync);
            }
        }

        /// <summary>
        /// Invoked by base class when a new message is received.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <param name="result">The result.</param>
        /// <returns>The asynchronous task</returns>
        protected override async Task MessageReceivedInternalAsync(IDialogContext context, IAwaitable<IMessageActivity> result)
        {
            var message = await result;

            if (this.locations.Count == 0)
            {
                await this.TryResolveAddressAsync(context, message);
            }
            else if (!this.TryResolveAddressSelectionAsync(context, message))
            {
                await context.PostAsync(this.ResourceManager.InvalidLocationResponse);
                context.Wait(this.MessageReceivedAsync);
            }
        }

        private async Task ResumeAfterNativeLocationDialogAsync(IDialogContext context, IAwaitable<LocationDialogResponse> result)
        {
            var response = await result;

            // If special command do nothing as base class already handled it.
            if (await this.HandleSpecialCommandResponse(context, response)) return;

            var location = response.Value;

            // If user passed ReverseGeocode flag and dialog returned a geo point,
            // then try to reverse geocode it using BingGeoSpatialService
            if (this.options.HasFlag(LocationOptions.ReverseGeocode) && location?.Point != null)
            {
                var results = await new BingGeoSpatialService().GetLocationsByPointAsync(location.Point.Coordinates[0], location.Point.Coordinates[1]);
                var geocodedLocation = results?.Locations?.FirstOrDefault();
                if (geocodedLocation?.Address != null)
                {
                    // We don't trust reverse geo-coder on the street address level,
                    // so copy all fields except it.
                    // TODO: do we need to check the returned confidence level?
                    location.Address = new Bing.Address
                    {
                        CountryRegion = geocodedLocation.Address.CountryRegion,
                        AdminDistrict = geocodedLocation.Address.AdminDistrict,
                        AdminDistrict2 = geocodedLocation.Address.AdminDistrict2,
                        Locality = geocodedLocation.Address.Locality,
                        PostalCode = geocodedLocation.Address.PostalCode
                    };
                }
            }

            this.CompleteAndReturnPlace(context, location);
        }

        private async Task TryResolveAddressAsync(IDialogContext context, IMessageActivity message)
        {
            // TODO: handle exception
            var locationSet = await new BingGeoSpatialService().GetLocationsByQueryAsync(message.Text);
            var foundLocations = locationSet?.Locations;

            if (foundLocations == null || foundLocations.Count == 0)
            {
                await context.PostAsync(this.ResourceManager.LocationNotFound);

                context.Wait(this.MessageReceivedAsync);
            }
            else
            {
                this.locations.AddRange(foundLocations);

                var locationsCardReply = context.MakeMessage();
                locationsCardReply.Attachments = AddressCard.CreateLocationsCard(this.locations);
                locationsCardReply.AttachmentLayout = AttachmentLayoutTypes.Carousel;
                await context.PostAsync(locationsCardReply);

                if (this.locations.Count == 1)
                {
                    this.PromptForSingleAddressSelection(context);
                }
                else
                {
                    await this.PromptForMultipleAddressSelection(context);
                }
            }
        }

        private bool TryResolveAddressSelectionAsync(IDialogContext context, IMessageActivity message)
        {
            int value;
            if (int.TryParse(message.Text, out value) && value > 0 && value <= this.locations.Count)
            {
                this.CompleteAndReturnPlace(context, this.locations[value - 1]);
                return true;
            }

            return false;
        }

        private void PromptForSingleAddressSelection(IDialogContext context)
        {
            PromptStyle style = this.channelHandler.SupportsKeyboard
                        ? PromptStyle.Keyboard
                        : PromptStyle.None;

            PromptDialog.Confirm(
                    context,
                    async (dialogContext, answer) =>
                    {
                        if (await answer)
                        {
                            this.CompleteAndReturnPlace(dialogContext, this.locations.First());
                        }
                        else
                        {
                            await this.StartAsync(dialogContext);
                        }
                    },
                    prompt: this.ResourceManager.SingleResultFound,
                    retry: null,
                    attempts: 3,
                    promptStyle: style);
        }

        private async Task PromptForMultipleAddressSelection(IDialogContext context)
        {
            if (this.channelHandler.SupportsKeyboard)
            {
                var keyboardCardReply = context.MakeMessage();
                keyboardCardReply.Attachments = AddressCard.CreateLocationsKeyboardCard(this.locations, this.ResourceManager.MultipleResultsFound);
                keyboardCardReply.AttachmentLayout = AttachmentLayoutTypes.List;
                await context.PostAsync(keyboardCardReply);
            }
            else
            {
                await context.PostAsync(this.ResourceManager.MultipleResultsFound);
            }

            context.Wait(this.MessageReceivedAsync);
        }

        private void CompleteAndReturnPlace(IDialogContext context, Location location)
        {
            if (location == null)
            {
                context.Done<Place>(null);
            }
            else if (this.requiredFields != LocationRequiredFields.None)
            {
                context.Call(
                    new LocationRequiredFieldsDialog(location, this.requiredFields, this.ResourceManager),
                    async (dialogContext, result) =>
                    {
                        var response = await result;
                        var specialCommand = await this.HandleSpecialCommandResponse(dialogContext, response);
                        if (!specialCommand)
                        {
                            dialogContext.Done(PlaceExtensions.FromLocation(response.Value));
                        }
                    });
            }
            else
            {
                context.Done(PlaceExtensions.FromLocation(location));
            }
        }
    }
}