namespace Microsoft.Bot.Builder.Location.Channels
{
    using System;
    using Dialogs;

    [Serializable]
    internal class DefaultChannelHandler : IChannelHandler
    {
        public bool HasNativeLocationControl => false;

        public bool SupportsKeyboard => false;

        public IDialog<LocationDialogResponse> CreateNativeLocationDialog(string prompt, LocationResourceManager resourceManager)
        {
            throw new NotSupportedException("Default channel handler doesn't support native location control.");
        }
    }
}
