using Aplib.Core.Desire;
using Aplib.Tests.Stubs.Desire;
using FluentAssertions;

namespace Aplib.Tests.Desire;

public class GoalTests
{
    [Fact]
    public void Goal_WhenConstructed_ContainsCorrectMetaData()
    {
        // Arrange
        Tactic tactic = new TacticStub(() => { });
        IGoalPredicate predicate = new ConstantGoalPredicate(0f);
        var name = "Such a good goal name";
        var description = "\"A lie is just a good story that someone ruined with the truth.\" - Barney Stinson";

        // Act
        Goal g = new(tactic, predicate, name, description); // Does not use helper method DefaultGoal on purpose

        // Assert
        g.Should().NotBeNull();
        g.Name.Should().Be(name);
        g.Description.Should().Be(description);
    }

    [Fact]
    public void Goal_WhenConstructed_DidNotIterateYet()
    {
        // Arrange
        var iterations = 0;
        Tactic tactic = new TacticStub(() => iterations++);

        // Act
        Goal _ = DefaultGoal(tactic: tactic);

        // Assert
        iterations.Should().Be(0);
    }

    [Fact]
    public void Goal_WhenIterating_DoesIterate()
    {
        // Arrange
        var iterations = 0;
        Tactic tactic = new TacticStub(() => iterations++);

        // Act
        Goal g = DefaultGoal(tactic: tactic);
        g.Iterate();

        // Assert
        iterations.Should().BeGreaterThan(0);
    }

    [Theory]
    [InlineData(true)]
    [InlineData(false)]
    public void Goal_WhenReached_ReturnsAsCompleted(bool completed)
    {
        // Arrange
        float distance = completed ? 0 : 69420;
        IGoalPredicate predicate = new ConstantGoalPredicate(distance);

        // Act
        bool isCompleted = DefaultGoal(predicate: predicate).IsCompleted();

        // Assert
        isCompleted.Should().Be(completed);
    }


    #region HelperMethods

    private static Goal DefaultGoal(Tactic? tactic = null, IGoalPredicate? predicate = null, string? name = null,
        string? description = null) => new(
            tactic: tactic ?? new TacticStub(() => { }),
            goalPredicate: predicate ?? new ConstantGoalPredicate(0),
            name: name ?? "Such a good goal name",
            description: description ??
                         "\"A lie is just a good story that someone ruined with the truth.\" - Barney Stinson");

    #endregion
}