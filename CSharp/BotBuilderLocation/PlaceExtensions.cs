namespace Microsoft.Bot.Builder.Location
{
    using Connector;

    public static class PlaceExtensions
    {
        public static PostalAddress GetPostalAddress(this Place place)
        {
            if (place.Address != null)
            {
                return (PostalAddress) place.Address;
            }

            return null;
        }

        public static GeoCoordinates GetGeoCoordinates(this Place place)
        {
            if (place.Geo != null)
            {
                return (GeoCoordinates)place.Geo;
            }

            return null;
        }

        internal static Place FromLocation(Bing.Location location)
        {
            var place = new Place
            {
                Type = location.EntityType,
                Name = location.Name
            };

            if (location.Address != null)
            {
                place.Address = new PostalAddress
                {
                    FormattedAddress = location.Address.FormattedAddress,
                    Country = location.Address.CountryRegion,
                    Locality = location.Address.Locality,
                    PostOfficeBoxNumber = location.Address.PostalCode,
                    PostalCode = location.Address.PostalCode,
                    Region = location.Address.AdminDistrict,
                    StreetAddress = location.Address.AddressLine
                };
            }

            if (location.Point != null && location.Point.HasCoordinates)
            {
                place.Geo = new GeoCoordinates
                {
                    Latitude = location.Point.Coordinates[0],
                    Longitude = location.Point.Coordinates[1]
                };
            }

            return place;
        }
    }
}
