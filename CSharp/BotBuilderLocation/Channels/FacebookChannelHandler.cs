namespace Microsoft.Bot.Builder.Location.Channels
{
    using System;
    using Connector;
    using Dialogs;

    [Serializable]
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
