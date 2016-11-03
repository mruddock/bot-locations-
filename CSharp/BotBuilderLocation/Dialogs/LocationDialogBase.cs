namespace Microsoft.Bot.Builder.Location.Dialogs
{
    using System;
    using System.Reflection;
    using System.Threading.Tasks;
    using Builder.Dialogs;
    using Connector;
    using Internals.Fibers;

    /// <summary>
    /// Represents base dialog that handles all the base functionalities such as
    /// running special commands scorables on all received messages.
    /// </summary>
    /// <typeparam name="T">The dialog type</typeparam>
    [Serializable]
    public abstract class LocationDialogBase<T> : IDialog<T> where T : class 
    {
        private readonly LocationResourceManager resourceManager;

        /// <summary>
        /// Determines whether this is the root dialog or not.
        /// </summary>
        /// <remarks>
        /// This is used to determine how the dialog should handle special commands
        /// such as reset.
        /// </remarks>
        protected virtual bool IsRootDialog => false;

        /// <summary>
        /// Gets the resource manager.
        /// </summary>
        /// <value>
        /// The resource manager.
        /// </value>
        internal LocationResourceManager ResourceManager => this.resourceManager;

        /// <summary>
        /// Initializes a new instance of the <see cref="LocationDialogBase{T}" /> class.
        /// </summary>
        /// <param name="resourceAssembly">The resource assembly.</param>
        /// <param name="resourceName">Name of the resource.</param>
        internal LocationDialogBase(Assembly resourceAssembly, string resourceName)
        {
            this.resourceManager = new LocationResourceManager(resourceAssembly, resourceName);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="LocationDialogBase{T}" /> class.
        /// </summary>
        /// <param name="resourceManager">The resource manager.</param>
        internal LocationDialogBase(LocationResourceManager resourceManager)
        {
            SetField.NotNull(out this.resourceManager, nameof(resourceManager), resourceManager);
        }

        /// <summary>
        /// Starts the dialog.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <returns>The asynchronous task.</returns>
        public abstract Task StartAsync(IDialogContext context);

        /// <summary>
        /// Invoked when a new message is received.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <param name="result">The result.</param>
        /// <returns>The asynchronous task.</returns>
        internal async Task MessageReceivedAsync(IDialogContext context, IAwaitable<IMessageActivity> result)
        {
            var text = (await result)?.Text?.Trim();
            
            if (StringComparer.OrdinalIgnoreCase.Equals(text, this.resourceManager.Help))
            {
                await context.PostAsync(this.resourceManager.HelpMessage);
                context.Wait(this.MessageReceivedAsync);
            }
            else if (StringComparer.OrdinalIgnoreCase.Equals(text, this.resourceManager.Reset))
            {
                // If this is the root dialog handle reset by resending the start prompt
                // else create a reset response and pass it to the parent dialog.
                if (this.IsRootDialog)
                {
                    await this.StartAsync(context);
                }
                else
                {
                    var response = new LocationDialogResponse { SpecialCommand = SpecialCommand.Reset };
                    context.Done(response);
                }
            }
            else if (StringComparer.OrdinalIgnoreCase.Equals(text, this.resourceManager.Cancel))
            {
                context.Done<T>(null);
            }
            else
            {
                await this.MessageReceivedInternalAsync(context, result);
            }
        }

        /// <summary>
        /// Implements the dialog specific logic that needs to run on new messages.
        /// If the message is special command, it gets handled by <see cref="MessageReceivedAsync"/>
        /// and this method doesn't get called.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <param name="result">The result.</param>
        /// <returns></returns>
        protected abstract Task MessageReceivedInternalAsync(IDialogContext context, IAwaitable<IMessageActivity> result);

        /// <summary>
        /// Handles the response by checking if it is special command.
        /// Returns true if response is a special command, false otherwise.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <param name="response">The response.</param>
        /// <returns>The asynchronous task.</returns>
        internal async Task<bool> HandleSpecialCommandResponse(IDialogContext context, LocationDialogResponse response)
        {
            // If response is null or cancel, pass it up to parent dialog.
            if (response == null || response.SpecialCommand == SpecialCommand.Cancel)
            {
                context.Done<T>(null);
                return true;
            }
            // If response is a reset, check whether this is the root dialog or not
            // if yes, claim it and rerun the start method, otherwise pass it up
            // to parent dialog to handle it.
            if (response.SpecialCommand == SpecialCommand.Reset)
            {
                if (!this.IsRootDialog)
                {
                    context.Done(response);
                }
                else
                {
                    await this.StartAsync(context);
                }

                return true;
            }

            return false;
        }
    }
}
