namespace Microsoft.Bot.Builder.Location.Channels
{
    using Dialogs;

    internal interface IChannelHandler
    {
        bool HasNativeLocationControl { get; }

        bool SupportsKeyboard { get; }

        IDialog<Bing.Location> CreateNativeLocationDialog(string prompt, LocationResourceManager resourceManager);
    }
}
