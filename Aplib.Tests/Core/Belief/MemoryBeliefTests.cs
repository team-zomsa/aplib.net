using Aplib.Core;
using Aplib.Core.Belief;

namespace Aplib.Tests.Core.Belief;

/// <summary>
/// Describes a set of tests for the <see cref="MemoryBelief{TReference,TObservation}"/> class.
/// </summary>
public class MemoryBeliefTests
{
    /// <summary>
    /// Given a MultipleMemoryBelief instance with an observation,
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
        list[0] = 2;
        belief.UpdateBelief();

        // Assert
        Assert.Equal(1, belief.GetMostRecentMemory());
    }
}
