namespace Microsoft.Bot.Builder.Location.Channels
{
    using System;
    using Connector;
    using Dialogs;

    [Serializable]
    internal class DefaultChannelHandler : IChannelHandler
    {
        public bool HasNativeLocationControl => false;

        public bool SupportsKeyboard => false;

        public IDialog<Place> CreateNativeLocationDialog(string prompt)
        {
            throw new NotSupportedException();
        }
    }
}
