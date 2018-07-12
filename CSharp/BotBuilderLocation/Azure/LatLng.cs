using Newtonsoft.Json;
using System;

namespace Microsoft.Bot.Builder.Location.Azure
{
    [Serializable]
    public class LatLng
    {
        [JsonProperty(PropertyName = "lat")]
        public double Latitude { get; set; }

        [JsonProperty(PropertyName = "lon")]
        public double Longitude { get; set; }
    }
}
