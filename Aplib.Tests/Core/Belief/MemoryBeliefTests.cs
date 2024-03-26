using Aplib.Core;
using Aplib.Core.Belief;

namespace Aplib.Tests.Core.Belief;

/// <summary>
/// Describes a set of tests for the <see cref="MemoryBelief{TReference,TObservation}"/> class.
/// </summary>
public class MemoryBeliefTests
{
    /// <summary>
    /// Given a MemoryBelief instance with an observation,
    /// When the observation is updated and GetMostRecentMemory is called,
    /// Then the last observation is returned.
    /// </summary>
    [Fact]
    public void GetMostRecentMemory_WhenObservationIsUpdated_ShouldReturnLastObservation()
    {
        // Arrange
        List<int> list = [1];
        MemoryBelief<List<int>, int> belief = new(list, reference => reference.Count, 1);

        // Act
        list.Add(2);
        belief.UpdateBelief();

        // Assert
        Assert.Equal(1, belief.GetMostRecentMemory());
    }

    /// <summary>
    /// Given a MemoryBelief instance with an observation,
    /// When the observation is updated and GetMemoryAt is called with an index,
    /// Then the observation at the specified index is returned.
    /// </summary>
    [Fact]
    public void GetMemoryAt_WhenObservationIsUpdated_ShouldReturnObservationAtSpecifiedIndex()
    {
        // Arrange
        List<int> list = [1, 2, 3];
        MemoryBelief<List<int>, int> belief = new(list, reference => reference.Count, 3);

        // Act
        list.Add(4);
        belief.UpdateBelief();
        list.Add(5);
        belief.UpdateBelief();

        // Assert
        Assert.Equal(3, belief.GetMemoryAt(1));
        Assert.Equal(4, belief.GetMemoryAt(2));
    }

    /// <summary>
    /// Given a MemoryBelief instance with an observation,
    /// When the observation is updated and GetAllMemories is called,
    /// Then all the currently saved observations are returned.
    /// </summary>
    [Fact]
    public void GetAllMemories_ReturnsAllMemories()
    {
        // Arrange
        List<int> list = [1, 2, 3];
        MemoryBelief<List<int>, int> belief = new(list, reference => reference.Count, 3);

        // Act
        list.Add(4);
        belief.UpdateBelief();

        // Assert
        Assert.Equal([0, 0, 3], belief.GetAllMemories());
    }
}
