namespace Microsoft.Bot.Builder.Location.Channels
{
    using System;
    using Dialogs;

    [Serializable]
    internal class FacebookChannelHandler : IChannelHandler
    {
        public bool HasNativeLocationControl => true;

        public bool SupportsKeyboard => true;

        public IDialog<LocationDialogResponse> CreateNativeLocationDialog(string prompt, LocationResourceManager resourceManager)
        {
            return new FacebookLocationDialog(prompt, resourceManager);
        }
    }
}
