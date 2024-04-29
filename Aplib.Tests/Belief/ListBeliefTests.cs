using Aplib.Core.Belief;
using System.Collections.Generic;

namespace Aplib.Core.Tests.Belief;

/// <summary>
/// Unit tests for <see cref="ListBelief{TReference,TObservation}" />.
/// </summary>
public class ListBeliefTests
{
    /// <summary>
    /// A constant 'false' method for testing.
    /// </summary>
    /// <returns><c>false</c>.</returns>
    private static bool NeverUpdate() => false;

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
    /// Given a ListBelief with an shouldUpdate condition that is not satisfied,
    /// When UpdateBelief is called,
    /// Then the observation is not updated.
    /// </summary>
    [Fact]
    public void ListBelief_ShouldUpdateConditionIsNotSatisfied_DoesNotUpdateObservation()
    {
        // Arrange
        List<string> strings = ["foo", "bar"];
        // Observation: What is the last character?
        ListBelief<string, char> belief = new(strings, str => str[^1], NeverUpdate);

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
        belief.UpdateBelief();

        // Assert
        Assert.Equal(new List<byte>(), belief);
    }

    // [Fact]
    // public void Bug()
    // {
    //     // Arrange
    //     MyEnumerable value = new(1);
    //     // The bug is the fact that we can get around the constraint that `TReference` should be a reference type.
    //     Belief<IEnumerable<int>, List<int>> belief = new(value, vs => vs.ToList());
    //
    //     // Act
    //     value.Number = 2;
    //     belief.UpdateBelief();
    //
    //     // Assert
    //     Assert.Equal(new List<int> { 2, 2, 2 }, belief);
    // }
    //
    // private struct MyEnumerable : IEnumerable<int>
    // {
    //     public int Number { get; set; }
    //
    //     private const int MAX = 3;
    //
    //     public MyEnumerable(int number) => Number = number;
    //
    //     public IEnumerator<int> GetEnumerator()
    //     {
    //         for (int i = 0; i < MAX; i++) yield return Number;
    //     }
    //
    //     IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    // }
}
