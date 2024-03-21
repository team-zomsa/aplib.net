using Aplib.Core.Belief;

namespace Aplib.Tests.Core.Belief;
public class BeliefTests
{
    private static TReference ID<TReference>(TReference reference) => reference;

    private static int GetCount<T>(IEnumerable<T> reference) => reference.Count();

    private static bool TrueUpdateIf() => true;

    private static bool FalseUpdateIf() => false;


    /// <summary>
    /// Given a Belief instance,
    /// When it is assigned to a variable of its observation type,
    /// Then it is implicitly converted to its observation type.
    /// </summary>
    [Fact]
    public void Belief_AssignedToObservationType_IsImplicitlyConvertedToObservationType()
    {
        // Arrange
        string reference = "def";
        Belief<string, string> belief = new(reference, ID);

        // Act
        string observation = belief;

        // Assert
        Assert.Equal("def", observation);
    }

    /// <summary>
    /// Given a Belief instance with an updateIf condition that is satisfied,
    /// When UpdateBelief is called,
    /// Then the observation is updated.
    /// </summary>
    [Fact]
    public void UpdateBelief_UpdateIfConditionIsSatisfied_UpdatesObservation()
    {
        // Arrange
        List<int> list = [];
        Belief<List<int>, int> belief = new(list, GetCount, TrueUpdateIf);

        // Act
        list.Add(69);
        belief.UpdateBelief();

        // Assert
        Assert.Equal(1, belief);
    }

    /// <summary>
    /// Given a Belief instance with an updateIf condition that is not satisfied,
    /// When UpdateBelief is called,
    /// Then the observation is not updated.
    /// </summary>
    [Fact]
    public void UpdateBelief_UpdateIfConditionIsNotSatisfied_DoesNotUpdateObservation()
    {
        // Arrange
        List<int> list = [];
        Belief<List<int>, int> belief = new(list, GetCount, FalseUpdateIf);

        // Act
        list.Add(69);
        belief.UpdateBelief();

        // Assert
        Assert.Equal(0, belief);
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
        Belief<string, string> belief = new(def, ID, TrueUpdateIf);

        // Act
        def = "abc";
        belief.UpdateBelief();

        // Assert
        Assert.NotEqual(def, belief);
    }
}
