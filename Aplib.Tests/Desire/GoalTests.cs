using Aplib.Core;
using Aplib.Core.Believe;
using Aplib.Core.Desire.Goals;
using Aplib.Core.Intent.Tactics;
using Aplib.Tests.Stubs.Desire;
using Aplib.Tests.Tools;
using FluentAssertions;
using System.Diagnostics.CodeAnalysis;
using Action = Aplib.Core.Intent.Actions.Action;

namespace Aplib.Tests.Desire;

[SuppressMessage("ReSharper", "StringLiteralTypo")]
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
        Tactic tactic = new TacticStub(new Action(() => { }, new Metadata("a1")), "tictac");
        Goal.HeuristicFunction heuristicFunction = CommonHeuristicFunctions.Constant(0f);
        const string name = "Such a good goal name";
        const string description = "\"A lie is just a good story that someone ruined with the truth.\" - Barney Stinson";
        Metadata metadata = new(name, description);
        
        // Act
        Goal goal = new(tactic, heuristicFunction, metadata);

        // Assert
        goal.Should().NotBeNull();
        goal.Metadata.Name.Should().Be(name);
        goal.Metadata.Description.Should().Be(description);
    }

    [Fact]
    public void Goal_WithoutDescription_ContainsCorrectMetaData()
    {
        // Arrange
        Tactic tactic = new TacticStub(new Action(() => { }, new Metadata("a1")), "tictac");
        Goal.HeuristicFunction heuristicFunction = CommonHeuristicFunctions.Constant(0f);
        const string name = "Such a good goal name";
        Metadata metadata = new(name);

        // Act
        Goal goal = new(tactic, heuristicFunction, metadata);

        // Assert
        goal.Should().NotBeNull();
        goal.Metadata.Name.Should().Be(name);
        goal.Metadata.Description.Should().BeNull();
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
        Tactic tactic = new TacticStub(new Action(() => iterations++, new Metadata("a1")), "tictac");

        // Act
        Goal _ = new TestGoalBuilder().UseTactic(tactic).Build();

        // Assert
        iterations.Should().Be(0);
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
        BeliefSet beliefSet = new();
        Goal.HeuristicFunction heuristicFunction = CommonHeuristicFunctions.Completed();

        // Act
        Goal goal = new TestGoalBuilder().WithHeuristicFunction(heuristicFunction).Build();
        bool isCompleted = goal.IsCompleted(beliefSet);

        // Assert
        isCompleted.Should().Be(true);
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
        BeliefSet beliefSet = new();
        Goal.HeuristicFunction heuristicFunction = CommonHeuristicFunctions.Uncompleted();

        // Act
        Goal goal = new TestGoalBuilder().WithHeuristicFunction(heuristicFunction).Build();
        bool isCompleted = goal.IsCompleted(beliefSet);

        // Assert
        isCompleted.Should().Be(false);
    }

    /// <summary>
    /// Given a valid goal and believe,
    /// when the goal's heuristic function is evaluated,
    /// the believeset is not altered
    /// </summary>
    [Fact]
    public void Goal_WhereEvaluationIsPerformed_DoesNotInfluenceBelieveSet()
    {
        // Arrange
        BeliefSet beliefSet = new();

        // Act
        string currentBelieveSetState = beliefSet.State;
        Goal goal = new TestGoalBuilder().Build();
        _ = goal.IsCompleted(beliefSet);

        // Assert
        beliefSet.State.Should().Be(currentBelieveSetState);
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
        Tactic tactic = new TacticStub(new Action(() => { }, new Metadata("a1")), "tictac");
        const string name = "Such a good goal name";
        const string description = "\"A lie is just a good story that someone ruined with the truth.\" - Barney Stinson";
        Metadata goalMetadata = new(name, description);
        
        Func<bool> heuristicFunctionBoolean = () => goalCompleted;
        Goal.HeuristicFunction heuristicFunctionNonBoolean = CommonHeuristicFunctions.Boolean(() => goalCompleted);

        Goal goalBoolean = new(tactic, heuristicFunctionBoolean, goalMetadata);
        Goal goalNonBoolean = new(tactic, heuristicFunctionNonBoolean, goalMetadata);

        // Act
        BeliefSet beliefSet = new();
        bool goalBooleanEvaluation = goalBoolean.IsCompleted(beliefSet);
        bool goalNonBooleanEvaluation = goalNonBoolean.IsCompleted(beliefSet);

        // Assert
        goalBooleanEvaluation.Should().Be(goalNonBooleanEvaluation);
    }
}
