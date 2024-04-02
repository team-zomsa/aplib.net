using Aplib.Core.Belief;
using Aplib.Core.Desire.Goals;
using Aplib.Core.Intent.Actions;
using Aplib.Core.Intent.Tactics;
using Aplib.Tests.Tools;
using FluentAssertions;
using Moq;

namespace Aplib.Core.Tests.Desire;

public class GoalTests
{
    /// <summary>
    /// A test belief set that contains two public simple beliefs.
    /// </summary>
    private class MyBeliefSet : BeliefSet
    {
        /// <summary>
        /// Belief that sets Updated to true when UpdateBelief is called.
        /// </summary>
        public readonly SimpleBelief MyBelief = new();
    }

    /// <summary>
    /// A simple belief that can be used to test whether <see cref="UpdateBelief" /> has been called.
    /// </summary>
    private class SimpleBelief : IBelief
    {
        /// <summary>
        /// Stores whether <see cref="UpdateBelief" /> has been called.
        /// </summary>
        public bool Updated { get; private set; }

        /// <summary>
        /// Sets <see cref="Updated" /> to true.
        /// </summary>
        public void UpdateBelief() => Updated = true;
    }

    /// <summary>
    /// Given valid parameters and metadata,
    /// When the goal is constructed,
    /// Then the goal should correctly store the metadata.
    /// </summary>
    [Fact]
    public void Goal_WhenConstructed_ContainsCorrectMetaData()
    {
        // Arrange
        Tactic<IBeliefSet> tactic = Mock.Of<Tactic<IBeliefSet>>();
        Goal<IBeliefSet>.HeuristicFunction heuristicFunction = CommonHeuristicFunctions<IBeliefSet>.Constant(0f);
        const string name = "Such a good goal name";
        const string description =
            "\"A lie is just a good story that someone ruined with the truth.\" - Barney Stinson";
        Metadata metadata = new(name, description);

        // Act
        // Does not use helper methods on purpose
        Goal<IBeliefSet> goal = new(tactic, heuristicFunction: heuristicFunction, metadata: metadata);

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
        Mock<Tactic<IBeliefSet>> tactic = new();
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
        MyBeliefSet beliefSet = new();
        Goal<IBeliefSet>.HeuristicFunction heuristicFunction = CommonHeuristicFunctions<IBeliefSet>.Uncompleted();

        // Act
        Goal<IBeliefSet> goal = new TestGoalBuilder().WithHeuristicFunction(heuristicFunction).Build();
        CompletionStatus isCompleted = goal.GetStatus(beliefSet);

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
        MyBeliefSet beliefSet = new();
        Goal<IBeliefSet>.HeuristicFunction heuristicFunction = CommonHeuristicFunctions<IBeliefSet>.Completed();

        // Act
        Goal<IBeliefSet> goal = new TestGoalBuilder().WithHeuristicFunction(heuristicFunction).Build();
        CompletionStatus isCompleted = goal.GetStatus(beliefSet);

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
        MyBeliefSet beliefSet = new();

        // Act
        Goal<IBeliefSet> goal = new TestGoalBuilder().Build();
        _ = goal.GetStatus(beliefSet);

        // Assert
        beliefSet.MyBelief.Updated.Should().Be(false);
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

        bool heuristicFunctionBoolean(IBeliefSet _) => goalCompleted;
        Goal<IBeliefSet>.HeuristicFunction heuristicFunctionNonBoolean = CommonHeuristicFunctions<IBeliefSet>.Boolean(_ => goalCompleted);

        Goal<IBeliefSet> goalBoolean = new(tactic, heuristicFunctionBoolean);
        Goal<IBeliefSet> goalNonBoolean = new(tactic, heuristicFunctionNonBoolean);

        // Act
        MyBeliefSet beliefSet = new();
        CompletionStatus goalBooleanEvaluation = goalBoolean.GetStatus(beliefSet);
        CompletionStatus goalNonBooleanEvaluation = goalNonBoolean.GetStatus(beliefSet);

        // Assert
        goalBooleanEvaluation.Should().Be(goalNonBooleanEvaluation);
    }
}
