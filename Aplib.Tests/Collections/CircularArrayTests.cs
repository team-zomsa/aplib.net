using Aplib.Core.Collections;

namespace Aplib.Core.Tests.Collections;

public class CircularArrayTests
{
    /// <summary>
    /// Given a CircularArray instance,
    /// When an element is put into the array,
    /// The array should wrap around when it reaches its end.
    /// (i.e., the first element should become the previous second element, 
    /// and the last element should be the new element)
    /// </summary>
    [Fact]
    public void Put_ArrayIsFull_WrapsAround()
    {
        // Arrange
        CircularArray<int> circularArray = new([1, 2, 3, 4, 5]);

        // Act
        circularArray.Put(0);

        // Assert
        Assert.Equal(0, circularArray[0]);
        Assert.Equal(1, circularArray[1]);
        Assert.Equal(2, circularArray[2]);
    }

    /// <summary>
    /// Given a CircularArray instance,
    /// When the head is updated,
    /// Putting an element should set the correct index 
    /// even if the head is not at the start of the array.
    /// </summary>
    [Fact]
    public void Put_HeadIsUpdated_SetsCorrectIndex()
    {
        // Arrange
        CircularArray<int> circularArray = new(3);

        // Act
        // circularArray.ToArray() == [0, 0, 0]
        circularArray[1] = 6;
        // circularArray.ToArray() == [0, 6, 0]
        circularArray.Put(4);
        // circularArray.ToArray() == [4, 0, 6]
        circularArray[1] = 5;
        // circularArray.ToArray() == [4, 5, 6]

        // Assert
        Assert.Equal(4, circularArray[0]);
        Assert.Equal(5, circularArray[1]);
        Assert.Equal(6, circularArray[2]);
    }

    /// <summary>
    /// Given a CircularArray instance,
    /// When the head is updated,
    /// GetHead should return the correct head.
    /// </summary>
    [Fact]
    public void GetHead_HeadIsUpdated_ReturnsLastElement()
    {
        // Arrange
        CircularArray<int> circularArray = new([1, 2, 3]);
        int prevHead = circularArray.GetHead();

        // Act
        circularArray.Put(0);
        int head = circularArray.GetHead();

        // Assert
        Assert.NotEqual(prevHead, head);
        Assert.Equal(2, head);
    }

    /// <summary>
    /// Given a CircularArray instance,
    /// When the head is updated,
    /// GetLast should return the last element.
    /// </summary>
    [Fact]
    public void GetFirst_HeadIsUpdated_ReturnsFirstElement()
    {
        // Arrange
        CircularArray<int> circularArray = new([1, 2, 3]);
        int prevFirst = circularArray.GetFirst();

        // Act
        circularArray.Put(4);
        int firstElement = circularArray.GetFirst();

        // Assert
        Assert.NotEqual(prevFirst, firstElement);
        Assert.Equal(4, firstElement);
    }

    /// <summary>
    /// Given a CircularArray instance,
    /// When the head is updated,
    /// Converts the circular array to an array in the correct order.
    /// </summary>
    [Fact]
    public void ToArray_HeadIsUpdated_ReturnsCorrectlyOrderedArray()
    {
        // Arrange
        CircularArray<int> circularArray = new([1, 2, 3, 4, 5]);

        // Act
        circularArray.Put(0);
        int[] array = circularArray.ToArray(1, 3);

        // Assert
        Assert.Equal([1, 2, 3], array);
    }
}
