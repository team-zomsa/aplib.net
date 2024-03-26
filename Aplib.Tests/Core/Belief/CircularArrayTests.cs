using Aplib.Core;

namespace Aplib.Tests.Core.Belief;

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
    public void Put_ArrayIsFull_UpdatesHead() 
    {
        // Arrange
        CircularArray<int> circularArray = new([1, 2, 3]);

        // Act
        circularArray.Put(4);

        // Assert
        Assert.Equal(2, circularArray[0]);
        Assert.Equal(3, circularArray[1]);
        Assert.Equal(4, circularArray[2]);
    }

    /// <summary>
    /// Given a CircularArray instance,
    /// When the head is updated,
    /// Setting an element should set the correct index 
    /// even if the head is not at the start of the array.
    /// </summary>
    [Fact]
    public void Set_HeadIsUpdated_SetsCorrectIndex()
    {
        // Arrange
        CircularArray<int> circularArray = new(3);

        // Act
        circularArray[1] = 4;
        circularArray.Put(6);
        circularArray[1] = 5;

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
    public void GetHead_HeadIsUpdated_IsCorrect()
    {
        // Arrange
        CircularArray<int> circularArray = new([1, 2, 3]);
        int prevHead = circularArray.GetHead();

        // Act
        circularArray.Put(4);
        int head = circularArray.GetHead();

        // Assert
        Assert.NotEqual(prevHead, head);
        Assert.Equal(2, head);
    }
}