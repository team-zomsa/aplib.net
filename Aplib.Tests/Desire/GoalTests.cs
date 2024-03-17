using Aplib.Core.Desire;
using Aplib.Tests.Stubs.Desire;
using Aplib.Tests.Tools;
using FluentAssertions;

namespace Aplib.Tests.Desire;

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
        Tactic tactic = new TacticStub(() => { });
        Goal.HeuristicFunction heuristicFunction = CommonHeuristicFunctions.Constant(0f);
        const string name = "Such a good goal name";
        const string description = "\"A lie is just a good story that someone ruined with the truth.\" - Barney Stinson";

        // Act
        Goal goal = new(tactic, heuristicFunction, name, description); // Does not use helper methods on purpose

        // Assert
        goal.Should().NotBeNull();
        goal.Name.Should().Be(name);
        goal.Description.Should().Be(description);
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
        Tactic tactic = new TacticStub(() => iterations++);

        // Act
        Goal _ = new TestGoalBuilder().UseTactic(tactic).Build();

        // Assert
        iterations.Should().Be(0);
    }

    /// <summary>
    /// Given the Goal is created properly using its constructor,
    /// When the goal is being iterated over,
    /// Then the given tactic has has been applied at least once
    /// </summary>
    [Fact]
    public void Goal_WhenIterating_DoesIterate()
    {
        // Arrange
        int iterations = 0;
        Tactic tactic = new TacticStub(() => iterations++);

        // Act
        Goal goal = new TestGoalBuilder().UseTactic(tactic).Build();
        goal.Iterate();

        // Assert
        iterations.Should().BeGreaterThan(0);
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
        Goal.HeuristicFunction heuristicFunction = CommonHeuristicFunctions.Completed();

        // Act
        Goal goal = new TestGoalBuilder().WithHeuristicFunction(heuristicFunction).Build();
        bool isCompleted = goal.Evaluate();

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
        Goal.HeuristicFunction heuristicFunction = CommonHeuristicFunctions.Uncompleted();

        // Act
        Goal goal = new TestGoalBuilder().WithHeuristicFunction(heuristicFunction).Build();
        bool isCompleted = goal.Evaluate();

        // Assert
        isCompleted.Should().Be(false);
    }

    /// <summary>
    /// Given the Goal's different constructors have been called with semantically equal argumetns
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
        Tactic tactic = new TacticStub(() => { });
        const string name = "Such a good goal name";
        const string description = "\"A lie is just a good story that someone ruined with the truth.\" - Barney Stinson";

        Func<bool> heuristicFunctionBoolean = () => goalCompleted;
        Goal.HeuristicFunction heuristicFunctionNonBoolean = CommonHeuristicFunctions.Boolean(() => goalCompleted);

        Goal goalBoolean = new(tactic, heuristicFunctionBoolean, name, description);
        Goal goalNonBoolean = new(tactic, heuristicFunctionNonBoolean, name, description);

        // Act
        bool goalBooleanEvaluation = goalBoolean.Evaluate();
        bool goalNonBooleanEvaluation = goalNonBoolean.Evaluate();

        // Assert
        goalBooleanEvaluation.Should().Be(goalNonBooleanEvaluation);
    }
}
