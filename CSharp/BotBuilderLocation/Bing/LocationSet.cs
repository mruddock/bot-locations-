namespace Microsoft.Bot.Builder.Location.Bing
{
    using System;
    using System.Collections.Generic;
    using Newtonsoft.Json;

    [Serializable]
    internal class LocationSet
    {
        [JsonProperty(PropertyName = "estimatedTotal")]
        public int EstimatedTotal { get; set; }

        [JsonProperty(PropertyName = "resources")]
        public List<Location> Locations { get; set; }
    }
}