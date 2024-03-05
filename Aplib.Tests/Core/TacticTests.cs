using Aplib.Core;
using Action = Aplib.Core.Action;
using Tactic = Aplib.Core.Tactic;

namespace Aplib.Tests.Core;
public class TacticTests
{
    private readonly Action _emptyAction = new(() => { });
    private static bool TrueGuard() => true;
    private static bool FalseGuard() => false;

    [Fact]
    public void GetNextTactic_WhenParentIsNull_ReturnsNull()
    {
        // Arrange
        Tactic tactic = new(TacticType.Primitive, []);

        // Act
        Tactic? nextTactic = tactic.GetNextTactic();

        // Assert
        Assert.Null(nextTactic);
    }

    [Fact]
    public void GetNextTactic_WhenTacticTypeIsPrimitive_ReturnsPrimitiveTactic()
    {
        // Arrange
        Tactic tactic = new PrimitiveTactic(_emptyAction);
        Tactic parentTactic = new(TacticType.FirstOf, [tactic]);
        tactic.Parent = parentTactic;

        // Act
        Tactic? nextTactic = tactic.GetNextTactic();

        // Assert
        Assert.Equal(tactic, nextTactic);
    }

    [Fact]
    public void GetFirstEnabledActions_WhenTacticTypeIsFirstOf_ReturnsEnabledPrimitiveTactics()
    {
        // Arrange
        Tactic tactic1 = new PrimitiveTactic(_emptyAction);
        Tactic tactic2 = new PrimitiveTactic(_emptyAction);
        Tactic parentTactic = new(TacticType.FirstOf, [tactic1, tactic2]);

        // Act
        List<PrimitiveTactic> enabledActions = parentTactic.GetFirstEnabledActions();

        // Assert
        Assert.Contains(tactic1, enabledActions);
    }

    [Fact]
    public void GetFirstEnabledActions_WhenTacticTypeIsAnyOf_ReturnsEnabledPrimitiveTactics()
    {
        // Arrange
        Tactic tactic1 = new PrimitiveTactic(_emptyAction);
        Tactic tactic2 = new PrimitiveTactic(_emptyAction);
        Tactic parentTactic = new(TacticType.AnyOf, [tactic1, tactic2]);

        // Act
        List<PrimitiveTactic> enabledActions = parentTactic.GetFirstEnabledActions();

        // Assert
        Assert.Contains(tactic1, enabledActions);
        Assert.Contains(tactic2, enabledActions);
    }

    [Fact]
    public void GetFirstEnabledActions_WhenTacticTypeIsPrimitiveAndActionIsActionable_ReturnsEnabledPrimitiveTactic()
    {
        // Arrange
        Tactic tactic = new PrimitiveTactic(_emptyAction, TrueGuard);

        // Act
        List<PrimitiveTactic> enabledActions = tactic.GetFirstEnabledActions();

        // Assert
        Assert.Contains(tactic, enabledActions);
    }

    [Fact]
    public void GetFirstEnabledActions_WhenTacticTypeIsPrimitiveAndActionIsNotActionable_ReturnsEmptyList()
    {
        // Arrange
        PrimitiveTactic tactic = new(_emptyAction, FalseGuard);

        // Act
        List<PrimitiveTactic> enabledActions = tactic.GetFirstEnabledActions();

        // Assert
        Assert.Empty(enabledActions);
    }

    [Fact]
    public void IsActionable_WhenGuardReturnsTrue_ReturnsTrue()
    {
        // Arrange
        Tactic tactic = new(TacticType.Primitive, [], TrueGuard);

        // Act
        bool isActionable = tactic.IsActionable();

        // Assert
        Assert.True(isActionable);
    }

    [Fact]
    public void IsActionable_WhenGuardReturnsFalse_ReturnsFalse()
    {
        // Arrange
        PrimitiveTactic tactic = new(_emptyAction, FalseGuard);

        // Act
        bool isActionable = tactic.IsActionable();

        // Assert
        Assert.False(isActionable);
    }
}
