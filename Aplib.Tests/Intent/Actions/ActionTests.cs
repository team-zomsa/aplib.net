using Aplib.Core;
using Aplib.Core.Belief;
using Aplib.Core.Intent.Actions;
using FluentAssertions;
using Moq;

namespace Aplib.Core.Tests.Intent.Actions;

/// <summary>
/// Describes a set of tests for the <see cref="GuardedAction{TQuery}" /> class.
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
        Action<IBeliefSet> action = new(_ => { }, metadata);

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
        Action<IBeliefSet> action = new(_ => { }, metadata);

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
        Action<IBeliefSet> action = new(_ => result = "abc");

        // Act
        action.Execute(It.IsAny<IBeliefSet>());

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
        GuardedAction<IBeliefSet, int> action = new(guard: _ => 42, effect: (_, query) => result = query);

        // Act
        _ = action.IsActionable(It.IsAny<IBeliefSet>());
        action.Execute(It.IsAny<IBeliefSet>());

        // Assert
        result.Should().Be(42);
    }

    /// <summary>
    /// Given an action with no query,
    /// When checking if the action is actionable,
    /// Then the result should always be true.
    /// </summary>
    [Fact]
    public void IsActionable_NoQuery_AlwaysTrue()
    {
        // Arrange
        Action<IBeliefSet> action = new(_ => { });

        // Act
        bool actionable = action.IsActionable(It.IsAny<IBeliefSet>());

        // Assert
        actionable.Should().BeTrue();
    }

    /// <summary>
    /// Given an action with a false bool guard,
    /// When checking if the action is actionable,
    /// Then the result should be true.
    /// </summary>
    [Fact]
    public void IsActionable_QueryIsFalse_IsActionable()
    {
        // Arrange
        GuardedAction<IBeliefSet, bool> action = new(guard: _ => false, effect: (_, _) => { });

        // Act
        bool result = action.IsActionable(It.IsAny<IBeliefSet>());

        // Assert
        result.Should().BeTrue();
    }

    /// <summary>
    /// Given an action with a non-null int guard,
    /// When checking if the action is actionable,
    /// Then the result should be true.
    /// </summary>
    [Fact]
    public void IsActionable_QueryIsNotNull_IsActionable()
    {
        // Arrange
        GuardedAction<IBeliefSet, int> action = new(guard: _ => 10, effect: (_, _) => { });

        // Act
        bool result = action.IsActionable(It.IsAny<IBeliefSet>());

        // Assert
        result.Should().BeTrue();
    }

    /// <summary>
    /// Given an action with a null object guard,
    /// When checking if the action is actionable,
    /// Then the result should be false.
    /// </summary>
    [Fact]
    public void IsActionable_QueryIsNull_IsNotActionable()
    {
        // Arrange
        GuardedAction<IBeliefSet, object> action = new(guard: _ => null!, effect: (_, _) => { });

        // Act
        bool actionable = action.IsActionable(It.IsAny<IBeliefSet>());

        // Assert
        actionable.Should().BeFalse();
    }

    /// <summary>
    /// Given an action with a false query,
    /// When checking if the action is actionable,
    /// Then the result should be false.
    /// </summary>
    [Fact]
    public void IsActionable_QueryWithFalse_ReturnsFalse()
    {
        // Arrange
        Action<IBeliefSet> action = new(_ => { }, _ => false);

        // Act
        bool actionable = action.IsActionable(It.IsAny<IBeliefSet>());

        // Assert
        actionable.Should().BeFalse();
    }

    /// <summary>
    /// Given an action with a true query,
    /// When checking if the action is actionable,
    /// Then the result should be true.
    /// </summary>
    [Fact]
    public void IsActionable_QueryWithTrue_ReturnsTrue()
    {
        // Arrange
        Action<IBeliefSet> action = new(_ => { }, _ => true);

        // Act
        bool actionable = action.IsActionable(It.IsAny<IBeliefSet>());

        // Assert
        actionable.Should().BeTrue();
    }
}
