namespace Microsoft.Bot.Builder.Location.Bing
{
    using System;
    using System.Collections.Generic;
    using Newtonsoft.Json;

    [Serializable]
    internal class GeocodePoint
    {
        [JsonProperty(PropertyName = "coordinates")]
        public List<double> Coordinates { get; set; }

        [JsonProperty(PropertyName = "calculationMethod")]
        public string CalculationMethod { get; set; }

        [JsonProperty(PropertyName = "usageTypes")]
        public List<string> UsageTypes { get; set; }

        [JsonIgnore]
        public bool HasCoordinates => this.Coordinates != null && this.Coordinates.Count == 2;
    }
}