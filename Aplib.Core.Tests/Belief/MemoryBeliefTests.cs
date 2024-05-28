using Aplib.Core.Belief.Beliefs;
using FluentAssertions;
using System;
using System.Collections.Generic;

namespace Aplib.Core.Tests.Belief;

/// <summary>
/// Describes a set of tests for the <see cref="MemoryBelief{TReference,TObservation}"/> class.
/// </summary>
public class MemoryBeliefTests
{
    /// <summary>
    /// Given no metadata,
    /// When a MemoryBelief is constructed,
    /// Then it gets a random id and no name or description.
    /// </summary>
    [Fact]
    public void MemoryBelief_ConstructedWithoutMetadata_ContainsDefaultMetadata()
    {
        // Arrange
        List<int> list = [1, 2, 3];

        // Act
        MemoryBelief<List<int>, int> belief = new(list, reference => reference.Count, 3, () => true);

        // Assert
        belief.Metadata.Id.Should().NotBeEmpty();
        belief.Metadata.Name.Should().BeNull();
        belief.Metadata.Description.Should().BeNull();
    }

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
        Assert.Equal(4, belief.GetMemoryAt(0));
        Assert.Equal(3, belief.GetMemoryAt(1));
    }

    /// <summary>
    /// Given a MemoryBelief instance with an observation,
    /// When asking for an index that is out of bounds,
    /// Then an exception should be thrown.
    /// </summary>
    [Fact]
    public void GetMemoryAt_IndexOutOfBounds_ShouldThrowException()
    {
        // Arrange
        List<int> list = [1, 2, 3];
        MemoryBelief<List<int>, int> belief = new(list, reference => reference.Count, 3);

        // Act
        void GetMemoryAtNegativeIndex() => belief.GetMemoryAt(-1);
        void GetMemoryAtIndexGreaterThanCount() => belief.GetMemoryAt(3);

        // Assert
        Assert.Throws<ArgumentOutOfRangeException>(GetMemoryAtNegativeIndex);
        Assert.Throws<ArgumentOutOfRangeException>(GetMemoryAtIndexGreaterThanCount);
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
        Assert.Equal([3], belief.GetAllMemories());
    }
}
