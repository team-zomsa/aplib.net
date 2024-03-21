using Aplib.Core.Belief;

namespace Aplib.Tests.Core.Belief;
public class BeliefSetTests
{
    /// <summary>
    /// Given a BeliefSet instance with multiple beliefs,
    /// When UpdateBeliefs is called,
    /// Then all beliefs are updated.
    /// </summary>
    [Fact]
    public void UpdateBeliefs_Called_UpdatesAllBeliefs()
    {
        // Arrange
        TestBeliefSet beliefSet = new();

        // Act
        beliefSet.UpdateBeliefs();

        // Assert
        Assert.True(beliefSet.Belief1.Updated);
        Assert.True(beliefSet.Belief2.Updated);
    }

    private class TestBeliefSet : BeliefSet
    {
        public SimpleBelief Belief1 = new();
        public SimpleBelief Belief2 = new();
    }

    /// <summary>
    /// A simple belief that can be used to test whether <see cref="UpdateBelief"/> has been called.
    /// </summary>
    private class SimpleBelief : IBelief
    {
        public bool Updated { get; private set; } = false;

        public void UpdateBelief()
        {
            Updated = true;
        }
    }
}
