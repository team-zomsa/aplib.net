using Aplib.Core.Tactics;
using FluentAssertions;
using Action = Aplib.Core.Action;

namespace Aplib.Tests.Core.Tactics;
public class TacticTests
{
    private readonly Action _emptyAction = new(effect: () => { });
    private static string _result = "abc";
    private readonly Action _filledAction = new(effect: () => _result = "def");

    private static bool TrueGuard() => true;

    private static bool FalseGuard() => false;

    /// <summary>
    /// Given a parent of type <see cref="TacticType.FirstOf"/> with two subtactics,
    /// When getting the next tactic,
    /// Then the result should be the first subtactic.
    /// </summary>
    [Fact]
    public void GetAction_WhenTacticTypeIsFirstOf_ReturnsEnabledPrimitiveTactics()
    {
        // Arrange
        PrimitiveTactic tactic1 = new(_emptyAction);
        PrimitiveTactic tactic2 = new(_filledAction);
        FirstOfTactic parentTactic = new([tactic1, tactic2]);

        // Act
        Action? enabledAction = parentTactic.GetAction();

        // Assert
        Assert.Equal(_emptyAction, enabledAction);
    }

    /// <summary>
    /// Given a parent of type <see cref="TacticType.FirstOf"/> with two subtactics and a guard that is true,
    /// When getting the next tactic,
    /// Then the result should be the first subtactic.
    /// </summary>
    [Fact]
    public void GetAction_WhenTacticTypeIsFirstOfAndGuardEnabled_ReturnsEnabledPrimitiveTactics()
    {
        // Arrange
        PrimitiveTactic tactic1 = new(_emptyAction);
        PrimitiveTactic tactic2 = new(_filledAction);
        FirstOfTactic parentTactic = new(TrueGuard, [tactic1, tactic2]);

        // Act
        Action? enabledAction = parentTactic.GetAction();

        // Assert
        Assert.Equal(_emptyAction, enabledAction);
    }

    /// <summary>
    /// Given a parent of type <see cref="TacticType.AnyOf"/> with two subtactics,
    /// When getting the next tactic,
    /// Then the result should contain all the subtactics.
    /// </summary>
    [Fact]
    public void GetAction_WhenTacticTypeIsAnyOf_ReturnsEnabledPrimitiveTactics()
    {
        // Arrange
        PrimitiveTactic tactic1 = new(_emptyAction);
        PrimitiveTactic tactic2 = new(_emptyAction);
        AnyOfTactic parentTactic = new([tactic1, tactic2]);

        // Act
        Action? enabledAction = parentTactic.GetAction();

        // Assert
        Assert.Equal(_emptyAction, enabledAction);
    }

    /// <summary>
    /// Given a primitive tactic with an actionable action,
    /// When getting the first enabled actions,
    /// Then the result should contain the primitive tactic.
    /// </summary>
    [Fact]
    public void GetAction_WhenTacticTypeIsPrimitiveAndActionIsActionable_ReturnsEnabledPrimitiveTactic()
    {
        // Arrange
        PrimitiveTactic tactic = new(_emptyAction, TrueGuard);

        // Act
        Action? enabledAction = tactic.GetAction();

        // Assert
        Assert.Equal(_emptyAction, enabledAction);
    }

    /// <summary>
    /// Given a primitive tactic with a non-actionable action,
    /// When getting the first enabled actions,
    /// Then the result should be an empty list.
    /// </summary>
    [Fact]
    public void GetAction_WhenTacticTypeIsPrimitiveAndActionIsNotActionable_ReturnsEmptyList()
    {
        // Arrange
        PrimitiveTactic tactic = new(_emptyAction, FalseGuard);

        // Act
        Action? enabledAction = tactic.GetAction();

        // Assert
        Assert.Null(enabledAction);
    }

    /// <summary>
    /// Given a tactic with a guard that returns true and an action,
    /// When calling the Execute method,
    /// Then _result should be "def".
    /// </summary>
    [Fact]
    public void Execute_WhenGuardReturnsTrue_ActionIsExecuted()
    {
        // Arrange
        PrimitiveTactic tactic = new(_filledAction, TrueGuard);

        // Act
        tactic.GetAction()!.Execute();

        // Assert
        Assert.Equal("def", _result);
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
        PrimitiveTactic tactic = new(_filledAction, TrueGuard);

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
