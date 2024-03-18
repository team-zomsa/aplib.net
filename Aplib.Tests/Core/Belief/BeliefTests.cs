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
    /// When it is assigned to a variable of its resource type,
    /// Then it is implicitly converted to its resource type.
    /// </summary>
    [Fact]
    public void Belief_IsImplicitlyConvertedToResourceType_WhenAssignedToResourceType()
    {
        // Arrange
        string reference = "def";
        Belief<string, string> belief = new(reference, ID);

        // Act
        string resource = belief;

        // Assert
        Assert.Equal("def", resource);
    }

    /// <summary>
    /// Given a Belief instance with an updateIf condition that is satisfied,
    /// When UpdateBelief is called,
    /// Then the resource is updated.
    /// </summary>
    [Fact]
    public void UpdateBelief_UpdatesResource_WhenUpdateIfConditionIsSatisfied()
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
    /// Then the resource is not updated.
    /// </summary>
    [Fact]
    public void UpdateBelief_DoesNotUpdateResource_WhenUpdateIfConditionIsNotSatisfied()
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
    /// Then the resource is not updated.
    /// </summary>
    [Fact]
    public void UpdateBelief_DoesNotUpdateResource_WhenReferenceIsAssignedTo()
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
