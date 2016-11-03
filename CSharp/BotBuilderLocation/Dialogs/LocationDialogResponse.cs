namespace Microsoft.Bot.Builder.Location.Dialogs
{
    internal class LocationDialogResponse
    {
        public Bing.Location Value { get; set; }

        public SpecialCommand SpecialCommand { get; set; }
    }
}
