namespace Microsoft.Bot.Builder.Location.SpecialCommands
{
    using System.Collections.Generic;
    using System.Threading;
    using System.Threading.Tasks;
    using Connector;
    using Dialogs;
    using Internals.Scorables;
    using Resources;

    internal static class SpecialCommands
    {
        public static async Task<bool> TryPostAsync(IDialogContext context, IMessageActivity message, LocationResourceManager resourceManager)
        {
            var commands = new List<IScorable<IMessageActivity, double>>
            {
                new CancelSpecialCommandScorable(context, context, resourceManager.GetResource(nameof(Strings.Cancel))),
                new HelpSpecialCommandScorable(context, context, resourceManager.GetResource(nameof(Strings.Help)), resourceManager.GetResource(nameof(Strings.HelpMessage)))
            };

            var fold = new FoldScorable<IMessageActivity, double>(DoubleComparer.Instance, commands);

            return await fold.TryPostAsync(message, CancellationToken.None);
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
