namespace Microsoft.Bot.Builder.Location
{
    using System;
    using System.Collections.Generic;
    using System.Reflection;
    using System.Threading;
    using System.Threading.Tasks;
    using Connector;
    using Dialogs;
    using Internals.Fibers;
    using Internals.Scorables;
    using Resources;
    using SpecialCommands;

    [Serializable]
    public abstract class LocationDialogBase<T> : IDialog<T> where T : class 
    {
        private readonly LocationResourceManager resourceManager;

        internal LocationResourceManager ResourceManager => this.resourceManager;

        protected LocationDialogBase(Assembly resourceAssembly, string resourceName)    
        {
            this.resourceManager = new LocationResourceManager(resourceAssembly, resourceName);
        }

        internal LocationDialogBase(LocationResourceManager resourceManager)
        {
            SetField.NotNull(out this.resourceManager, nameof(resourceManager), resourceManager);
        }

        public abstract Task StartAsync(IDialogContext context);

        protected async Task MessageReceivedAsync(IDialogContext context, IAwaitable<IMessageActivity> result)
        {
            var commands = new List<IScorable<IMessageActivity, double>>
            {
                new CancelSpecialCommandScorable<T>(context, context, this.resourceManager.Cancel),
                new HelpSpecialCommandScorable<T>(context, context, this.resourceManager.Help, this.resourceManager.HelpMessage)
            };

            var commandsFold = new FoldScorable<IMessageActivity, double>(new DoubleComparer(), commands);

            int initialFramesCount = context.Frames.Count;
            if (await commandsFold.TryPostAsync(await result, CancellationToken.None))
            {
                // TODO: this is a bit of a hack to only call context.Wait if the scorable
                // didn't manipulate the stack. Is there a cleaner way to achieve this?
                if (initialFramesCount == context.Frames.Count)
                {
                    context.Wait(this.MessageReceivedAsync);
                }
            }
            else
            {
                await this.MessageReceivedInternalAsync(context, result);
            }
        }

        protected abstract Task MessageReceivedInternalAsync(IDialogContext context, IAwaitable<IMessageActivity> result);

        private sealed class DoubleComparer : IComparer<double>
        {
            int IComparer<double>.Compare(double one, double two)
            {
                return one.CompareTo(two);
            }
        }
    }
}
