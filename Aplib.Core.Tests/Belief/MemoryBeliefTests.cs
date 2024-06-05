using Aplib.Core.Belief.Beliefs;
using Aplib.Core.Collections;
using FluentAssertions;
using Moq;
using System;
using System.Collections.Generic;

namespace Aplib.Core.Tests.Belief;

/// <summary>
/// Describes a set of tests for the <see cref="MemoryBelief{TReference,TObservation}"/> class.
/// </summary>
public class MemoryBeliefTests
{
    public class TestMemoryBelief : MemoryBelief<object, object>
    {
        public object Reference => _reference;

        public Func<object, object> GetObservationFromReference => _getObservationFromReference;

        public Func<bool> ShouldUpdate => _shouldUpdate;

        public ExposedQueue<object> MemorizedObservations => _memorizedObservations;

        public TestMemoryBelief
        (
            Metadata metadata,
            object reference,
            Func<object, object> getObservationFromReference,
            int framesToRemember,
            Func<bool> shouldUpdate
        )
            : base(metadata, reference, getObservationFromReference, framesToRemember, shouldUpdate)
        {
        }

        public TestMemoryBelief
        (
            object reference,
            Func<object, object> getObservationFromReference,
            int framesToRemember,
            Func<bool> shouldUpdate
        )
            : base(reference, getObservationFromReference, framesToRemember, shouldUpdate)
        {
        }

        public TestMemoryBelief
        (
            Metadata metadata,
            object reference,
            Func<object, object> getObservationFromReference,
            int framesToRemember
        )
            : base(metadata, reference, getObservationFromReference, framesToRemember)
        {
        }

        public TestMemoryBelief
            (object reference, Func<object, object> getObservationFromReference, int framesToRemember)
            : base(reference, getObservationFromReference, framesToRemember)
        {
        }
    }

    [Fact]
    public void MemoryBelief_WhenConstructed_HasExpectedData()
    {
        // Arrange
        Metadata metadata = It.IsAny<Metadata>();
        object reference = new Mock<object>().Object;
        Func<object, object> getObservationFromReference = new Mock<Func<object, object>>().Object;
        const int framesToRemember = 0;
        Func<bool> shouldUpdate = It.IsAny<Func<bool>>();

        // Act
        TestMemoryBelief belief = new(metadata, reference, getObservationFromReference, framesToRemember, shouldUpdate);

        // Assert
        belief.Metadata.Should().Be(metadata);
        belief.Reference.Should().Be(reference);
        belief.GetObservationFromReference.Should().Be(getObservationFromReference);
        belief.MemorizedObservations.MaxCount.Should().Be(framesToRemember);
        ((object)belief.ShouldUpdate).Should().Be(shouldUpdate);
    }

    [Fact]
    public void MemoryBelief_WithoutMetadata_HasExpectedData()
    {
        // Arrange
        object reference = new Mock<object>().Object;
        Func<object, object> getObservationFromReference = new Mock<Func<object, object>>().Object;
        const int framesToRemember = 1;
        Func<bool> shouldUpdate = It.IsAny<Func<bool>>();

        // Act
        TestMemoryBelief belief = new(reference, getObservationFromReference, framesToRemember, shouldUpdate);

        // Assert
        belief.Metadata.Id.Should().NotBeEmpty();
        belief.Metadata.Name.Should().BeNull();
        belief.Metadata.Description.Should().BeNull();
        belief.Reference.Should().Be(reference);
        belief.GetObservationFromReference.Should().Be(getObservationFromReference);
        belief.MemorizedObservations.MaxCount.Should().Be(framesToRemember);
        ((object)belief.ShouldUpdate).Should().Be(shouldUpdate);
    }

    [Fact]
    public void MemoryBelief_WithoutShouldUpdate_HasExpectedData()
    {
        // Arrange
        Metadata metadata = It.IsAny<Metadata>();
        object reference = new Mock<object>().Object;
        Func<object, object> getObservationFromReference = new Mock<Func<object, object>>().Object;
        const int framesToRemember = 2;

        // Act
        TestMemoryBelief belief = new(metadata, reference, getObservationFromReference, framesToRemember);

        // Assert
        belief.Metadata.Should().Be(metadata);
        belief.Reference.Should().Be(reference);
        belief.GetObservationFromReference.Should().Be(getObservationFromReference);
        belief.MemorizedObservations.MaxCount.Should().Be(framesToRemember);
        belief.ShouldUpdate().Should().BeTrue();
    }

    [Fact]
    public void MemoryBelief_WithoutMetadataWithoutShouldUpdate_HasExpectedData()
    {
        // Arrange
        object reference = new Mock<object>().Object;
        Func<object, object> getObservationFromReference = new Mock<Func<object, object>>().Object;
        const int framesToRemember = 3;

        // Act
        TestMemoryBelief belief = new(reference, getObservationFromReference, framesToRemember);

        // Assert
        belief.Metadata.Id.Should().NotBeEmpty();
        belief.Metadata.Name.Should().BeNull();
        belief.Metadata.Description.Should().BeNull();
        belief.Reference.Should().Be(reference);
        belief.GetObservationFromReference.Should().Be(getObservationFromReference);
        belief.MemorizedObservations.MaxCount.Should().Be(framesToRemember);
        belief.ShouldUpdate().Should().BeTrue();
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
