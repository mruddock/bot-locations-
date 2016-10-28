namespace Microsoft.Bot.Builder.Location.Channels
{
    using Connector;
    using Dialogs;

    internal class FacebookChannelHandler : IChannelHandler
    {
        public bool HasNativeLocationControl => true;

        public bool SupportsKeyboard => true;

        public IDialog<Place> CreateNativeLocationDialog(string prompt)
        {
            return new FacebookLocationDialog(prompt);
        }
    }
}
