// This program has been developed by students from the bachelor Computer Science at Utrecht
// University within the Software Project course.
// Copyright Utrecht University (Department of Information and Computing Sciences)

using Aplib.Core.Belief.BeliefSets;
using Aplib.Core.Desire.Goals;
using Aplib.Core.Intent.Actions;
using Aplib.Core.Intent.Tactics;
using FluentAssertions;
using Moq;

namespace Aplib.Core.Tests.Desire;

public class GoalTests
{
    /// <summary>
    /// Given valid parameters and metadata,
    /// When the goal is constructed,
    /// Then the goal should correctly store the metadata.
    /// </summary>
    [Fact]
    public void Goal_WhenConstructed_ContainsCorrectMetaData()
    {
        // Arrange
        ITactic<IBeliefSet> tactic = Mock.Of<ITactic<IBeliefSet>>();
        const string name = "Such a good goal name";
        const string description =
            "\"A lie is just a good story that someone ruined with the truth.\" - Barney Stinson";
        Metadata metadata = new(name, description);

        // Act
        // Does not use helper methods on purpose
        Goal<IBeliefSet> goal = new(metadata, tactic, _ => false);

        // Assert
        goal.Should().NotBeNull();
        goal.Metadata.Should().Be(metadata);
    }

    /// <summary>
    /// Given the Goal is created properly using its constructor,
    /// When the goal has been constructed,
    /// Then the given tactic has not been applied yet
    /// </summary>
    [Fact]
    public void Goal_WhenConstructed_DidNotIterateYet()
    {
        // Arrange
        int iterations = 0;
        Mock<ITactic<IBeliefSet>> tactic = new();
        tactic.Setup(x => x.GetAction(It.IsAny<IBeliefSet>())).Returns(new Action<IBeliefSet>(_ => { iterations++; }));

        // Act
        Goal<IBeliefSet> goal = new(tactic.Object, _ => false);

        // Assert
        goal.Tactic.Should().Be(tactic.Object);
        iterations.Should().Be(0);
    }

    /// <summary>
    /// Given the Goal's predicate evaluates to false,
    /// when the UpdateStatus() method of a goal is used,
    /// then Status should return false.
    /// </summary>
    [Fact]
    public void Goal_WhenNotReached_DoesNotReturnAsCompleted()
    {
        // Arrange
        IBeliefSet beliefSet = Mock.Of<IBeliefSet>();
        ITactic<IBeliefSet> tactic = Mock.Of<ITactic<IBeliefSet>>();
        Goal<IBeliefSet> goal = new(tactic, _ => false);

        // Act
        goal.UpdateStatus(beliefSet);
        CompletionStatus isCompleted = goal.Status;

        // Assert
        isCompleted.Should().Be(CompletionStatus.Unfinished);
    }

    /// <summary>
    /// Given the Goal's predicate evaluates to true,
    /// when the UpdateStatus() method of a goal is used,
    /// then the method should return true.
    /// </summary>
    [Fact]
    public void Goal_WhenReached_ReturnsAsCompleted()
    {
        // Arrange
        IBeliefSet beliefSet = Mock.Of<IBeliefSet>();
        ITactic<IBeliefSet> tactic = Mock.Of<ITactic<IBeliefSet>>();
        Goal<IBeliefSet> goal = new(tactic, _ => true);

        // Act
        goal.UpdateStatus(beliefSet);
        CompletionStatus isCompleted = goal.Status;

        // Assert
        isCompleted.Should().Be(CompletionStatus.Success);
    }

    /// <summary>
    /// Given a valid goal and belief,
    /// when the goal's predicate is evaluated,
    /// the belief set is not altered
    /// </summary>
    [Fact]
    public void Goal_WhereEvaluationIsPerformed_DoesNotInfluenceBelieveSet()
    {
        // Arrange
        Mock<IBeliefSet> beliefSetMock = new();
        ITactic<IBeliefSet> tactic = Mock.Of<ITactic<IBeliefSet>>();
        Goal<IBeliefSet> goal = new(tactic, _ => false);

        // Act
        goal.UpdateStatus(beliefSetMock.Object);

        // Assert
        beliefSetMock.Verify(beliefSet => beliefSet.UpdateBeliefs(), Times.Never);
    }

    /// <summary>
    /// Given a valid goal
    /// when the goal's predicate result will be different in the next frame
    /// the most recent result is used
    /// </summary>
    [Fact]
    public void Goal_WhereHeuristicsChange_UsesUpdatedHeuristics()
    {
        // Arrange
        IBeliefSet beliefSet = Mock.Of<IBeliefSet>();
        ITactic<IBeliefSet> tactic = Mock.Of<ITactic<IBeliefSet>>();
        bool shouldSucceed = false;
        Goal<IBeliefSet> goal = new(tactic, _ => shouldSucceed);

        // Act
        goal.UpdateStatus(beliefSet);
        CompletionStatus stateBefore = goal.Status;
        shouldSucceed = true; // Make heuristic function return a different value on next invocation.
        goal.UpdateStatus(beliefSet);
        CompletionStatus stateAfter = goal.Status;

        // Assert
        stateBefore.Should().Be(CompletionStatus.Unfinished);
        stateAfter.Should().Be(CompletionStatus.Success);
    }

    [Theory]
    [InlineData(true)]
    [InlineData(false)]
    public void Goal_WithoutFailGuard_DoesNotFail(bool shouldSucceed)
    {
        // Arrange
        IBeliefSet beliefSet = Mock.Of<IBeliefSet>();
        ITactic<IBeliefSet> tactic = Mock.Of<ITactic<IBeliefSet>>();
        Goal<IBeliefSet> goal = new(tactic, predicate: _ => shouldSucceed);

        // Act
        goal.UpdateStatus(beliefSet);

        // Assert
        goal.Status.Should().NotBe(CompletionStatus.Failure);
    }

    [Fact]
    public void UpdateStatus_WhenBothFailGuardIsTrueAndPredicateIsTrue_CompletesTheGoal()
    {
        // Arrange
        IBeliefSet beliefSet = Mock.Of<IBeliefSet>();
        ITactic<IBeliefSet> tactic = Mock.Of<ITactic<IBeliefSet>>();
        Goal<IBeliefSet> goal = new(tactic, predicate: _ => true, failGuard: _ => true);

        // Act
        goal.UpdateStatus(beliefSet);

        // Assert
        goal.Status.Should().Be(CompletionStatus.Success);
    }

    [Fact]
    public void UpdateStatus_WhenFailGuardIsTrue_FailsTheGoal()
    {
        // Arrange
        IBeliefSet beliefSet = Mock.Of<IBeliefSet>();
        ITactic<IBeliefSet> tactic = Mock.Of<ITactic<IBeliefSet>>();
        Goal<IBeliefSet> goal = new(tactic, predicate: _ => false, failGuard: _ => true);

        // Act
        goal.UpdateStatus(beliefSet);

        // Assert
        goal.Status.Should().Be(CompletionStatus.Failure);
    }
}
