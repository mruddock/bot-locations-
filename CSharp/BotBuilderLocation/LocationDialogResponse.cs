namespace Microsoft.Bot.Builder.Location
{
    internal class LocationDialogResponse
    {
        public Bing.Location Value { get; set; }

        public SpecialCommand SpecialCommand { get; set; }
    }
}
