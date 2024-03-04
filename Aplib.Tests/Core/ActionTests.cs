using Aplib.Core;
using Action = Aplib.Core.Action;

namespace Aplib.Tests.Core;

/// <summary>
/// Describes a set of tests for the <see cref="GuardedAction{TQuery}"/> class.
/// </summary>
public class ActionTests
{
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
        Action action = new(effect: () => { result = "def"; });

        // Act
        action.Execute();

        // Assert
        Assert.Equal("def", result);
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
        Action action = new(effect: () => { });

        // Act
        bool actionable = action.IsActionable();

        // Assert
        Assert.True(actionable);
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
        Action action = new(effect: () => { }, guard: () => true);

        // Act
        bool actionable = action.IsActionable();

        // Assert
        Assert.True(actionable);
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
        Action action = new(effect: () => { }, guard: () => false);

        // Act
        bool actionable = action.IsActionable();

        // Assert
        Assert.False(actionable);
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
        GuardedAction<int> action = new(guard: () => 42, effect: (guard) => { result = guard; });

        // Act
        _ = action.IsActionable();
        action.Execute();

        // Assert
        Assert.Equal(42, result);
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
}
