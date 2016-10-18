namespace Microsoft.Bot.Builder.Location.Bing
{
    using System;
    using Newtonsoft.Json;

    [Serializable]
    internal class Address
    {
        [JsonProperty(PropertyName = "addressLine")]
        public string AddressLine { get; set; }

        [JsonProperty(PropertyName = "adminDistrict")]
        public string AdminDistrict { get; set; }

        [JsonProperty(PropertyName = "adminDistrict2")]
        public string AdminDistrict2 { get; set; }

        [JsonProperty(PropertyName = "countryRegion")]
        public string CountryRegion { get; set; }

        [JsonProperty(PropertyName = "formattedAddress")]
        public string FormattedAddress { get; set; }

        [JsonProperty(PropertyName = "locality")]
        public string Locality { get; set; }

        [JsonProperty(PropertyName = "postalCode")]
        public string PostalCode { get; set; }
    }
}