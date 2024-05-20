using Aplib.Core.Belief.Beliefs;
using System.Collections.Generic;

namespace Aplib.Core.Tests.Belief;

/// <summary>
/// Unit tests for <see cref="ListBelief{TReference,TObservation}" />.
/// </summary>
public class ListBeliefTests
{
    /// <summary>
    /// Given a ListBelief without an explicit shouldUpdate parameter,
    /// When UpdateBelief is called,
    /// Then the observation is updated.
    /// </summary>
    [Fact]
    public void ListBelief_WithoutShouldUpdate_UpdatesObservation()
    {
        // Arrange
        int[] numbers = [1, 1, 2, 3, 5, 8];

        // Observation: Is the number even?
        ListBelief<int, bool> belief = new(numbers, i => i % 2 == 0);

        // Act
        numbers[0] = 0;
        belief.UpdateBelief();

        // Assert
        List<bool> expected = [true, false, true, false, false, true];
        Assert.Equal(expected, belief);
    }

    /// <summary>
    /// Given a ListBelief with a shouldUpdate condition that is not satisfied,
    /// When UpdateBelief is called,
    /// Then the observation is not updated.
    /// </summary>
    [Fact]
    public void ListBelief_ShouldUpdateConditionIsNotSatisfied_DoesNotUpdateObservation()
    {
        // Arrange
        List<string> strings = ["foo", "bar"];

        // Observation: What is the last character?
        ListBelief<string, char> belief = new(strings, str => str[^1], () => false);

        // Act
        // Append 'x' to each string
        for (int i = 0; i < strings.Count; i++) strings[i] += 'x';
        belief.UpdateBelief();

        // Assert
        Assert.Equal(new List<char> { 'o', 'r' }, belief);
    }

    /// <summary>
    /// Given an empty collection,
    /// When a ListBelief is created from that collection,
    /// Then the observation is also empty
    /// </summary>
    [Fact]
    public void ListBelief_FromEmptyEnumerable_HasEmptyObservationList()
    {
        // Arrange
        // ReSharper disable once CollectionNeverUpdated.Local
        Stack<byte> stack = new();

        // Act
        ListBelief<byte, byte> belief = new(stack, b => b);

        // Assert
        Assert.Empty(belief.Observation);
    }
}
