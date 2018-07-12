using Newtonsoft.Json;
using System;

namespace Microsoft.Bot.Builder.Location.Azure
{
    [Serializable]
    public class PoiInfo
    {
        [JsonProperty(PropertyName = "name")]
        public string Name { get; set; }
    }
}
