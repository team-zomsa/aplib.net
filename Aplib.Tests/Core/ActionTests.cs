namespace Aplib.Tests.Core;

/// <summary>
/// Describes a set of tests for the <see cref="Aplib.Core.Action{TQuery}"/> class.
/// </summary>
public class ActionTests
{
    /// <summary>
    /// Given a side effect action with a string query,
    /// When the action is executed,
    /// Then the result should be null.
    /// </summary>
    [Fact]
    public void Execute_SideEffects_ReturnsCorrectEffect()
    {
        // Arrange
        string? result = "Abc";
        Aplib.Core.Action<string> action = new(effect: (query) => { result = query; }, query: () => { return null!; });

        // Act
        action.Execute();

        // Assert
        Assert.Null(result);
    }

    /// <summary>
    /// Given a guarded action with an int query,
    /// When the action is guarded and executed,
    /// Then the result should be the value of the query.
    /// </summary>
    [Fact]
    public void Execute_WithGuard_ShouldInvokeQueryAndStoreResult()
    {
        // Arrange
        int result = 0;
        Aplib.Core.Action<int> action = new(query: () => 42, effect: (query) => { result = query; });

        // Act
        action.Guard();
        action.Execute();

        // Assert
        Assert.Equal(42, result);
    }

    /// <summary>
    /// Given an action with a non-null int query,
    /// When checking if the action is actionable,
    /// Then the result should be true.
    /// </summary>
    [Fact]
    public void Actionable_QueryIsNotNull_IsActionable()
    {
        // Arrange
        Aplib.Core.Action<int> action = new(query: () => 10, effect: b => { });

        // Act
        bool result = action.Actionable;

        // Assert
        Assert.True(result);
    }

    /// <summary>
    /// Given an action with a false bool query,
    /// When checking if the action is actionable,
    /// Then the result should be false.
    /// </summary>
    [Fact]
    public void Actionable_QueryIsFalse_IsNotActionable()
    {
        // Arrange
        Aplib.Core.Action<bool> action = new(query: () => false, effect: b => { });

        // Act
        bool result = action.Actionable;

        // Assert
        Assert.False(result);
    }

    /// <summary>
    /// Given an action with a null object query,
    /// When checking if the action is actionable,
    /// Then the result should be false.
    /// </summary>
    [Fact]
    public void Actionable_QueryIsNull_IsNotActionable()
    {
        // Arrange
        Aplib.Core.Action<object> action = new(query: () => null!, effect: b => { });

        // Act
        bool result = action.Actionable;

        // Assert
        Assert.False(result);
    }
}
