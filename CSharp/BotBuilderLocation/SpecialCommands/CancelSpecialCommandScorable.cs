namespace Microsoft.Bot.Builder.Location.SpecialCommands
{
    using System.Threading;
    using System.Threading.Tasks;
    using Connector;
    using Dialogs.Internals;

    internal class CancelSpecialCommandScorable<T> : SpecialCommandScorable where T : class
    {
        public CancelSpecialCommandScorable(IDialogStack stack, IBotToUser botToUser, string command)
            : base(stack, botToUser, command)
        {
        }

        public override Task PostAsync(IMessageActivity item, object state, CancellationToken token)
        {
            this.Stack.Done<T>(null);
            return Task.FromResult(0);
        }
    }
}
