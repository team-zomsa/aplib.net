namespace Aplib.Core.Tests.Belief;

public class ExposedQueueTests
{
    [Fact]
    public void ExposedQueue_WhenInitialized_ShouldHaveCorrectMaxCount()
    {
        // Arrange
        ExposedQueue<int> queue = new(3);

        // Act
        int maxCount = queue.MaxCount;

        // Assert
        Assert.Equal(3, maxCount);
    }
}
