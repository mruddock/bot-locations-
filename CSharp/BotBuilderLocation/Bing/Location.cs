namespace Microsoft.Bot.Builder.Location.Bing
{
    using System;
    using System.Collections.Generic;
    using Newtonsoft.Json;

    [Serializable]
    internal class Location
    {
        [JsonProperty(PropertyName = "__type")]
        public string LocationType { get; set; }

        [JsonProperty(PropertyName = "bbox")]
        public List<double> BoundaryBox { get; set; }

        [JsonProperty(PropertyName = "name")]
        public string Name { get; set; }

        [JsonProperty(PropertyName = "point")]
        public GeocodePoint Point { get; set; }

        [JsonProperty(PropertyName = "address")]
        public Address Address { get; set; }

        [JsonProperty(PropertyName = "confidence")]
        public string Confidence { get; set; }

        [JsonProperty(PropertyName = "entityType")]
        public string EntityType { get; set; }

        [JsonProperty(PropertyName = "geocodePoints")]
        public List<GeocodePoint> GeocodePoints { get; set; }

        [JsonProperty(PropertyName = "matchCodes")]
        public List<string> MatchCodes { get; set; }
    }
}