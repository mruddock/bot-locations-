namespace Microsoft.Bot.Builder.Location
{
    using System;

    [Flags]
    public enum LocationRequiredFields
    {
        None = 0,

        StreetAddress = 1,

        Locality = 2,

        Region = 4,

        Country = 8,

        PostalCode = 16
    }
}
