using Aplib.Core.Belief;
using System.Linq;

namespace Aplib.Core.Tests.Core.Belief;

/// <summary>
/// Describes a set of tests for the <see cref="Belief{TReference,TObservation}"/> class.
/// </summary>
public class BeliefTests
{
    /// <summary>
    /// A constant 'true' method for testing.
    /// </summary>
    /// <returns>True.</returns>
    private static bool AlwaysUpdate() => true;

    /// <summary>
    /// A constant 'false' method for testing.
    /// </summary>
    /// <returns>False.</returns>
    private static bool NeverUpdate() => false;

    /// <summary>
    /// Given a Belief instance,
    /// When it is assigned to a variable of its observation type,
    /// Then it is implicitly converted to its observation type.
    /// </summary>
    [Fact]
    public void Belief_AssignedToObservationType_IsCorrectlyImplicitlyConvertedToObservationType()
    {
        // Arrange
        string def = "def";
        Belief<string, string> belief = new(def, reference => reference);

        // Act
        string observation = belief;

        // Assert
        Assert.Equal(def, observation);
    }

    /// <summary>
    /// Given a Belief instance with an shouldUpdate condition that is satisfied,
    /// When UpdateBelief is called,
    /// Then the observation is updated.
    /// </summary>
    [Fact]
    public void UpdateBelief_ShouldUpdateConditionIsSatisfied_UpdatesObservation()
    {
        // Arrange
        List<int> list = [];
        Belief<List<int>, int> belief = new(list, reference => reference.Count, AlwaysUpdate);

        // Act
        list.Add(69);
        belief.UpdateBelief();

        // Assert
        Assert.Equal(list.Count, belief);
    }

    /// <summary>
    /// Given a Belief instance with an shouldUpdate condition that is not satisfied,
    /// When UpdateBelief is called,
    /// Then the observation is not updated.
    /// </summary>
    [Fact]
    public void UpdateBelief_ShouldUpdateConditionIsNotSatisfied_DoesNotUpdateObservation()
    {
        // Arrange
        List<int> list = [];
        Belief<List<int>, int> belief = new(list, reference => reference.Count, NeverUpdate);

        // Act
        list.Add(420);
        belief.UpdateBelief();

        // Assert
        Assert.NotEqual(list.Count, belief);
    }

    /// <summary>
    /// Given a Belief instance with a reference,
    /// When the reference is assigned to and UpdateBelief is called,
    /// Then the observation is not updated.
    /// </summary>
    [Fact]
    public void UpdateBelief_ReferenceIsAssignedTo_DoesNotUpdateObservation()
    {
        // Arrange
        string def = "def";
        Belief<string, string> belief = new(def, reference => reference, AlwaysUpdate);

        // Act
        def = "abc";
        belief.UpdateBelief();

        // Assert
        Assert.NotEqual(def, belief);
    }
}
