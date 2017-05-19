namespace Microsoft.Bot.Builder.Location.Tests
{
    using System.Threading;
    using System.Threading.Tasks;
    using Builder.Dialogs;
    using Connector;
    using Moq;
    using VisualStudio.TestTools.UnitTesting;

    [TestClass]
    public class SpecialCommandsTests
    {
        [TestMethod]
        public async Task If_Help_Command_Post_Help_Message()
        {
            // Arrange
            var dialog = new LocationDialog(string.Empty, "facebook", string.Empty, LocationOptions.UseNativeControl);

            var context = this.GetSetupMockObject();

            // Act
            await dialog.MessageReceivedAsync(context.Object, TestHelper.CreateAwaitableMessage("help"));

            // Assert
            var locationResourceManager = new LocationResourceManager();
            context.Verify(c => c.PostAsync(It.Is<IMessageActivity>(a => a.Text == locationResourceManager.HelpMessage), It.IsAny<CancellationToken>()), Times.Once);
        }

        [TestMethod]
        public async Task If_Reset_Command_Call_StartAsync()
        {
            // Arrange
            string prompt = "Where do you want to ship your widget?";
            var dialog = new LocationDialog(string.Empty, "facebook", prompt, LocationOptions.None);

            var context = this.GetSetupMockObject();

            // Act
            await dialog.MessageReceivedAsync(context.Object, TestHelper.CreateAwaitableMessage("reset"));

            // Assert
            var locationResourceManager = new LocationResourceManager();
            context.Verify(c => c.PostAsync(It.Is<IMessageActivity>(a => a.Text == locationResourceManager.ResetPrompt), It.IsAny<CancellationToken>()), Times.Once);
        }

        [TestMethod]
        public async Task If_Cancel_Command_Call_Context_Done()
        {
            // Arrange
            var dialog = new LocationDialog(string.Empty, "facebook", string.Empty, LocationOptions.UseNativeControl);

            var context = this.GetSetupMockObject();

            // Act
            await dialog.MessageReceivedAsync(context.Object, TestHelper.CreateAwaitableMessage("cancel"));

            // Assert
            context.Verify(c => c.Done(It.Is<Place>(v => v == null)));
        }

        private Mock<IDialogContext> GetSetupMockObject()
        {
            var context = new Mock<IDialogContext>(MockBehavior.Loose);

            context.Setup(c => c.UserData).Returns(new Mock<IBotDataBag>().Object);
            context.Setup(c => c.MakeMessage()).Returns(() => new Activity());

            return context;
        }
    }
}