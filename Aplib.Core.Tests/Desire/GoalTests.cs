using Aplib.Core.Belief.BeliefSets;
using Aplib.Core.Desire.Goals;
using Aplib.Core.Intent.Actions;
using Aplib.Core.Intent.Tactics;
using Aplib.Core.Tests.Tools;
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
        Goal<IBeliefSet>.HeuristicFunction heuristicFunction = CommonHeuristicFunctions<IBeliefSet>.Constant(0f);
        const string name = "Such a good goal name";
        const string description =
            "\"A lie is just a good story that someone ruined with the truth.\" - Barney Stinson";
        Metadata metadata = new(name, description);

        // Act
        // Does not use helper methods on purpose
        Goal<IBeliefSet> goal = new(metadata, tactic, heuristicFunction: heuristicFunction);

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
        Goal<IBeliefSet> goal = new TestGoalBuilder().UseTactic(tactic.Object).Build();

        // Assert
        goal.Tactic.Should().Be(tactic.Object);
        iterations.Should().Be(0);
    }

    /// <summary>
    /// Given the Goal's heuristic function is configured to *not* have reached its goal,
    /// when the Evaluate() method of a goal is used,
    /// then the method should return false.
    /// </summary>
    [Fact]
    public void Goal_WhenNotReached_DoesNotReturnAsCompleted()
    {
        // Arrange
        Goal<IBeliefSet>.HeuristicFunction heuristicFunction = CommonHeuristicFunctions<IBeliefSet>.Uncompleted();

        // Act
        Goal<IBeliefSet> goal = new TestGoalBuilder().WithHeuristicFunction(heuristicFunction).Build();
        goal.UpdateStatus(It.IsAny<IBeliefSet>());
        CompletionStatus isCompleted = goal.Status;

        // Assert
        isCompleted.Should().Be(CompletionStatus.Unfinished);
    }

    /// <summary>
    /// Given the Goal's heuristic function is configured to have reached its goal
    /// when the Evaluate() method of a goal is used,
    /// then the method should return true.
    /// </summary>
    [Fact]
    public void Goal_WhenReached_ReturnsAsCompleted()
    {
        // Arrange
        Goal<IBeliefSet>.HeuristicFunction heuristicFunction = CommonHeuristicFunctions<IBeliefSet>.Completed();

        // Act
        Goal<IBeliefSet> goal = new TestGoalBuilder().WithHeuristicFunction(heuristicFunction).Build();
        goal.UpdateStatus(It.IsAny<IBeliefSet>());
        CompletionStatus isCompleted = goal.Status;

        // Assert
        isCompleted.Should().Be(CompletionStatus.Success);
    }

    /// <summary>
    /// Given a valid goal and belief,
    /// when the goal's heuristic function is evaluated,
    /// the belief set is not altered
    /// </summary>
    [Fact]
    public void Goal_WhereEvaluationIsPerformed_DoesNotInfluenceBelieveSet()
    {
        // Arrange
        Mock<IBeliefSet> beliefSetMock = new();
        Goal<IBeliefSet> goal = new TestGoalBuilder().Build();

        // Act
        goal.UpdateStatus(It.IsAny<IBeliefSet>());

        // Assert
        beliefSetMock.Verify(beliefSet => beliefSet.UpdateBeliefs(), Times.Never);
    }

    /// <summary>
    /// Given a valid goal with heuristics
    /// when the goal's heuristic function's result will be different in the next frame
    /// the most recent heuristics are used
    /// </summary>
    [Fact]
    public void Goal_WhereHeuristicsChange_UsesUpdatedHeuristics()
    {
        // Arrange
        IBeliefSet beliefSetMock = Mock.Of<IBeliefSet>();
        bool shouldSucceed = false;
        // ReSharper disable once AccessToModifiedClosure
        Goal<IBeliefSet> goal = new TestGoalBuilder().WithHeuristicFunction(_ => shouldSucceed).Build();

        // Act
        goal.UpdateStatus(beliefSetMock);
        CompletionStatus stateBefore = goal.Status;
        shouldSucceed = true; // Make heuristic function return a different value on next invocation.
        goal.UpdateStatus(beliefSetMock);
        CompletionStatus stateAfter = goal.Status;

        // Assert
        stateBefore.Should().Be(CompletionStatus.Unfinished);
        stateAfter.Should().Be(CompletionStatus.Success);
    }

    /// <summary>
    /// Given the Goal's different constructors have been called with semantically equal arguments
    /// when the Evaluate() method of all goals are used,
    /// then all returned values should equal.
    /// </summary>
    /// <param name="goalCompleted"></param>
    [Theory]
    [InlineData(true)]
    [InlineData(false)]
    public void GoalConstructor_WhereHeuristicFunctionTypeDiffers_HasEqualBehaviour(bool goalCompleted)
    {
        // Arrange
        ITactic<IBeliefSet> tactic = Mock.Of<ITactic<IBeliefSet>>();

        // ReSharper disable once MoveLocalFunctionAfterJumpStatement
        bool HeuristicFunctionBoolean(IBeliefSet _) => goalCompleted;
        Goal<IBeliefSet>.HeuristicFunction heuristicFunctionNonBoolean =
            CommonHeuristicFunctions<IBeliefSet>.Boolean(_ => goalCompleted);

        Goal<IBeliefSet> goalBoolean = new(tactic, HeuristicFunctionBoolean);
        Goal<IBeliefSet> goalNonBoolean = new(tactic, heuristicFunctionNonBoolean);

        // Act
        goalBoolean.UpdateStatus(It.IsAny<IBeliefSet>());
        CompletionStatus goalBooleanEvaluation = goalBoolean.Status;
        goalNonBoolean.UpdateStatus(It.IsAny<IBeliefSet>());
        CompletionStatus goalNonBooleanEvaluation = goalNonBoolean.Status;

        // Assert
        goalBooleanEvaluation.Should().Be(goalNonBooleanEvaluation);
    }

    [Theory]
    [InlineData(true)]
    [InlineData(false)]
    public void UpdateStatus_WhenFailGuardIsTrue_FailsTheGoal(bool goalCompleted)
    {
        // Arrange
        ITactic<IBeliefSet> tactic = Mock.Of<ITactic<IBeliefSet>>();
        Goal<IBeliefSet>.HeuristicFunction heuristic = CommonHeuristicFunctions<IBeliefSet>.Boolean(_ => goalCompleted);
        Goal<IBeliefSet> goal = new(tactic, heuristic, _ => true);

        // Act
        goal.UpdateStatus(It.IsAny<IBeliefSet>());

        // Assert
        goal.Status.Should().Be(CompletionStatus.Failure);
    }

    [Theory]
    [InlineData(true)]
    [InlineData(false)]
    public void Goal_WithoutFailGuard_DoesNotFail(bool goalCompleted)
    {
        // Arrange
        ITactic<IBeliefSet> tactic = Mock.Of<ITactic<IBeliefSet>>();
        Goal<IBeliefSet>.HeuristicFunction heuristic = CommonHeuristicFunctions<IBeliefSet>.Boolean(_ => goalCompleted);

        // Act
        Goal<IBeliefSet> goal = new(tactic, heuristic);
        goal.UpdateStatus(It.IsAny<IBeliefSet>());

        // Assert
        goal.Status.Should().NotBe(CompletionStatus.Failure);
    }
}
