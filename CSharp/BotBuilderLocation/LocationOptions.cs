namespace Microsoft.Bot.Builder.Location
{
    using System;

    /// <summary>
    /// Options for customizing <see cref="LocationSelectionDialog"/>
    /// </summary>
    [Flags]
    public enum LocationOptions
    {
        /// <summary>
        /// No options.
        /// </summary>
        None = 0,

        /// <summary>
        /// Some of the channels (e.g. Facebook) has a built in
        /// location widget. Use this option to indicate if you want
        /// the <c>LocationDialog</c> to use it when available.
        /// </summary>
        UseNativeControl = 1,

        /// <summary>
        /// Use this option if you want the location dialog to reverse lookup
        /// geo-coordinates before returning. This can be useful if you depend
        /// on the channel location service or native control to get user location
        /// but still want the control to return to you a full address.
        /// </summary>
        ReverseGeocode = 2
    }
}