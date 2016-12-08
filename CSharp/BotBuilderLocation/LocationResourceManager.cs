namespace Microsoft.Bot.Builder.Location
{
    using System;
    using System.Reflection;
    using System.Resources;
    using Resources;

    /// <summary>
    /// The location resource manager. Inherit from this class if you would like to override
    /// some or all the prompt strings.
    /// </summary>
    [Serializable]
    public class LocationResourceManager
    {
        private readonly ResourceManager resourceManager;

        /// <summary>
        /// The <see cref="AskForCountry"/> resource string.
        /// </summary>
        public virtual string AskForCountry => this.GetResource(nameof(Strings.AskForCountry));

        /// <summary>
        /// The <see cref="AskForLocality"/> resource string.
        /// </summary>
        public virtual string AskForLocality => this.GetResource(nameof(Strings.AskForLocality));

        /// <summary>
        /// The <see cref="AskForPostalCode"/> resource string.
        /// </summary>
        public virtual string AskForPostalCode => this.GetResource(nameof(Strings.AskForPostalCode));

        /// <summary>
        /// The <see cref="AskForRegion"/> resource string.
        /// </summary>
        public virtual string AskForRegion => this.GetResource(nameof(Strings.AskForRegion));

        /// <summary>
        /// The <see cref="AskForStreetAddress"/> resource string.
        /// </summary>
        public virtual string AskForStreetAddress => this.GetResource(nameof(Strings.AskForStreetAddress));

        /// <summary>
        /// The <see cref="CancelCommand"/> resource string.
        /// </summary>
        public virtual string CancelCommand => this.GetResource(nameof(Strings.CancelCommand));

        /// <summary>
        /// The <see cref="HelpCommand"/> resource string.
        /// </summary>
        public virtual string HelpCommand => this.GetResource(nameof(Strings.HelpCommand));

        /// <summary>
        /// The <see cref="HelpMessage"/> resource string.
        /// </summary>
        public virtual string HelpMessage => this.GetResource(nameof(Strings.HelpMessage));

        /// <summary>
        /// The <see cref="InvalidLocationResponse"/> resource string.
        /// </summary>
        public virtual string InvalidLocationResponse => this.GetResource(nameof(Strings.InvalidLocationResponse));

        /// <summary>
        /// The <see cref="InvalidLocationResponseFacebook"/> resource string.
        /// </summary>
        public virtual string InvalidLocationResponseFacebook => this.GetResource(nameof(Strings.InvalidLocationResponseFacebook));

        /// <summary>
        /// The <see cref="LocationNotFound"/> resource string.
        /// </summary>
        public virtual string LocationNotFound => this.GetResource(nameof(Strings.LocationNotFound));

        /// <summary>
        /// The <see cref="MultipleResultsFound"/> resource string.
        /// </summary>
        public virtual string MultipleResultsFound => this.GetResource(nameof(Strings.MultipleResultsFound));

        /// <summary>
        /// The <see cref="ResetCommand"/> resource string.
        /// </summary>
        public virtual string ResetCommand => this.GetResource(nameof(Strings.ResetCommand));

        /// <summary>
        /// The <see cref="ResetPrompt"/> resource string.
        /// </summary>
        public virtual string ResetPrompt => this.GetResource(nameof(Strings.ResetPrompt));

        /// <summary>
        /// The <see cref="CancelPrompt"/> resource string.
        /// </summary>
        public virtual string CancelPrompt => this.GetResource(nameof(Strings.CancelPrompt));

        /// <summary>
        /// The <see cref="SelectLocation"/> resource string.
        /// </summary>
        public virtual string SelectLocation => this.GetResource(nameof(Strings.SelectLocation));

        /// <summary>
        /// The <see cref="SingleResultFound"/> resource string.
        /// </summary>
        public virtual string SingleResultFound => this.GetResource(nameof(Strings.SingleResultFound));

        /// <summary>
        /// The <see cref="TitleSuffix"/> resource string.
        /// </summary>
        public virtual string TitleSuffix => this.GetResource(nameof(Strings.TitleSuffix));

        /// <summary>
        /// The <see cref="TitleSuffixFacebook"/> resource string.
        /// </summary>
        public virtual string TitleSuffixFacebook => this.GetResource(nameof(Strings.TitleSuffixFacebook));

        /// <summary>
        /// The <see cref="Confirmation"/> resource string.
        /// </summary>
        public virtual string Confirmation => this.GetResource(nameof(Strings.Confirmation));

        /// <summary>
        /// The <see cref="ConfirmationAsk"/> resource string.
        /// </summary>
        public virtual string ConfirmationAsk => this.GetResource(nameof(Strings.ConfirmationAsk));

        /// <summary>
        /// The <see cref="AddressSeparator"/> resource string.
        /// </summary>
        public virtual string AddressSeparator => this.GetResource(nameof(Strings.AddressSeparator));

        /// <summary>
        /// The <see cref="OtherComand"/> resource string.
        /// </summary>
        public virtual string OtherComand => this.GetResource(nameof(Strings.OtherComand));

        /// <summary>
        /// The <see cref="ConfirmationInvalidResponse"/> resource string.
        /// </summary>
        public virtual string ConfirmationInvalidResponse => this.GetResource(nameof(Strings.ConfirmationInvalidResponse));

        internal LocationResourceManager(Assembly resourceAssembly = null, string resourceName = null)
        {
            if (resourceAssembly == null || resourceName == null)
            {
                resourceAssembly = typeof(LocationDialog).Assembly;
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
