namespace Microsoft.Bot.Builder.Location.Channels
{
    using Connector;
    using Dialogs;

    internal interface IChannelHandler
    {
        bool HasNativeLocationControl { get; }

        bool SupportsKeyboard { get; }

        IDialog<Place> CreateNativeLocationDialog(string prompt);
    }
}
