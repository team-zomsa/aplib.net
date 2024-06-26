// This program has been developed by students from the bachelor Computer Science at Utrecht
// University within the Software Project course.
// Copyright Utrecht University (Department of Information and Computing Sciences)

using Aplib.Core.Belief.BeliefSets;
using Aplib.Core.Intent.Actions;
using Aplib.Core.Intent.Tactics;
using Aplib.Core.Logging;
using FluentAssertions;
using Moq;
using System.Collections.Generic;

namespace Aplib.Core.Tests.Intent.Tactics;

public class TacticTests
{
    // A subclass of Tactic for testing
    public class TestTactic : Tactic<IBeliefSet>
    {
        public System.Predicate<IBeliefSet> Guard => _guard;

        public TestTactic(Metadata metadata, System.Predicate<IBeliefSet> guard) : base(metadata, guard) { }

        public TestTactic(System.Predicate<IBeliefSet> guard) : base(guard) { }

        public TestTactic(Metadata metadata) : base(metadata) { }

        public TestTactic() { }

        public override IAction<IBeliefSet> GetAction(IBeliefSet beliefSet)
            => throw new System.NotImplementedException();
        public override IEnumerable<ILoggable> GetLogChildren()
            => throw new System.NotImplementedException();
    }

    // A subclass of FirstOfTactic for testing
    public class TestFirstOfTactic : FirstOfTactic<IBeliefSet>
    {
        public System.Predicate<IBeliefSet> Guard => _guard;

        public LinkedList<ITactic<IBeliefSet>> Subtactics => _subtactics;

        public TestFirstOfTactic
            (Metadata metadata, System.Predicate<IBeliefSet> guard, params ITactic<IBeliefSet>[] subtactics)
            : base(metadata, guard, subtactics)
        {
        }

        public TestFirstOfTactic(Metadata metadata, params ITactic<IBeliefSet>[] subtactics)
            : base(metadata, subtactics)
        {
        }

        public TestFirstOfTactic(System.Predicate<IBeliefSet> guard, params ITactic<IBeliefSet>[] subtactics)
            : base(guard, subtactics)
        {
        }

        public TestFirstOfTactic(params ITactic<IBeliefSet>[] subtactics) : base(subtactics) { }
    }


    [Fact]
    public void Tactic_WhenConstructed_HasExpectedData()
    {
        // Arrange
        Metadata metadata = It.IsAny<Metadata>();
        System.Predicate<IBeliefSet> guard = It.IsAny<System.Predicate<IBeliefSet>>();

        // Act
        TestTactic tactic = new(metadata, guard);

        // Assert
        tactic.Metadata.Should().Be(metadata);
        tactic.Guard.Should().Be(guard);
    }

    [Fact]
    public void Tactic_WithoutMetadata_HasExpectedData()
    {
        // Arrange
        System.Predicate<IBeliefSet> guard = It.IsAny<System.Predicate<IBeliefSet>>();

        // Act
        TestTactic tactic = new(guard);

        // Assert
        tactic.Metadata.Id.Should().NotBeEmpty();
        tactic.Metadata.Name.Should().BeNull();
        tactic.Metadata.Description.Should().BeNull();
        tactic.Guard.Should().Be(guard);
    }

    [Fact]
    public void Tactic_WithoutGuard_HasExpectedData()
    {
        // Arrange
        Metadata metadata = It.IsAny<Metadata>();

        // Act
        TestTactic tactic = new(metadata);

        // Assert
        tactic.Metadata.Should().Be(metadata);
        tactic.Guard(It.IsAny<IBeliefSet>()).Should().BeTrue();
    }

    [Fact]
    public void Tactic_Default_HasExpectedData()
    {
        // Act
        TestTactic tactic = new();

        // Assert
        tactic.Metadata.Id.Should().NotBeEmpty();
        tactic.Metadata.Name.Should().BeNull();
        tactic.Metadata.Description.Should().BeNull();
        tactic.Guard(It.IsAny<IBeliefSet>()).Should().BeTrue();
    }

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
        PrimitiveTactic<IBeliefSet> tactic = new(action.Object, guard: _ => true);

        // Act
        IAction<IBeliefSet> selectedAction = tactic.GetAction(It.IsAny<IBeliefSet>())!;

        // Assert
        selectedAction.Should().NotBeNull();
    }

    /// <summary>
    /// Given no metadata,
    /// When an AnyOfTactic is constructed,
    /// Then it has no name, and a random id.
    /// </summary>
    [Fact]
    public void AnyOfTactic_WithoutMetadata_ContainsDefaultMetadata()
    {
        // Act
        RandomTactic<IBeliefSet> tactic = new(_ => true);

        // Assert
        tactic.Metadata.Id.Should().NotBeEmpty();
        tactic.Metadata.Name.Should().BeNull();
        tactic.Metadata.Description.Should().BeNull();
    }

    /// <summary>
    /// Given a parent of type <see cref="RandomTactic{TBeliefSet}" /> with two subtactics,
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
        RandomTactic<IBeliefSet> parentTactic = new(tactic1, tactic2);

        // Act
        IAction<IBeliefSet>? selectedAction = parentTactic.GetAction(It.IsAny<IBeliefSet>());

        // Assert
        selectedAction.Should().Be(action1);
    }

    [Fact]
    public void AnyOfTactic_WithFalseGuard_ReturnsNoAction()
    {
        // Arrange
        Action<IBeliefSet> action = new(_ => { });
        PrimitiveTactic<IBeliefSet> tactic = new(action, _ => true);
        RandomTactic<IBeliefSet> parentTactic = new(_ => false, tactic);

        // Act
        IAction<IBeliefSet>? selectedAction = parentTactic.GetAction(It.IsAny<IBeliefSet>());

        // Assert
        selectedAction.Should().BeNull();
    }

    [Fact]
    public void FirstOfTacticTactic_WhenConstructed_HasExpectedData()
    {
        // Arrange
        Metadata metadata = It.IsAny<Metadata>();
        System.Predicate<IBeliefSet> guard = It.IsAny<System.Predicate<IBeliefSet>>();
        ITactic<IBeliefSet>[] subtactics = [It.IsAny<ITactic<IBeliefSet>>()];

        // Act
        TestFirstOfTactic tactic = new(metadata, guard, subtactics);

        // Assert
        tactic.Metadata.Should().Be(metadata);
        tactic.Guard.Should().Be(guard);
        tactic.Subtactics.Should().BeEquivalentTo(subtactics);
    }

    [Fact]
    public void FirstOfTacticTactic_WithoutGuard_HasExpectedData()
    {
        // Arrange
        Metadata metadata = It.IsAny<Metadata>();
        ITactic<IBeliefSet>[] subtactics = [It.IsAny<ITactic<IBeliefSet>>()];

        // Act
        TestFirstOfTactic tactic = new(metadata, subtactics);

        // Assert
        tactic.Metadata.Should().Be(metadata);
        tactic.Guard(It.IsAny<IBeliefSet>()).Should().BeTrue();
        tactic.Subtactics.Should().BeEquivalentTo(subtactics);
    }

    [Fact]
    public void FirstOfTacticTactic_WithoutMetadata_HasExpectedData()
    {
        // Arrange
        System.Predicate<IBeliefSet> guard = It.IsAny<System.Predicate<IBeliefSet>>();
        ITactic<IBeliefSet>[] subtactics = [It.IsAny<ITactic<IBeliefSet>>()];

        // Act
        TestFirstOfTactic tactic = new(guard, subtactics);

        // Assert
        tactic.Metadata.Id.Should().NotBeEmpty();
        tactic.Metadata.Name.Should().BeNull();
        tactic.Metadata.Description.Should().BeNull();
        tactic.Guard.Should().Be(guard);
        tactic.Subtactics.Should().BeEquivalentTo(subtactics);
    }

    [Fact]
    public void FirstOfTacticTactic_WithoutMetadataWithoutGuard_HasExpectedData()
    {
        // Arrange
        ITactic<IBeliefSet>[] subtactics = [It.IsAny<ITactic<IBeliefSet>>()];

        // Act
        TestFirstOfTactic tactic = new(subtactics);

        // Assert
        tactic.Metadata.Id.Should().NotBeEmpty();
        tactic.Metadata.Name.Should().BeNull();
        tactic.Metadata.Description.Should().BeNull();
        tactic.Guard(It.IsAny<IBeliefSet>()).Should().BeTrue();
        tactic.Subtactics.Should().BeEquivalentTo(subtactics);
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
        FirstOfTactic<IBeliefSet> parentTactic = new(tactic1, tactic2);

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
        FirstOfTactic<IBeliefSet> parentTactic = new(tactic1, tactic2);

        // Act
        IAction<IBeliefSet>? selectedAction = parentTactic.GetAction(It.IsAny<IBeliefSet>());

        // Assert
        selectedAction.Should().Be(action2);
    }

    [Fact]
    public void FirstOfTactic_WithFalseGuard_ReturnsNoAction()
    {
        // Arrange
        Action<IBeliefSet> action = new(_ => { });
        PrimitiveTactic<IBeliefSet> tactic = new(action, _ => true);
        FirstOfTactic<IBeliefSet> parentTactic = new(_ => false, tactic);

        // Act
        IAction<IBeliefSet>? selectedAction = parentTactic.GetAction(It.IsAny<IBeliefSet>());

        // Assert
        selectedAction.Should().BeNull();
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
        Action<IBeliefSet> action = new(_ => { });
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
        Action<IBeliefSet> action = new(_ => { });
        PrimitiveTactic<IBeliefSet> tactic = new(action, _ => false);

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
        Action<IBeliefSet> action = new(_ => { });
        PrimitiveTactic<IBeliefSet> tactic = new(action, _ => true);

        // Act
        bool isActionable = tactic.IsActionable(It.IsAny<IBeliefSet>());

        // Assert
        isActionable.Should().BeTrue();
    }

    [Fact]
    public void PrimitiveTacticIsActionable_WhenGuardReturnsTrueAndHasQuery_ReturnsCorrectQuery()
    {
        // Arrange
        int result = 0;
        QueryAction<IBeliefSet, int> action = new((_, b) => result = b, _ => 42);
        PrimitiveTactic<IBeliefSet> tactic = new(action, _ => true);
        IBeliefSet beliefSet = Mock.Of<IBeliefSet>();

        // Act
        bool isActionable = tactic.IsActionable(beliefSet);
        tactic.GetAction(beliefSet)!.Execute(beliefSet);

        // Assert
        isActionable.Should().BeTrue();
        result.Should().Be(42);
    }

    [Fact]
    public void PrimitiveTacticIsActionable_WhenGuardReturnsTrueAndHasNullQuery_IsNotActionable()
    {
        // Arrange
        int result = 0;
        QueryAction<IBeliefSet, int?> action = new((_, b) => result = b!.Value,
            _ =>
            {
                int? x = null;
                return x;
            }
        );
        PrimitiveTactic<IBeliefSet> tactic = new(action, _ => true);
        IBeliefSet beliefSet = Mock.Of<IBeliefSet>();

        // Act
        bool isActionable = tactic.IsActionable(beliefSet);

        // Assert
        isActionable.Should().BeFalse();
        result.Should().Be(0);
    }
}
