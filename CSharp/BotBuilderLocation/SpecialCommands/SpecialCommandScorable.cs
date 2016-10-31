namespace Microsoft.Bot.Builder.Location.SpecialCommands
{
    using System;
    using System.Threading;
    using System.Threading.Tasks;
    using Connector;
    using Dialogs.Internals;
    using Internals.Fibers;
    using Internals.Scorables;

    [Serializable]
    internal abstract class SpecialCommandScorable<T> : IScorable<IMessageActivity, double> where T : class
    {
        private readonly string command;
        private readonly IDialogStack stack;
        private readonly IBotToUser botToUser;

        protected IDialogStack Stack => this.stack;

        protected IBotToUser BotToUser => this.botToUser;

        protected SpecialCommandScorable(IDialogStack stack, IBotToUser botToUser, string command)
        {
            SetField.NotNull(out this.stack, nameof(stack), stack);
            SetField.NotNull(out this.command, nameof(command), command);
            SetField.NotNull(out this.botToUser, nameof(botToUser), botToUser);
        }

        public Task<object> PrepareAsync(IMessageActivity item, CancellationToken token)
        {
            return Task.FromResult(StringComparer.OrdinalIgnoreCase.Equals(item.Text, this.command)
                ? new object()
                : null);
        }

        public bool HasScore(IMessageActivity item, object state)
        {
            return state != null;
        }

        public double GetScore(IMessageActivity item, object state)
        {
            return 1.0;
        }

        public abstract Task PostAsync(IMessageActivity item, object state, CancellationToken token);
    }
}
