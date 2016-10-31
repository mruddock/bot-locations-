namespace Microsoft.Bot.Builder.Location.SpecialCommands
{
    using System.Threading;
    using System.Threading.Tasks;
    using Dialogs;
    using Dialogs.Internals;
    using Connector;
    using Internals.Fibers;

    internal class HelpSpecialCommandScorable : SpecialCommandScorable
    {
        private readonly string helpText;

        public HelpSpecialCommandScorable(IDialogStack stack, IBotToUser botToUser, string command, string helpText)
            : base(stack, botToUser, command)
        {
            SetField.NotNull(out this.helpText, nameof(helpText), helpText);
        }

        public override async Task PostAsync(IMessageActivity item, object state, CancellationToken token)
        {
            await this.BotToUser.PostAsync(this.helpText);
        }
    }
}
