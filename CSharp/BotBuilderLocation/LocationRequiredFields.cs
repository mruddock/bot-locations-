namespace Microsoft.Bot.Builder.Location
{
    using System;

    [Flags]
    public enum LocationRequiredFields
    {
        AddressLine = 0,

        AdminDistrict = 1,

        AdminDistrict2 = 2,

        CountryRegion = 4,

        GeoCoordinates = 8,

        AddressLineOrGeoCoordinates = 16
    }
}
