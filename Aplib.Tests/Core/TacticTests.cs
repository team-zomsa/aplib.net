using Aplib.Core;
using Action = Aplib.Core.Action;
using Tactic = Aplib.Core.Tactic;

namespace Aplib.Tests.Core;
public class TacticTests
{
    private readonly Action _emptyAction = new(() => { });
    private static bool TrueGuard() => true;
    private static bool FalseGuard() => false;

    /// <summary>
    /// Given a tactic without a parent,
    /// When getting the next tactic,
    /// Then the result should be null.
    /// </summary>
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

    /// <summary>
    /// Given a parent with one subtactic,
    /// When getting the next tactic,
    /// Then the result should be the same tactic.
    /// </summary>
    [Fact]
    public void GetNextTactic_WhenTacticTypeIsPrimitive_ReturnsPrimitiveTactic()
    {
        // Arrange
        PrimitiveTactic tactic = new(_emptyAction);
        _ = new Tactic(TacticType.FirstOf, [tactic]);

        // Act
        Tactic? nextTactic = tactic.GetNextTactic();

        // Assert
        Assert.Equal(tactic, nextTactic);
    }

    /// <summary>
    /// Given a parent of type <see cref="TacticType.FirstOf"/> with two subtactics,
    /// When getting the next tactic,
    /// Then the result should be the first subtactic.
    /// </summary>
    [Fact]
    public void GetFirstEnabledActions_WhenTacticTypeIsFirstOf_ReturnsEnabledPrimitiveTactics()
    {
        // Arrange
        PrimitiveTactic tactic1 = new(_emptyAction);
        PrimitiveTactic tactic2 = new(_emptyAction);
        Tactic parentTactic = new(TacticType.FirstOf, [tactic1, tactic2]);

        // Act
        List<PrimitiveTactic> enabledActions = parentTactic.GetFirstEnabledActions();

        // Assert
        Assert.Contains(tactic1, enabledActions);
    }

    /// <summary>
    /// Given a parent of type <see cref="TacticType.AnyOf"/> with two subtactics,
    /// When getting the next tactic,
    /// Then the result should contain all the subtactics.
    /// </summary>
    [Fact]
    public void GetFirstEnabledActions_WhenTacticTypeIsAnyOf_ReturnsEnabledPrimitiveTactics()
    {
        // Arrange
        PrimitiveTactic tactic1 = new(_emptyAction);
        PrimitiveTactic tactic2 = new(_emptyAction);
        Tactic parentTactic = new(TacticType.AnyOf, [tactic1, tactic2]);

        // Act
        List<PrimitiveTactic> enabledActions = parentTactic.GetFirstEnabledActions();

        // Assert
        Assert.Contains(tactic1, enabledActions);
        Assert.Contains(tactic2, enabledActions);
    }

    /// <summary>
    /// Given a primitive tactic with an actionable action,
    /// When getting the first enabled actions,
    /// Then the result should contain the primitive tactic.
    /// </summary>
    [Fact]
    public void GetFirstEnabledActions_WhenTacticTypeIsPrimitiveAndActionIsActionable_ReturnsEnabledPrimitiveTactic()
    {
        // Arrange
        PrimitiveTactic tactic = new(_emptyAction, TrueGuard);

        // Act
        List<PrimitiveTactic> enabledActions = tactic.GetFirstEnabledActions();

        // Assert
        Assert.Contains(tactic, enabledActions);
    }

    /// <summary>
    /// Given a primitive tactic with a non-actionable action,
    /// When getting the first enabled actions,
    /// Then the result should be an empty list.
    /// </summary>
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

    /// <summary>
    /// Given a tactic with a guard that returns true,
    /// When checking if the tactic is actionable,
    /// Then the result should be true.
    /// </summary>
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

    /// <summary>
    /// Given a tactic with a guard that returns false,
    /// When checking if the tactic is actionable,
    /// Then the result should be false.
    /// </summary>
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
