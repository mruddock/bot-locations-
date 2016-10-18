namespace Microsoft.Bot.Builder.Location.SpecialCommands
{
    using System.Collections.Generic;
    using Connector;
    using Dialogs.Internals;
    using Internals.Scorables;
    using Resources;

    internal static class SpecialCommandsScorables
    {
        public static IScorable<IMessageActivity, double> GetCommand(IDialogStack stack, IBotToUser botToUser, LocationResourceManager resourceManager)
        {
            var commands = new List<IScorable<IMessageActivity, double>>
            {
                new CancelSpecialCommandScorable(stack, botToUser, resourceManager.GetResource(nameof(Strings.Cancel))),
                new HelpSpecialCommandScorable(stack, botToUser, resourceManager.GetResource(nameof(Strings.Help)), resourceManager.GetResource(nameof(Strings.HelpMessage)))
            };

            return new FoldScorable<IMessageActivity, double>(DoubleComparer.Instance, commands);
        }

        private sealed class DoubleComparer : IComparer<double>
        {
            public static readonly IComparer<double> Instance = new DoubleComparer();

            int IComparer<double>.Compare(double one, double two)
            {
                return one.CompareTo(two);
            }
        }
    }
}
