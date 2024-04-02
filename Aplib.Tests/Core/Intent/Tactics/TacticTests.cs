using Aplib.Core.Belief;
using Aplib.Core.Intent.Actions;
using Aplib.Core.Intent.Tactics;
using FluentAssertions;
using Moq;

namespace Aplib.Tests.Core.Intent.Tactics;

public class TacticTests
{
    private static string _result = "abc";
    private readonly Action<IBeliefSet> _emptyAction = new(_ => { });
    private readonly Action<IBeliefSet> _filledAction = new(_ => _result = "def");

    /// <summary>
    /// Given a tactic with a guard that returns true and an action,
    /// When calling the Execute method,
    /// Then _result should be "def".
    /// </summary>
    [Fact]
    public void Execute_WhenGuardReturnsTrue_ActionIsExecuted()
    {
        // Arrange
        PrimitiveTactic<IBeliefSet> tactic = new(_filledAction, guard: TrueGuard);

        // Act
        IAction<IBeliefSet> action = tactic.GetAction(It.IsAny<IBeliefSet>())!;
        action.Execute(It.IsAny<IBeliefSet>());

        // Assert
        _result.Should().Be("def");
    }

    /// <summary>
    /// Given a parent of type <see cref="AnyOfTactic" /> with two subtactics,
    /// When getting the next tactic,
    /// Then the result should contain all the subtactics.
    /// </summary>
    [Fact]
    public void GetAction_WhenTacticTypeIsAnyOf_ReturnsEnabledPrimitiveTactics()
    {
        // Arrange
        PrimitiveTactic<IBeliefSet> tactic1 = new(_emptyAction);
        PrimitiveTactic<IBeliefSet> tactic2 = new(_emptyAction);
        AnyOfTactic<IBeliefSet> parentTactic = new(null, tactic1, tactic2);

        // Act
        IAction<IBeliefSet>? enabledAction = parentTactic.GetAction(It.IsAny<IBeliefSet>());

        // Assert
        enabledAction.Should().NotBeNull();
        enabledAction!.Should().Be(_emptyAction);
    }

    /// <summary>
    /// Given a parent of type <see cref="FirstOfTactic" /> with two subtactics,
    /// When getting the next tactic,
    /// Then the result should be the first subtactic.
    /// </summary>
    [Fact]
    public void GetAction_WhenTacticTypeIsFirstOf_ReturnsEnabledPrimitiveTactics()
    {
        // Arrange
        PrimitiveTactic<IBeliefSet> tactic1 = new(_emptyAction);
        PrimitiveTactic<IBeliefSet> tactic2 = new(_filledAction);
        FirstOfTactic<IBeliefSet> parentTactic = new(null, tactic1, tactic2);

        // Act
        IAction<IBeliefSet>? enabledAction = parentTactic.GetAction(It.IsAny<IBeliefSet>());

        // Assert
        enabledAction.Should().NotBeNull();
        enabledAction!.Should().Be(_emptyAction);
    }

    /// <summary>
    /// Given a parent of type <see cref="FirstOfTactic" /> with two subtactics and a guard that is true,
    /// When getting the next tactic,
    /// Then the result should be the first subtactic.
    /// </summary>
    [Fact]
    public void GetAction_WhenTacticTypeIsFirstOfAndGuardEnabled_ReturnsEnabledPrimitiveTactics()
    {
        // Arrange
        PrimitiveTactic<IBeliefSet> tactic1 = new(_emptyAction);
        PrimitiveTactic<IBeliefSet> tactic2 = new(_filledAction);
        FirstOfTactic<IBeliefSet> parentTactic = new(TrueGuard, null, tactic1, tactic2);

        // Act
        IAction<IBeliefSet>? enabledAction = parentTactic.GetAction(It.IsAny<IBeliefSet>());

        // Assert
        enabledAction.Should().NotBeNull();
        enabledAction!.Should().Be(_emptyAction);
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
        PrimitiveTactic<IBeliefSet> tactic = new(_emptyAction, TrueGuard);

        // Act
        IAction<IBeliefSet>? enabledAction = tactic.GetAction(It.IsAny<IBeliefSet>());

        // Assert
        enabledAction.Should().NotBeNull();
        enabledAction!.Should().Be(_emptyAction);
    }

    /// <summary>
    /// Given a primitive tactic with a non-actionable action,
    /// When getting the first enabled actions,
    /// Then the result should be null.
    /// </summary>
    [Fact]
    public void GetAction_WhenTacticTypeIsPrimitiveAndActionIsNotActionable_ReturnsNoAction()
    {
        // Arrange
        PrimitiveTactic<IBeliefSet> tactic = new(_emptyAction, FalseGuard);

        // Act
        IAction<IBeliefSet>? enabledAction = tactic.GetAction(It.IsAny<IBeliefSet>());

        // Assert
        enabledAction.Should().BeNull();
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
        PrimitiveTactic<IBeliefSet> tactic = new(_emptyAction, FalseGuard);

        // Act
        bool isActionable = tactic.IsActionable(It.IsAny<IBeliefSet>());

        // Assert
        isActionable.Should().BeFalse();
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
        PrimitiveTactic<IBeliefSet> tactic = new(_filledAction, TrueGuard);

        // Act
        bool isActionable = tactic.IsActionable(It.IsAny<IBeliefSet>());

        // Assert
        isActionable.Should().BeTrue();
    }

    private static bool FalseGuard(IBeliefSet _) => false;

    private static bool TrueGuard(IBeliefSet _) => true;
}
