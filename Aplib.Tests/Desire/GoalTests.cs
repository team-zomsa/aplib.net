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
        Goal.HeuristicFunction heuristicFunction = CommonGoalHeuristicFunctions.Constant(0f);
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
    public void Goal_WhenNotReached_DoesNotReturnAsCompleted()
    {
        // Arrange
        const float distance = 0;
        Goal.HeuristicFunction heuristicFunction = CommonGoalHeuristicFunctions.Constant(distance);

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
    public void Goal_WhenReached_ReturnsAsCompleted()
    {
        // Arrange
        const float distance = 69_420;
        Goal.HeuristicFunction heuristicFunction = CommonGoalHeuristicFunctions.Constant(distance);

        // Act
        Goal goal = new TestGoalBuilder().WithHeuristicFunction(heuristicFunction).Build();
        bool isCompleted = goal.Evaluate();

        // Assert
        isCompleted.Should().Be(false);
    }
}
