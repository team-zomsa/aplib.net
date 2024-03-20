using Aplib.Core.Belief;

namespace Aplib.Tests.Core.Belief;
public class BeliefsetTests
{
    /// <summary>
    /// Given a Beliefset instance with multiple beliefs,
    /// When UpdateBeliefs is called,
    /// Then all beliefs are updated.
    /// </summary>
    [Fact]
    public void UpdateBeliefs_Called_UpdatesAllBeliefs()
    {
        // Arrange
        TestBeliefSet beliefset = new();

        // Act
        beliefset.UpdateBeliefs();

        // Assert
        Assert.True(beliefset.Belief1.Updated);
        Assert.True(beliefset.Belief2.Updated);
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
