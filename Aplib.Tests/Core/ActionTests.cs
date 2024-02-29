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
        Aplib.Core.Action<string> action = new();
        string? result = "Abc";
        action.Effect = (query) => result = query;

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
        Aplib.Core.Action<int> action = new();
        int result = 0;
        action.Query = () => 42;
        action.Effect = (query) => result = query;

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
        Aplib.Core.Action<int> action = new()
        {
            Query = () => 10
        };

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
        Aplib.Core.Action<bool> action = new()
        {
            Query = () => false
        };

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
        Aplib.Core.Action<object> action = new()
        {
            Query = () => null!
        };

        // Act
        bool result = action.Actionable;

        // Assert
        Assert.False(result);
    }
}
