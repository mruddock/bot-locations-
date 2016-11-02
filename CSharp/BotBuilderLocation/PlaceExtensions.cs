namespace Microsoft.Bot.Builder.Location
{
    using Connector;

    /// <summary>
    /// Extensions for <see cref="Place"/>
    /// </summary>
    public static class PlaceExtensions
    {
        /// <summary>
        /// Gets the postal address.
        /// </summary>
        /// <param name="place">The place.</param>
        /// <returns>The <see cref="PostalAddress"/> if available, null otherwise.</returns>
        public static PostalAddress GetPostalAddress(this Place place)
        {
            if (place.Address != null)
            {
                return (PostalAddress) place.Address;
            }

            return null;
        }

        /// <summary>
        /// Gets the geo coordinates.
        /// </summary>
        /// <param name="place">The place.</param>
        /// <returns>The <see cref="GeoCoordinates"/> if available, null otherwise.</returns>
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
