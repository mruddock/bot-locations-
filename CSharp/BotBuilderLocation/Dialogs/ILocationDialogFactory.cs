namespace Microsoft.Bot.Builder.Location.Dialogs
{
    using Builder.Dialogs;

    /// <summary>
    /// Represents the interface that defines how location (sub)dialogs are created.
    /// </summary>
    internal interface ILocationDialogFactory
    {
        /// <summary>
        /// Given a branch parameter, creates the appropriate corresponding dialog that should run.
        /// </summary>
        /// <param name="branch">The location dialog branch.</param>
        /// <returns>The dialog.</returns>
        IDialog<LocationDialogResponse> CreateDialog(BranchType branch);
    }
}
