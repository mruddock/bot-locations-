namespace Microsoft.Bot.Builder.Location
{
    using System;
    using System.Reflection;
    using System.Resources;
    using Resources;

    [Serializable]
    internal class LocationResourceManager
    {
        private readonly ResourceManager resourceManager;

        public string AskForCountry => this.GetResource(nameof(Strings.AskForCountry));

        public string AskForLocality => this.GetResource(nameof(Strings.AskForLocality));

        public string AskForPostalCode => this.GetResource(nameof(Strings.AskForPostalCode));

        public string AskForRegion => this.GetResource(nameof(Strings.AskForRegion));

        public string AskForStreetAddress => this.GetResource(nameof(Strings.AskForStreetAddress));

        public string Cancel => this.GetResource(nameof(Strings.Cancel));

        public string Help => this.GetResource(nameof(Strings.Help));

        public string HelpMessage => this.GetResource(nameof(Strings.HelpMessage));

        public string InvalidLocationResponse => this.GetResource(nameof(Strings.InvalidLocationResponse));

        public string LocationNotFound => this.GetResource(nameof(Strings.LocationNotFound));

        public string MultipleResultsFound => this.GetResource(nameof(Strings.MultipleResultsFound));

        public string Reset => this.GetResource(nameof(Strings.Reset));

        public string SelectLocation => this.GetResource(nameof(Strings.SelectLocation));

        public string SingleResultFound => this.GetResource(nameof(Strings.SingleResultFound));

        internal LocationResourceManager(Assembly resourceAssembly = null, string resourceName = null)
        {
            if (resourceAssembly == null || resourceName == null)
            {
                resourceAssembly = typeof(LocationSelectionDialog).Assembly;
                resourceName = typeof(Strings).FullName;
            }

            this.resourceManager = new ResourceManager(resourceName, resourceAssembly);
        }

        private string GetResource(string name)
        {
            return this.resourceManager.GetString(name) ??
                   Strings.ResourceManager.GetString(name);
        }
    }
}
