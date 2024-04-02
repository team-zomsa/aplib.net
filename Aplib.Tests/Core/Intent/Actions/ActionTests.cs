using Aplib.Core;
using Aplib.Core.Intent.Actions;
using FluentAssertions;
using Action = Aplib.Core.Intent.Actions.Action;

namespace Aplib.Core.Tests.Core.Intent.Actions;

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
        Action action = new(metadata);

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
        Action action = new(metadata);

        // Assert
        action.Should().NotBeNull();
        action.Metadata.Name.Should().Be(name);
        action.Metadata.Description.Should().BeNull();
    }
    
    /// <summary>
    /// Given a side effect action with a string guard,
    /// When the action is executed,
    /// Then the result should be null.
    /// </summary>
    [Fact]
    public void Execute_SideEffects_ReturnsCorrectEffect()
    {
        // Arrange
        string? result = "abc";
        Action action = new(() => result = "def");

        // Act
        action.Execute();

        // Assert
        Assert.Equal("def", result);
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
        GuardedAction<int> action = new(guard: () => 42, effect: guard => result = guard);

        // Act
        _ = action.IsActionable();
        action.Execute();

        // Assert
        Assert.Equal(42, result);
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
        Action action = new(() => { });

        // Act
        bool actionable = action.IsActionable();

        // Assert
        Assert.True(actionable);
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
        GuardedAction<bool> action = new(guard: () => false, effect: b => { });

        // Act
        bool result = action.IsActionable();

        // Assert
        Assert.True(result);
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
        GuardedAction<int> action = new(guard: () => 10, effect: b => { });

        // Act
        bool result = action.IsActionable();

        // Assert
        Assert.True(result);
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
        GuardedAction<object> action = new(guard: () => null!, effect: b => { });

        // Act
        bool result = action.IsActionable();

        // Assert
        Assert.False(result);
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
        Action action = new(() => { }, () => false);

        // Act
        bool actionable = action.IsActionable();

        // Assert
        Assert.False(actionable);
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
        Action action = new(() => { }, () => true);

        // Act
        bool actionable = action.IsActionable();

        // Assert
        Assert.True(actionable);
    }
}
