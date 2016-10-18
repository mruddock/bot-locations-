namespace Microsoft.Bot.Builder.Location.SpecialCommands
{
    using System.Threading;
    using System.Threading.Tasks;
    using Connector;
    using Dialogs.Internals;

    internal class CancelSpecialCommandScorable : SpecialCommandScorable
    {
        public CancelSpecialCommandScorable(IDialogStack stack, IBotToUser botToUser, string command)
            : base(stack, botToUser, command)
        {
        }

        public override Task PostAsync(IMessageActivity item, object state, CancellationToken token)
        {
            this.Stack.Done<Place>(null);
            return Task.FromResult(0);
        }
    }
}
