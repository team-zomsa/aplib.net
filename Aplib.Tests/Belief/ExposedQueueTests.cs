using System;
using System.Collections.Generic;

namespace Aplib.Core.Tests.Belief;

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
        ExposedQueue<int> queue = new([1, 2, 3]);

        // Act
        int maxCount = queue.MaxCount;
        int count = queue.Count;

        // Assert
        Assert.Equal(3, maxCount);
        Assert.Equal(3, count);
    }

    [Fact]
    public void ExposedQueue_WhenInitializedWithArrayAndCount_ShouldThrowExceptions()
    {
        // Arrange
        void CountExceedsLength() => _ = new ExposedQueue<int>([1, 2, 3], 4);
        void NegativeCount() => _ = new ExposedQueue<int>([1, 2, 3], -1);

        // Assert
        Assert.Throws<ArgumentOutOfRangeException>(CountExceedsLength);
        Assert.Throws<ArgumentOutOfRangeException>(NegativeCount);
    }

    [Fact]
    public void AccessIndex_WhenIndexIsOutOfBounds_ThrowsException()
    {
        // Arrange
        ExposedQueue<int> queue = new(3);
        int elem;

        // Act
        void AccessNegativeIndex() => elem = queue[-1];
        void AccessIndexGreaterThanCount() => elem = queue[queue.Count];

        // Assert
        Assert.Throws<ArgumentOutOfRangeException>(AccessNegativeIndex);
        Assert.Throws<ArgumentOutOfRangeException>(AccessIndexGreaterThanCount);
    }

    [Fact]
    public void Put_ArrayIsFull_WrapsAround()
    {
        // Arrange
        ExposedQueue<int> queue = new([4, 3, 2, 1]);

        // Act
        queue.Put(5);

        // Assert
        Assert.Equal(5, queue[0]);
        Assert.Equal(4, queue[1]);
        Assert.Equal(2, queue[3]);
    }

    [Fact]
    public void GetHead_HeadIsUpdated_ReturnsCorrectElement()
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
    public void CopyTo_WrongArguments_ThrowsException()
    {
        // Arrange
        ExposedQueue<int> queue = new([3, 2, 1]);

        // Act
        void StartOutOfBounds() => queue.CopyTo(new int[3], -1, 2);
        void EndOutOfOunds() => queue.CopyTo(new int[3], 0, 3);
        void StartAfterEnd() => queue.CopyTo(new int[3], 2, 1);

        // Assert
        Assert.Throws<ArgumentOutOfRangeException>(StartOutOfBounds);
        Assert.Throws<ArgumentOutOfRangeException>(EndOutOfOunds);
        Assert.Throws<ArgumentException>(StartAfterEnd);
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

    [Fact]
    public void Contains_ElementExists_ReturnsTrue()
    {
        // Arrange
        ExposedQueue<int> queue = new([3, 2, 1]);

        // Act
        bool contains = queue.Contains(2);

        // Assert
        Assert.True(contains);
    }

    [Fact]
    public void Contains_ElementDoesNotExist_ReturnsFalse()
    {
        // Arrange
        ExposedQueue<int> queue = new([3, 2, 1]);

        // Act
        bool contains = queue.Contains(4);

        // Assert
        Assert.False(contains);
    }

    [Fact]
    public void Remove_WhenCalled_ReturnsFalse()
    {
        // Arrange
        ExposedQueue<int> queue = new([3, 2, 1]);

        // Act
        bool removed = queue.Remove(4);

        // Assert
        Assert.False(removed);
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
