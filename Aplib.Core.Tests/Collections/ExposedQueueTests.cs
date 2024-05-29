using Aplib.Core.Collections;
using System;
using System.Collections.Generic;

namespace Aplib.Core.Tests.Collections;

public class ExposedQueueTests
{
    [Fact]
    public void ExposedQueue_WhenInitialized_ShouldHaveCorrectMaxCountAndCount()
    {
        // Arrange
        ExposedQueue<int> queue = new(3);

        // Act
        int maxCount = queue.MaxCount;
        int count = queue.Count;

        // Assert
        Assert.Equal(3, maxCount);
        Assert.Equal(0, count);
    }

    [Fact]
    public void ExposedQueue_WhenInitializedWithArray_ShouldHaveCorrectMaxCountAndCount()
    {
        // Arrange
        int[] array = [1, 2, 3];
        const int expectedMaxCount = 3;
        const int expectedCount = 3;
        ExposedQueue<int> queue = new(array);

        // Act
        int maxCount = queue.MaxCount;
        int count = queue.Count;

        // Assert
        Assert.Equal(expectedMaxCount, maxCount);
        Assert.Equal(expectedCount, count);
    }

    [Fact]
    public void ExposedQueue_WhenInitializedWithArrayAndCount_ShouldHaveCorrectMaxCountAndCount()
    {
        // Arrange
        int[] array = [1, 2, 0, 0];
        const int countParam = 2;
        const int expectedMaxCount = 4;
        const int expectedCount = 2;
        ExposedQueue<int> queue = new(array, countParam);

        // Act
        int maxCount = queue.MaxCount;
        int count = queue.Count;

        // Assert
        Assert.Equal(expectedMaxCount, maxCount);
        Assert.Equal(expectedCount, count);
    }

    [Fact]
    public void ExposedQueue_WhenInitializedWithArrayAndCount_ShouldSetHeadCorrectly()
    {
        // Arrange
        ExposedQueue<int> queue = new([1, 0, 0], 1);
        ExposedQueue<int> expected = new([3, 2, 1]);

        // Act
        queue.Put(2);
        queue.Put(3);

        // Assert
        Assert.Equal(expected, queue);
    }

    [Fact]
    public void ExposedQueue_WhenInitializedWithCountExceedingLength_ShouldThrowException()
    {
        // Arrange
        void CountExceedsLength() => _ = new ExposedQueue<int>([1, 2, 3], 4);

        // Act & Assert
        Assert.Throws<ArgumentOutOfRangeException>(CountExceedsLength);
    }

    [Fact]
    public void ExposedQueue_WhenInitializedWithNegativeCount_ShouldThrowException()
    {
        // Arrange
        void NegativeCount() => _ = new ExposedQueue<int>([1, 2, 3], -1);

        // Act & Assert
        Assert.Throws<ArgumentOutOfRangeException>(NegativeCount);
    }

    [Fact]
    public void AccessIndex_WhenIndexIsNegative_ThrowsException()
    {
        // Arrange
        void AccessNegativeIndex() => _ = new ExposedQueue<int>(3)[-1];

        // Act & Assert
        Assert.Throws<ArgumentOutOfRangeException>(AccessNegativeIndex);
    }

    [Fact]
    public void AccessIndex_WhenIndexIsGreaterThanCount_ThrowsException()
    {
        // Arrange
        void AccessIndexGreaterThanCount() => _ = new ExposedQueue<int>(3)[4];

        // Act & Assert
        Assert.Throws<ArgumentOutOfRangeException>(AccessIndexGreaterThanCount);
    }


    [Fact]
    public void Put_ArrayIsFull_WrapsAround()
    {
        // Arrange
        ExposedQueue<int> queue = new([4, 3, 2, 1]);
        ExposedQueue<int> expected = new([5, 4, 3, 2]);

        // Act
        queue.Put(5);

        // Assert
        Assert.Equal(expected.Count, queue.Count);
        Assert.Equal(expected, queue);
    }

    [Fact]
    public void GetLast_HeadIsUpdated_ReturnsCorrectElement()
    {
        // Arrange
        ExposedQueue<int> queue = new([3, 2, 1]);

        // Act
        queue.Put(4);

        // Assert
        Assert.Equal(2, queue.GetLast());
    }

    [Fact]
    public void GetFirst_HeadIsUpdated_ReturnsFirstElement()
    {
        // Arrange
        ExposedQueue<int> queue = new([3, 2, 1]);

        // Act
        queue.Put(4);

        // Assert
        Assert.Equal(4, queue.GetFirst());
    }

    [Fact]
    public void CopyTo_WhenStartIndexIsNegative_ThrowsException()
    {
        // Arrange
        ExposedQueue<int> queue = new([3, 2, 1]);

        // Assert
        Assert.Throws<ArgumentOutOfRangeException>(() => queue.CopyTo(new int[3], -1, 2));
    }

    [Fact]
    public void CopyTo_WhenEndIndexIsOutOfBounds_ThrowsException()
    {
        // Arrange
        ExposedQueue<int> queue = new([3, 2, 1]);

        // Assert
        Assert.Throws<ArgumentOutOfRangeException>(() => queue.CopyTo(new int[3], 0, 3));
    }

    [Fact]
    public void CopyTo_WhenStartIndexIsAfterEndIndex_ThrowsException()
    {
        // Arrange
        ExposedQueue<int> queue = new([3, 2, 1]);

        // Assert
        Assert.Throws<ArgumentException>(() => queue.CopyTo(new int[3], 2, 1));
    }

    [Fact]
    public void CopyTo_WhenCalled_CopiesCorrectly()
    {
        // Arrange
        ExposedQueue<int> queue = new([3, 2, 1]);

        // Act
        int[] array = new int[3];
        queue.CopyTo(array, 0, 2);

        // Assert
        Assert.Equal(array, [3, 2, 1]);
    }

    [Fact]
    public void CopyTo_DefaultEndIndex_CopiesCorrectly()
    {
        // Arrange
        ExposedQueue<int> queue = new([3, 2, 1]);

        // Act
        int[] array = new int[3];
        queue.CopyTo(array, 0);

        // Assert
        Assert.Equal(array, [3, 2, 1]);
    }

    [Fact]
    public void ToArray_WhenCalled_ReturnsCorrectArray()
    {
        // Arrange
        ExposedQueue<int> queue = new([3, 2, 1]);

        // Act
        int[] array = queue.ToArray();

        // Assert
        Assert.Equal(array, [3, 2, 1]);
    }

    [Fact]
    public void Clear_QueueFilled_ClearsQueue()
    {
        // Arrange
        ExposedQueue<int> queue = new([3, 2, 1]);

        // Act
        queue.Clear();

        // Assert
        Assert.Empty(queue);
    }

    [Theory]
    [InlineData(2, true)]
    [InlineData(4, false)]
    public void Contains_OnElement_ReturnsCorrectAnswer(int element, bool expected)
    {
        // Arrange
        ExposedQueue<int> queue = new([3, 2, 1]);

        // Act
        bool contains = queue.Contains(element);

        // Assert
        Assert.Equal(expected, contains);
    }

    [Fact]
    public void Remove_WhenCalledOnMissingElement_ReturnsFalse()
    {
        // Arrange
        ExposedQueue<int> queue = new([3, 2, 1]);

        // Act
        bool removed = queue.Remove(4);

        // Assert
        Assert.False(removed);
    }

    [Fact]
    public void Remove_WhenCalledOnExistingElement_RemovesElement()
    {
        // Arrange
        ExposedQueue<int> queue = new([4, 3, 2, 1]);
        ExposedQueue<int> expected = new([4, 2, 1]);

        // Act
        bool removed = queue.Remove(3);

        // Assert
        Assert.True(removed);
        Assert.Equal(expected.Count, queue.Count);
        Assert.Equal(expected, queue);
    }

    [Fact]
    public void Put_AfterRemoval_StillHasCorrectHead()
    {
        // Arrange
        ExposedQueue<int> queue = new([4, 3, 1, 2]);
        ExposedQueue<int> expected = new([5, 4, 3, 2]);

        // Act
        queue.Remove(1);
        queue.Put(5);

        // Assert
        Assert.Equal(expected.Count, queue.Count);
        Assert.Equal(expected, queue);
    }

    [Fact]
    public void GetEnumerator_ReturnsCorrectEnumerator()
    {
        // Arrange
        ExposedQueue<int> queue = new([3, 2, 1]);

        // Act
        List<int> list = new();
        foreach (int item in queue)
            list.Add(item);

        // Assert
        Assert.Equal(list, [3, 2, 1]);
    }
}
