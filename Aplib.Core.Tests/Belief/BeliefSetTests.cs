using Aplib.Core.Belief.Beliefs;
using Aplib.Core.Belief.BeliefSets;

namespace Aplib.Core.Tests.Belief;

/// <summary>
/// Describes a set of tests for the <see cref="BeliefSet"/> class.
/// </summary>
public class BeliefSetTests
{
    /// <summary>
    /// Given a BeliefSet instance with multiple <i>public field</i> beliefs,
    /// When UpdateBeliefs is called,
    /// Then all beliefs are updated.
    /// </summary>
    [Fact]
    public void UpdateBeliefs_PublicBeliefFields_UpdatesAllBeliefs()
    {
        // Arrange
        TestBeliefSetPublic beliefSet = new();

        // Act
        // UpdateBeliefs should set Updated to true for all beliefs.
        beliefSet.UpdateBeliefs();

        // Assert
        Assert.True(beliefSet.Belief1.Updated);
        Assert.True(beliefSet.Belief2.Updated);
    }

    /// <summary>
    /// Given a BeliefSet instance with multiple <i>public property</i> beliefs,
    /// When UpdateBeliefs is called,
    /// Then no beliefs are updated.
    /// </summary>
    [Fact]
    public void UpdateBeliefs_PublicBeliefProperties_DoesNotUpdateAnyBeliefs()
    {
        // Arrange
        TestBeliefSetProperties beliefSet = new();

        // Act
        // UpdateBeliefs should *not* set Updated to true for any belief.
        beliefSet.UpdateBeliefs();

        // Assert
        Assert.False(beliefSet.Belief1.Updated);
        Assert.False(beliefSet.Belief2.Updated);
    }

    /// <summary>
    /// Given a BeliefSet instance with multiple <i>private field</i> beliefs,
    /// When UpdateBeliefs is called,
    /// Then no beliefs are updated.
    /// </summary>
    [Fact]
    public void UpdateBeliefs_PrivateBeliefFields_DoesNotUpdateAnyBeliefs()
    {
        // Arrange
        TestBeliefSetPrivate beliefSet = new();

        // Act
        // UpdateBeliefs should *not* set Updated to true for any belief.
        beliefSet.UpdateBeliefs();

        // Assert
        Assert.False(beliefSet.Belief1.Updated);
        Assert.False(beliefSet.Belief2.Updated);
    }

    /// <summary>
    /// A test belief set that contains two public simple beliefs.
    /// </summary>
    private class TestBeliefSetPublic : BeliefSet
    {
        /// <summary>
        /// Belief that sets Updated to true when UpdateBelief is called.
        /// </summary>
        public SimpleBelief Belief1 = new();

        /// <summary>
        /// Belief that sets Updated to true when UpdateBelief is called.
        /// </summary>
        public SimpleBelief Belief2 = new();
    }


    /// <summary>
    /// A test belief set that contains two simple public property beliefs.
    /// </summary>
    private class TestBeliefSetProperties : BeliefSet
    {
        /// <summary>
        /// Belief that sets Updated to true when UpdateBelief is called.
        /// </summary>
        public SimpleBelief Belief1 { get; } = new();

        /// <summary>
        /// Belief that sets Updated to true when UpdateBelief is called.
        /// </summary>
        public SimpleBelief Belief2 { get; } = new();
    }


    /// <summary>
    /// A test belief set that contains two private simple beliefs.
    /// </summary>
    private class TestBeliefSetPrivate : BeliefSet
    {
        /// <summary>
        /// Belief that sets Updated to true when UpdateBelief is called.
        /// </summary>
        private SimpleBelief _belief1 = new();

        /// <summary>
        /// Belief that sets Updated to true when UpdateBelief is called.
        /// </summary>
        private SimpleBelief _belief2 = new();

        /// <inheritdoc cref="_belief1"/>
        public SimpleBelief Belief1 => _belief1;

        /// <inheritdoc cref="_belief2"/>
        public SimpleBelief Belief2 => _belief2;
    }

    /// <summary>
    /// A simple belief that can be used to test whether <see cref="UpdateBelief"/> has been called.
    /// </summary>
    private class SimpleBelief : IBelief
    {
        /// <summary>
        /// Stores whether <see cref="UpdateBelief"/> has been called.
        /// </summary>
        public bool Updated { get; private set; } = false;

        /// <summary>
        /// Sets <see cref="Updated"/> to true.
        /// </summary>
        public void UpdateBelief()
        {
            Updated = true;
        }
    }
}
