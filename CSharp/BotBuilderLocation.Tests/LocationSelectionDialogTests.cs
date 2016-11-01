using System.Threading;
using System.Threading.Tasks;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Builder.Location.Channels;
using Microsoft.Bot.Connector;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace Microsoft.Bot.Builder.Location.Tests
{
    [TestClass]
    public class LocationSelectionDialogTests
    {
        [TestMethod]
        public async Task If_FacebookChannel_And_NativeControlOption_Call_FacebookLocationDialog()
        {
            // Arrange
            var dialog = new LocationDialog("facebook", string.Empty, LocationOptions.UseNativeControl);
            var context = new Mock<IDialogContext>(MockBehavior.Loose);

            // Act
            await dialog.StartAsync(context.Object);

            // Assert
            context.Verify(c => c.Call(It.IsAny<FacebookLocationDialog>(), It.IsAny<ResumeAfter<Bing.Location>>()), Times.Once());
        }

        [TestMethod]
        public async Task If_FacebookChannel_And_NotNativeControlOption_Do_Not_Call_FacebookLocationDialog()
        {
            // Arrange
            var dialog = new LocationDialog("facebook", string.Empty, LocationOptions.None);
            var context = new Mock<IDialogContext>(MockBehavior.Loose);

            context
                .Setup(c => c.MakeMessage())
                .Returns(() => new Activity());

            context
                .Setup(c => c.PostAsync(It.IsAny<IMessageActivity>(), It.IsAny<CancellationToken>()))
                .Returns(() => Task.CompletedTask);

            // Act
            await dialog.StartAsync(context.Object);

            // Assert
            context.Verify(c => c.Call(It.IsAny<FacebookLocationDialog>(), It.IsAny<ResumeAfter<Bing.Location>>()), Times.Never);
        }

        [TestMethod]
        public async Task Should_Post_To_User_Passed_Prompt_When_Start_Called()
        {
            // Arrange
            string prompt = "Where do you want to ship your widget?";
            var dialog = new LocationDialog("facebook", prompt, LocationOptions.None);
            var context = new Mock<IDialogContext>(MockBehavior.Loose);

            context
                .Setup(c => c.MakeMessage())
                .Returns(() => new Activity());

            context
                .Setup(c => c.PostAsync(It.IsAny<IMessageActivity>(), It.IsAny<CancellationToken>()))
                .Returns(() => Task.CompletedTask);

            // Act
            await dialog.StartAsync(context.Object);

            // Assert
            context.Verify(c => c.PostAsync(It.Is<IMessageActivity>(a => a.Text == prompt), It.IsAny<CancellationToken>()), Times.Once);
        }
    }
}