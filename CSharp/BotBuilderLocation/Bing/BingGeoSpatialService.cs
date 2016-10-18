namespace Microsoft.Bot.Builder.Location.Bing
{
    using System;
    using System.Configuration;
    using System.Globalization;
    using System.Linq;
    using System.Net.Http;
    using System.Threading.Tasks;
    using System.Web.Configuration;
    using Newtonsoft.Json;

    internal sealed class BingGeoSpatialService : IGeoSpatialService
    {
        private readonly static string ApiKey = WebConfigurationManager.AppSettings["BingMapsApiKey"];
        private readonly static string FindByQueryApiUrl = $"https://dev.virtualearth.net/REST/v1/Locations?key={ApiKey}&q=";
        private readonly static string FindByPointUrl = $"https://dev.virtualearth.net/REST/v1/Locations/{{0}},{{1}}?key={ApiKey}&q=";
        private readonly static string ImageUrl = $"https://dev.virtualearth.net/REST/V1/Imagery/Map/Road/{{0}},{{1}}/15?mapSize=500,500&pp={{0}},{{1}};1;{{2}}&key={ApiKey}";

        public async Task<LocationSet> GetLocationsByQueryAsync(string address)
        {
            if (string.IsNullOrEmpty(address))
            {
                throw new ArgumentNullException(nameof(address));
            }

            return await this.GetLocationsAsync(FindByQueryApiUrl + Uri.EscapeDataString(address));
        }

        public async Task<LocationSet> GetLocationsByPointAsync(double latitude, double longitude)
        {
            return await this.GetLocationsAsync(
                string.Format(CultureInfo.InvariantCulture, FindByPointUrl, latitude, longitude));
        }

        public string GetLocationMapImageUrl(GeocodePoint point, int? index = null)
        {
            if (point == null || !point.HasCoordinates)
            {
                throw new ArgumentNullException(nameof(point));
            }

            if (string.IsNullOrEmpty(ApiKey))
            {
                throw new ConfigurationErrorsException("BingMapsApiKey is missing in Web.config");
            }

            return string.Format(ImageUrl, point.Coordinates[0], point.Coordinates[1], index);
        }

        private async Task<LocationSet> GetLocationsAsync(string url)
        {

            if (string.IsNullOrEmpty(ApiKey))
            {
                throw new ConfigurationErrorsException("BingMapsApiKey is missing in Web.config");
            }

            using (var client = new HttpClient())
            {
                var response = await client.GetStringAsync(url);
                var apiResponse = JsonConvert.DeserializeObject<LocationApiResponse>(response);

                // TODO: what is the right logic for picking a location set?
                return apiResponse.LocationSets?.FirstOrDefault();
            }
        }
    }
}