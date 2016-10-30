namespace Microsoft.Bot.Builder.Location.Channels
{
    using System;
    using Dialogs;

    [Serializable]
    internal class FacebookChannelHandler : IChannelHandler
    {
        public bool HasNativeLocationControl => true;

        public bool SupportsKeyboard => true;

        public IDialog<Bing.Location> CreateNativeLocationDialog(string prompt)
        {
            return new FacebookLocationDialog(prompt);
        }
    }
}
