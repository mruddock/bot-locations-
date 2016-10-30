namespace Microsoft.Bot.Builder.Location
{
    using System;
    using System.Threading.Tasks;
    using Dialogs;
    using Internals.Fibers;
    using Resources;

    [Serializable]
    internal class LocationRequiredFieldsDialog : IDialog<Bing.Location>
    {
        private readonly Bing.Location location;
        private readonly LocationRequiredFields requiredFields;
        private readonly LocationResourceManager resourceManager;
        private string currentFieldName;

        public LocationRequiredFieldsDialog(Bing.Location location, LocationRequiredFields requiredFields, LocationResourceManager resourceManager)
        {
            SetField.NotNull(out this.location, nameof(location), location);
            SetField.NotNull(out this.resourceManager, nameof(resourceManager), resourceManager);
            this.requiredFields = requiredFields;
            this.location.Address = this.location.Address ?? new Bing.Address();
        }

        public async Task StartAsync(IDialogContext context)
        {
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
            if (this.requiredFields.HasFlag(field) && string.IsNullOrEmpty(value))
            {
                this.currentFieldName = name;
                await context.PostAsync(this.resourceManager.GetResource(stringName));
                context.Wait(async (dialogContext, result) =>
                {

                    this.location.Address.GetType().GetProperty(this.currentFieldName).SetValue(this.location.Address, (await result).Text);
                    await this.CompleteMissingFields(dialogContext);
                });

                return true;
            }

            return false;
        }
    }
}
