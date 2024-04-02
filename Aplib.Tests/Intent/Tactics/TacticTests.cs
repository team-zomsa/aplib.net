using Aplib.Core.Belief;
using Aplib.Core.Intent.Actions;
using Aplib.Core.Intent.Tactics;
using FluentAssertions;
using Moq;

namespace Aplib.Core.Tests.Intent.Tactics;

public class TacticTests
{
    /// <summary>
    /// Given a tactic and an action,
    /// When the guard of the tactic returns true,
    /// Then an action should be selected.
    /// </summary>
    [Fact]
    public void PrimitveTacticExecute_WhenGuardReturnsTrue_ActionIsSelected()
    {
        // Arrange
        Mock<IAction<IBeliefSet>> action = new();
        action.Setup(a => a.IsActionable(It.IsAny<IBeliefSet>())).Returns(true);
        PrimitiveTactic<IBeliefSet> tactic = new(action.Object, guard: _ => true);

        // Act
        IAction<IBeliefSet> selectedAction = tactic.GetAction(It.IsAny<IBeliefSet>())!;

        // Assert
        selectedAction.Should().NotBeNull();
    }

    /// <summary>
    /// Given a parent of type <see cref="AnyOfTactic{TBeliefSet}" /> with two subtactics,
    /// When getting the next tactic,
    /// Then the result should be the action of an enabled tactic.
    /// </summary>
    [Fact]
    public void AnyOfTacticGetAction_WhenASubtacticIsEnabled_ReturnsActionOfEnabledSubtactic()
    {
        // Arrange
        Action<IBeliefSet> action1 = new(_ => { });
        Action<IBeliefSet> action2 = new(_ => { });
        PrimitiveTactic<IBeliefSet> tactic1 = new(action1, _ => true);
        PrimitiveTactic<IBeliefSet> tactic2 = new(action2, _ => false);
        AnyOfTactic<IBeliefSet> parentTactic = new(null, tactic1, tactic2);

        // Act
        IAction<IBeliefSet>? selectedAction = parentTactic.GetAction(It.IsAny<IBeliefSet>());

        // Assert
        selectedAction.Should().Be(action1);
    }

    /// <summary>
    /// Given a parent of type <see cref="FirstOfTactic{TBeliefSet}" /> with two subtactics,
    /// When both subtactic guards are true,
    /// Then the result should be the first subtactic.
    /// </summary>
    [Fact]
    public void FirstOfTacticGetAction_WhenBothSubtacticsEnabled_ReturnsFirstSubtactic()
    {
        // Arrange
        Action<IBeliefSet> action1 = new(_ => { });
        Action<IBeliefSet> action2 = new(_ => { });
        PrimitiveTactic<IBeliefSet> tactic1 = new(action1, _ => true);
        PrimitiveTactic<IBeliefSet> tactic2 = new(action2, _ => true);
        FirstOfTactic<IBeliefSet> parentTactic = new(null, tactic1, tactic2);

        // Act
        IAction<IBeliefSet>? selectedAction = parentTactic.GetAction(It.IsAny<IBeliefSet>());

        // Assert
        selectedAction.Should().Be(action1);
    }

    /// <summary>
    /// Given a parent of type <see cref="FirstOfTactic{TBeliefSet}" /> with two subtactics,
    /// When the first subtactic guard is false and the second is true,
    /// Then the result should be the second subtactic.
    /// </summary>
    [Fact]
    public void FirstOfTacticGetAction_WhenFirstSubtacticIsDisabled_ReturnsSecondSubtactic()
    {
        // Arrange
        Action<IBeliefSet> action1 = new(_ => { });
        Action<IBeliefSet> action2 = new(_ => { });
        PrimitiveTactic<IBeliefSet> tactic1 = new(action1, _ => false);
        PrimitiveTactic<IBeliefSet> tactic2 = new(action2, _ => true);
        FirstOfTactic<IBeliefSet> parentTactic = new(null, tactic1, tactic2);

        // Act
        IAction<IBeliefSet>? selectedAction = parentTactic.GetAction(It.IsAny<IBeliefSet>());

        // Assert
        selectedAction.Should().Be(action2);
    }

    /// <summary>
    /// Given a primitive tactic with an actionable action,
    /// When calling GetAction,
    /// Then the result should be the action of the tactic.
    /// </summary>
    [Fact]
    public void PrimitiveTacticGetAction_WhenTacticTypeIsPrimitiveAndActionIsActionable_ReturnsAction()
    {
        // Arrange
        Action<IBeliefSet> action = new(_ => { }, _ => true);
        PrimitiveTactic<IBeliefSet> tactic = new(action);

        // Act
        IAction<IBeliefSet>? enabledAction = tactic.GetAction(It.IsAny<IBeliefSet>());

        // Assert
        enabledAction.Should().Be(action);
    }

    /// <summary>
    /// Given a primitive tactic with a non-actionable action,
    /// When calling GetAction,
    /// Then the result should be null.
    /// </summary>
    [Fact]
    public void PrimitiveTactic_WhenTacticTypeIsPrimitiveAndActionIsNotActionable_ReturnsNoAction()
    {
        // Arrange
        Action<IBeliefSet> action = new(_ => { }, _ => false);
        PrimitiveTactic<IBeliefSet> tactic = new(action);

        // Act
        IAction<IBeliefSet>? enabledAction = tactic.GetAction(It.IsAny<IBeliefSet>());

        // Assert
        enabledAction.Should().Be(null);
    }

    /// <summary>
    /// Given a tactic with a guard that returns false,
    /// When checking if the tactic is actionable,
    /// Then the result should be false.
    /// </summary>
    [Fact]
    public void PrimitiveTacticIsActionable_WhenGuardReturnsFalse_ReturnsFalse()
    {
        // Arrange
        PrimitiveTactic<IBeliefSet> tactic = new(It.IsAny<IAction<IBeliefSet>>(), _ => false);

        // Act
        bool isActionable = tactic.IsActionable(It.IsAny<IBeliefSet>());

        // Assert
        isActionable.Should().BeFalse();
    }

    /// <summary>
    /// Given a tactic with a guard that returns true and an actionable action,
    /// When checking if the tactic is actionable,
    /// Then the result should be true.
    /// </summary>
    [Fact]
    public void PrimitiveTacticIsActionable_WhenGuardReturnsTrueAndActionIsActionable_ReturnsTrue()
    {
        // Arrange
        Action<IBeliefSet> action = new(_ => { }, _ => true);
        PrimitiveTactic<IBeliefSet> tactic = new(action, _ => true);

        // Act
        bool isActionable = tactic.IsActionable(It.IsAny<IBeliefSet>());

        // Assert
        isActionable.Should().BeTrue();
    }
}
