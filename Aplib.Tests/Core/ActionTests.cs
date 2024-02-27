namespace Aplib.Tests.Core;

public class ActionTests
{
    /// <summary>
    /// Given an Action instance with an Effect delegate,
    /// When Execute is called,
    /// Then the Effect delegate should be invoked.
    /// </summary>
    [Fact]
    public void Execute_ShouldInvokeEffect()
    {
        // Arrange
        Aplib.Core.Action<string> action = new();
        string? result = null;
        action.Effect = (query) => result = query;

        // Act
        action.Execute();

        // Assert
        Assert.Null(result);
    }

    /// <summary>
    /// Given an Action instance with a Query delegate and an Effect delegate,
    /// When Guard and Execute are called,
    /// Then the Query delegate should be invoked and the result should be stored,
    /// And the Effect delegate should be invoked.
    /// </summary>
    [Fact]
    public void Guard_ShouldInvokeQueryAndStoreResult()
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

    [Fact]
    public void Actionable_ReturnsTrue_WhenQueryReturnsNonNullValue()
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

    [Fact]
    public void Actionable_ReturnsFalse_WhenQueryReturnsFalse()
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

    [Fact]
    public void Actionable_ReturnsFalse_WhenQueryReturnsNull()
    {
        // Arrange
        Aplib.Core.Action<object> action = new()
        {
            Query = () => null
        };

        // Act
        bool result = action.Actionable;

        // Assert
        Assert.False(result);
    }
}
