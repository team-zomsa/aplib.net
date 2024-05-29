using Aplib.Core.Belief.BeliefSets;
using Aplib.Core.Intent.Actions;
using FluentAssertions;
using Moq;

namespace Aplib.Core.Tests.Intent.Actions;

/// <summary>
/// Describes a set of tests for the <see cref="QueryAction{TBeliefSet,TQuery}" /> class.
/// </summary>
public class ActionTests
{
    [Fact]
    public void Action_WhenConstructed_ContainsCorrectMetaData()
    {
        // Arrange
        const string name = "Action";
        const string description = "A cheap store where I get all my stuff";
        Metadata metadata = new(name, description);

        // Act
        Action<IBeliefSet> action = new(metadata, _ => { });

        // Assert
        action.Should().NotBeNull();
        action.Metadata.Name.Should().Be(name);
        action.Metadata.Description.Should().Be(description);
    }

    [Fact]
    public void Action_WithoutDescription_ContainsCorrectMetaData()
    {
        // Arrange
        const string name = "my action";
        Metadata metadata = new(name);

        // Act
        Action<IBeliefSet> action = new(metadata, _ => { });

        // Assert
        action.Should().NotBeNull();
        action.Metadata.Name.Should().Be(name);
        action.Metadata.Description.Should().BeNull();
    }

    /// <summary>
    /// Given a side effect action,
    /// When the action is executed,
    /// Then the result should not be null.
    /// </summary>
    [Fact]
    public void Execute_SideEffects_ReturnsCorrectEffect()
    {
        // Arrange
        string? result = null;
        IBeliefSet beliefSet = Mock.Of<IBeliefSet>();
        Action<IBeliefSet> action = new(_ => result = "abc");

        // Act
        action.Execute(beliefSet);

        // Assert
        result.Should().Be("abc");
    }

    /// <summary>
    /// Given a guarded action with an int guard,
    /// When the action is guarded and executed,
    /// Then the result should be the value of the guard.
    /// </summary>
    [Fact]
    public void Execute_WithGuard_ShouldInvokeQueryAndStoreResult()
    {
        // Arrange
        int result = 0;
        IBeliefSet beliefSet = Mock.Of<IBeliefSet>();
        QueryAction<IBeliefSet, int> action = new(query: _ => 42, effect: (_, query) => result = query);

        // Act
        _ = action.Query(beliefSet);
        action.Execute(beliefSet);

        // Assert
        result.Should().Be(42);
    }
}
