namespace Microsoft.Bot.Builder.Location
{
    using System;
    using System.Threading.Tasks;
    using Connector;
    using Dialogs;
    using Internals.Fibers;
    using Resources;

    [Serializable]
    internal class LocationRequiredFieldsDialog : LocationDialogBase<Bing.Location>
    {
        private readonly Bing.Location location;
        private readonly LocationRequiredFields requiredFields;
        private string currentFieldName;

        public LocationRequiredFieldsDialog(Bing.Location location, LocationRequiredFields requiredFields, LocationResourceManager resourceManager)
            : base(resourceManager)
        {
            SetField.NotNull(out this.location, nameof(location), location);
            this.requiredFields = requiredFields;
            this.location.Address = this.location.Address ?? new Bing.Address();
        }

        public override async Task StartAsync(IDialogContext context)
        {
            await this.CompleteMissingFields(context);
        }

        protected override async Task MessageReceivedInternalAsync(IDialogContext context, IAwaitable<IMessageActivity> result)
        {
            this.location.Address.GetType().GetProperty(this.currentFieldName).SetValue(this.location.Address, (await result).Text);
            await this.CompleteMissingFields(context);
        }

        private async Task CompleteMissingFields(IDialogContext context)
        {
            bool notComplete =
                await this.CompleteFieldIfMissing(context, nameof(Strings.AskForStreetAddress), LocationRequiredFields.StreetAddress, "AddressLine", this.location.Address.AddressLine)
                || await this.CompleteFieldIfMissing(context, nameof(Strings.AskForLocality), LocationRequiredFields.Locality, "Locality", this.location.Address.Locality)
                || await this.CompleteFieldIfMissing(context, nameof(Strings.AskForRegion), LocationRequiredFields.Region, "AdminDistrict", this.location.Address.AdminDistrict)
                || await this.CompleteFieldIfMissing(context, nameof(Strings.AskForCountry), LocationRequiredFields.Country, "CountryRegion", this.location.Address.CountryRegion)
                || await this.CompleteFieldIfMissing(context, nameof(Strings.AskForPostalCode), LocationRequiredFields.PostalCode, "PostalCode", this.location.Address.PostalCode);

            if (!notComplete)
            {
                context.Done(this.location);
            }
        }

        private async Task<bool> CompleteFieldIfMissing(IDialogContext context, string stringName, LocationRequiredFields field, string name, string value)
        {
            if (!this.requiredFields.HasFlag(field) || !string.IsNullOrEmpty(value))
            {
                return false;
            }

            this.currentFieldName = name;
            await context.PostAsync(this.ResourceManager.GetResource(stringName));
            context.Wait(this.MessageReceivedAsync);

            return true;
        }
    }
}
