namespace Microsoft.Bot.Builder.Location.Dialogs
{
    internal class LocationDialogResponse
    {
        public Bing.Location Location { get; }

        public string Message { get; }

        public LocationDialogResponse(Bing.Location location = null, string message = null)
        {
            this.Location = location;
            this.Message = message;
        }
    }
}
