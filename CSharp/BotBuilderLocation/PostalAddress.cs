namespace Microsoft.Bot.Builder.Location
{
    using System;

    /// <summary>
    /// Represents a postal address (https://schema.org/PostalAddress)
    /// </summary>
    [Serializable]
    public class PostalAddress
    {
        public string FormattedAddress { get; set; }

        public string Country { get; set; }

        public string Locality { get; set; }

        public string Region { get; set; }

        public string PostalCode { get; set; }

        public string StreetAddress { get; set; }
    }
}
