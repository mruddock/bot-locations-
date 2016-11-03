namespace Microsoft.Bot.Builder.Location.Tests
{
    using Connector;
    using Dialogs;
    using Internals.Fibers;
    using Moq;

    internal static class TestHelper
    {
        internal static IAwaitable<T> CreateAwaitable<T>(T result)
        {
            var awaiter = new Mock<IAwaiter<T>>();
            awaiter.Setup(a => a.GetResult()).Returns(() => result);
            awaiter.SetupGet(a => a.IsCompleted).Returns(() => true);

            var awaitable = new Mock<IAwaitable<T>>();
            awaitable.Setup(a => a.GetAwaiter()).Returns(() => awaiter.Object);

            return awaitable.Object;
        }

        internal static IAwaitable<IMessageActivity> CreateAwaitableMessage(string text)
        {
            return TestHelper.CreateAwaitable<IMessageActivity>(new Activity { Text = text });
        }
    }
}
