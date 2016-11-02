namespace Microsoft.Bot.Builder.Location.Channels
{
    using Dialogs;

    internal interface IChannelHandler
    {
        bool HasNativeLocationControl { get; }

        bool SupportsKeyboard { get; }

        IDialog<LocationDialogResponse> CreateNativeLocationDialog(string prompt, LocationResourceManager resourceManager);
    }
}
