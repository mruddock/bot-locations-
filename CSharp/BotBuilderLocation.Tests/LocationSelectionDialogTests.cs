namespace Microsoft.Bot.Builder.Location.Tests
{
    using System.Threading;
    using System.Threading.Tasks;
    using Builder.Dialogs;
    using Connector;
    using Dialogs;
    using Moq;
    using VisualStudio.TestTools.UnitTesting;

    [TestClass]
    public class LocationSelectionDialogTests
    {
        [TestMethod]
        public async Task If_FacebookChannel_And_NativeControlOption_Call_FacebookLocationDialog()
        {
            // Arrange
            var dialog = new LocationDialog(string.Empty, "facebook", string.Empty, LocationOptions.UseNativeControl);
            var context = this.GetSetupMockObject();

            // Act
            await dialog.StartAsync(context.Object);

            // Assert
            context.Verify(c => c.Call(It.IsAny<FacebookNativeLocationRetrieverDialog>(), It.IsAny<ResumeAfter<LocationDialogResponse>>()), Times.Once());
        }

        [TestMethod]
        public async Task If_FacebookChannel_And_NotNativeControlOption_Do_Not_Call_FacebookLocationDialog()
        {
            // Arrange
            var dialog = new LocationDialog(string.Empty, "facebook", string.Empty, LocationOptions.None);
            var context = this.GetSetupMockObject();

            context
                .Setup(c => c.MakeMessage())
                .Returns(() => new Activity());

            context
                .Setup(c => c.PostAsync(It.IsAny<IMessageActivity>(), It.IsAny<CancellationToken>()))
                .Returns(() => Task.CompletedTask);

            // Act
            await dialog.StartAsync(context.Object);

            // Assert
            context.Verify(c => c.Call(It.IsAny<FacebookNativeLocationRetrieverDialog>(), It.IsAny<ResumeAfter<LocationDialogResponse>>()), Times.Never);
        }

        [TestMethod]
        public async Task Should_Post_To_User_Passed_Prompt_When_Start_Called()
        {
            // Arrange
            string prompt = "Where do you want to ship your widget?";
            var dialog = new LocationDialog(string.Empty, "facebook", prompt, LocationOptions.None);
            var context = this.GetSetupMockObject();

            context
                .Setup(c => c.MakeMessage())
                .Returns(() => new Activity());

            context
                .Setup(c => c.PostAsync(It.IsAny<IMessageActivity>(), It.IsAny<CancellationToken>()))
                .Returns(() => Task.CompletedTask);

            context
                .Setup(c => c.Call(It.IsAny<IDialog<LocationDialogResponse>>(), It.IsAny<ResumeAfter<LocationDialogResponse>>()))
                .Callback<IDialog<LocationDialogResponse>, ResumeAfter<LocationDialogResponse>>(async (d, c) => await d.StartAsync(context.Object));

            // Act
            await dialog.StartAsync(context.Object);

            // Assert
            var locationResourceManager = new LocationResourceManager();

            context.Verify(c => c.Call(It.IsAny<FacebookNativeLocationRetrieverDialog>(), It.IsAny<ResumeAfter<LocationDialogResponse>>()), Times.Never);
            context.Verify(c => c.PostAsync(It.Is<IMessageActivity>(a => a.Text == prompt + locationResourceManager.TitleSuffix), It.IsAny<CancellationToken>()), Times.Once);
        }

        private Mock<IDialogContext> GetSetupMockObject()
        {
            var context = new Mock<IDialogContext>(MockBehavior.Loose);
            context.Setup(c => c.UserData).Returns(new Mock<IBotDataBag>().Object);

            return context;
        }
    }
}