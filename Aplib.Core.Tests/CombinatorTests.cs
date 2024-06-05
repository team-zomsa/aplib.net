using Aplib.Core.Belief.BeliefSets;
using Aplib.Core.Desire.Goals;
using Aplib.Core.Desire.GoalStructures;
using Aplib.Core.Intent.Actions;
using Aplib.Core.Intent.Tactics;
using FluentAssertions;
using Moq;
using static Aplib.Core.Combinators;

namespace Aplib.Core.Tests;

public class CombinatorTests
{
    public static readonly object?[][] Metadatas =
    [
        [new Metadata(System.Guid.Empty), null, null],
        [new Metadata(System.Guid.Empty, "my name"), "my name", null],
        [new Metadata(System.Guid.Empty, description: "my description"), null, "my description"],
        [new Metadata(System.Guid.Empty, "a name", "a description"), "a name", "a description"]
    ];

    private static void CheckMetadata(string? expectedName, string? expectedDescription, IMetadata actual)
    {
        actual.Id.Should().BeEmpty();
        actual.Name.Should().Be(expectedName);
        actual.Description.Should().Be(expectedDescription);
    }

    private static void CheckDefaultMetadata(IMetadata actual)
    {
        actual.Id.Should().NotBeEmpty();
        actual.Name.Should().BeNull();
        actual.Description.Should().BeNull();
    }

    #region GoalStructure combinator tests

    [Theory]
    [MemberData(nameof(Metadatas))]
    public void FirstOfGoalStructureCombinator_WhenCalled_GivesExpectedGoalStructure
        (Metadata metadata, string? expectedName, string? expectedDescription)
    {
        // Arrange
        Mock<IGoalStructure<IBeliefSet>> goalStructure1 = new();
        goalStructure1.SetupGet(g => g.Status).Returns(CompletionStatus.Success);

        Mock<IGoalStructure<IBeliefSet>> goalStructure2 = new();
        goalStructure2.SetupGet(g => g.Status).Returns(CompletionStatus.Success);

        IBeliefSet beliefSet = Mock.Of<IBeliefSet>();

        // Act
        FirstOfGoalStructure<IBeliefSet> firstOfGoalStructure =
            FirstOf(metadata, goalStructure1.Object, goalStructure2.Object);

        firstOfGoalStructure.UpdateStatus(beliefSet);
        firstOfGoalStructure.UpdateStatus(beliefSet);

        // Assert
        CheckMetadata(expectedName, expectedDescription, firstOfGoalStructure.Metadata);
        firstOfGoalStructure.Status.Should().Be(CompletionStatus.Success);
        goalStructure1.Verify(x => x.UpdateStatus(It.IsAny<IBeliefSet>()), Times.Once);
        goalStructure2.Verify(x => x.UpdateStatus(It.IsAny<IBeliefSet>()), Times.Never);
    }

    [Fact]
    public void FirstOfGoalStructureCombinator_WithoutMetadata_GivesExpectedGoalStructure()
    {
        // Arrange
        Mock<IGoalStructure<IBeliefSet>> goalStructure1 = new();
        goalStructure1.SetupGet(g => g.Status).Returns(CompletionStatus.Success);

        Mock<IGoalStructure<IBeliefSet>> goalStructure2 = new();
        goalStructure2.SetupGet(g => g.Status).Returns(CompletionStatus.Success);

        IBeliefSet beliefSet = Mock.Of<IBeliefSet>();

        // Act
        FirstOfGoalStructure<IBeliefSet> firstOfGoalStructure =
            FirstOf(goalStructure1.Object, goalStructure2.Object);

        firstOfGoalStructure.UpdateStatus(beliefSet);
        firstOfGoalStructure.UpdateStatus(beliefSet);

        // Assert
        CheckDefaultMetadata(firstOfGoalStructure.Metadata);
        firstOfGoalStructure.Status.Should().Be(CompletionStatus.Success);
        goalStructure1.Verify(x => x.UpdateStatus(It.IsAny<IBeliefSet>()), Times.Once);
        goalStructure2.Verify(x => x.UpdateStatus(It.IsAny<IBeliefSet>()), Times.Never);
    }

    [Theory]
    [MemberData(nameof(Metadatas))]
    public void PrimitiveGoalStructureCombinator_WhenCalled_GivesExpectedGoalStructure
        (Metadata metadata, string? expectedName, string? expectedDescription)
    {
        // Arrange
        Mock<IGoal<IBeliefSet>> goal = new();
        goal.Setup(g => g.GetStatus(It.IsAny<IBeliefSet>())).Returns(CompletionStatus.Success);
        IBeliefSet beliefSet = Mock.Of<IBeliefSet>();

        // Act
        PrimitiveGoalStructure<IBeliefSet> primitiveGoalStructure = Primitive(metadata, goal.Object);

        primitiveGoalStructure.UpdateStatus(beliefSet);

        // Assert
        CheckMetadata(expectedName, expectedDescription, primitiveGoalStructure.Metadata);
        primitiveGoalStructure.Status.Should().Be(CompletionStatus.Success);
    }

    [Fact]
    public void PrimitiveGoalStructureCombinator_WithoutMetadata_GivesExpectedGoalStructure()
    {
        // Arrange
        Mock<IGoal<IBeliefSet>> goal = new();
        goal.Setup(g => g.GetStatus(It.IsAny<IBeliefSet>())).Returns(CompletionStatus.Success);
        IBeliefSet beliefSet = Mock.Of<IBeliefSet>();

        // Act
        PrimitiveGoalStructure<IBeliefSet> primitiveGoalStructure = Primitive(goal.Object);

        primitiveGoalStructure.UpdateStatus(beliefSet);

        // Assert
        CheckDefaultMetadata(primitiveGoalStructure.Metadata);
        primitiveGoalStructure.Status.Should().Be(CompletionStatus.Success);
    }

    [Theory]
    [MemberData(nameof(Metadatas))]
    public void RepeatGoalStructureCombinator_WhenCalled_GivesExpectedGoalStructure
        (Metadata metadata, string? expectedName, string? expectedDescription)
    {
        // Arrange
        Mock<IGoal<IBeliefSet>> goal = new();
        goal.Setup(g => g.GetStatus(It.IsAny<IBeliefSet>())).Returns(CompletionStatus.Failure);
        IBeliefSet beliefSet = Mock.Of<IBeliefSet>();

        // Act
        RepeatGoalStructure<IBeliefSet> repeatGoalStructure = Repeat(metadata, Primitive(goal.Object));

        repeatGoalStructure.UpdateStatus(beliefSet);
        IGoal<IBeliefSet> currentGoal = repeatGoalStructure.GetCurrentGoal(beliefSet);

        // Assert
        CheckMetadata(expectedName, expectedDescription, repeatGoalStructure.Metadata);
        repeatGoalStructure.Status.Should().Be(CompletionStatus.Unfinished);
        currentGoal.Should().Be(goal.Object);
    }

    [Fact]
    public void RepeatGoalStructureCombinator_WithoutMetadata_GivesExpectedGoalStructure()
    {
        // Arrange
        Mock<IGoal<IBeliefSet>> goal = new();
        goal.Setup(g => g.GetStatus(It.IsAny<IBeliefSet>())).Returns(CompletionStatus.Failure);
        IBeliefSet beliefSet = Mock.Of<IBeliefSet>();

        // Act
        RepeatGoalStructure<IBeliefSet> repeatGoalStructure = Repeat(Primitive(goal.Object));

        repeatGoalStructure.UpdateStatus(beliefSet);
        IGoal<IBeliefSet> currentGoal = repeatGoalStructure.GetCurrentGoal(beliefSet);

        // Assert
        CheckDefaultMetadata(repeatGoalStructure.Metadata);
        repeatGoalStructure.Status.Should().Be(CompletionStatus.Unfinished);
        currentGoal.Should().Be(goal.Object);
    }

    [Theory]
    [MemberData(nameof(Metadatas))]
    public void SequentialGoalStructureCombinator_WhenCalled_GivesExpectedGoalStructure
        (Metadata metadata, string? expectedName, string? expectedDescription)
    {
        Mock<IGoalStructure<IBeliefSet>> goalStructure1 = new();
        goalStructure1.SetupGet(g => g.Status).Returns(CompletionStatus.Success);

        Mock<IGoalStructure<IBeliefSet>> goalStructure2 = new();
        goalStructure2.SetupGet(g => g.Status).Returns(CompletionStatus.Success);

        IBeliefSet beliefSet = Mock.Of<IBeliefSet>();

        // Act
        SequentialGoalStructure<IBeliefSet> sequentialGoalStructure =
            Seq(metadata, goalStructure1.Object, goalStructure2.Object);

        sequentialGoalStructure.UpdateStatus(beliefSet);
        sequentialGoalStructure.UpdateStatus(beliefSet);

        // Assert
        CheckMetadata(expectedName, expectedDescription, sequentialGoalStructure.Metadata);
        sequentialGoalStructure.Status.Should().Be(CompletionStatus.Success);
        goalStructure1.Verify(x => x.UpdateStatus(It.IsAny<IBeliefSet>()), Times.Once);
    }

    [Fact]
    public void SequentialGoalStructureCombinator_WithoutMetadata_GivesExpectedGoalStructure()
    {
        Mock<IGoalStructure<IBeliefSet>> goalStructure1 = new();
        goalStructure1.SetupGet(g => g.Status).Returns(CompletionStatus.Success);

        Mock<IGoalStructure<IBeliefSet>> goalStructure2 = new();
        goalStructure2.SetupGet(g => g.Status).Returns(CompletionStatus.Success);

        IBeliefSet beliefSet = Mock.Of<IBeliefSet>();

        // Act
        SequentialGoalStructure<IBeliefSet> sequentialGoalStructure = Seq(goalStructure1.Object, goalStructure2.Object);

        sequentialGoalStructure.UpdateStatus(beliefSet);
        sequentialGoalStructure.UpdateStatus(beliefSet);

        // Assert
        CheckDefaultMetadata(sequentialGoalStructure.Metadata);
        sequentialGoalStructure.Status.Should().Be(CompletionStatus.Success);
        goalStructure1.Verify(x => x.UpdateStatus(It.IsAny<IBeliefSet>()), Times.Once);
    }

    #endregion

    #region Tactic combinator tests

    [Theory]
    [MemberData(nameof(Metadatas))]
    public void AnyOfTacticCombinator_WhenCalled_GivesExpectedTactic
        (Metadata metadata, string? expectedName, string? expectedDescription)
    {
        // Arrange
        Action<IBeliefSet> action1 = new(_ => { });
        Action<IBeliefSet> action2 = new(_ => { });
        // ReSharper disable once ConvertToLocalFunction
        System.Func<IBeliefSet, bool> guard = _ => false;

        // Act
        AnyOfTactic<IBeliefSet> anyOfTactic =
            AnyOf(metadata, guard, Primitive(action1, _ => true), Primitive(action2, _ => false));

        IAction<IBeliefSet>? selectedAction = anyOfTactic.GetAction(It.IsAny<IBeliefSet>());

        // Assert
        CheckMetadata(expectedName, expectedDescription, anyOfTactic.Metadata);
        selectedAction.Should().BeNull();
    }

    [Fact]
    public void AnyOfTacticCombinator_WithoutMetadata_GivesExpectedTactic()
    {
        // Arrange
        Action<IBeliefSet> action1 = new(_ => { });
        Action<IBeliefSet> action2 = new(_ => { });
        // ReSharper disable once ConvertToLocalFunction
        System.Func<IBeliefSet, bool> guard = _ => false;

        // Act
        AnyOfTactic<IBeliefSet> anyOfTactic =
            AnyOf(guard, Primitive(action1, _ => true), Primitive(action2, _ => false));

        IAction<IBeliefSet>? selectedAction = anyOfTactic.GetAction(It.IsAny<IBeliefSet>());

        // Assert
        CheckDefaultMetadata(anyOfTactic.Metadata);
        selectedAction.Should().BeNull();
    }

    [Theory]
    [MemberData(nameof(Metadatas))]
    public void AnyOfTacticCombinator_WithoutGuard_GivesExpectedTactic
        (Metadata metadata, string? expectedName, string? expectedDescription)
    {
        // Arrange
        Action<IBeliefSet> action1 = new(_ => { });
        Action<IBeliefSet> action2 = new(_ => { });

        // Act
        AnyOfTactic<IBeliefSet> anyOfTactic =
            AnyOf(metadata, Primitive(action1, _ => true), Primitive(action2, _ => false));

        IAction<IBeliefSet>? selectedAction = anyOfTactic.GetAction(It.IsAny<IBeliefSet>());

        // Assert
        CheckMetadata(expectedName, expectedDescription, anyOfTactic.Metadata);
        selectedAction.Should().Be(action1);
    }

    [Fact]
    public void AnyOfTacticCombinator_WithoutMetadataWithoutGuard_GivesExpectedTactic()
    {
        // Arrange
        Action<IBeliefSet> action1 = new(_ => { });
        Action<IBeliefSet> action2 = new(_ => { });

        // Act
        AnyOfTactic<IBeliefSet> anyOfTactic = AnyOf(Primitive(action1, _ => true), Primitive(action2, _ => false));

        IAction<IBeliefSet>? selectedAction = anyOfTactic.GetAction(It.IsAny<IBeliefSet>());

        // Assert
        CheckDefaultMetadata(anyOfTactic.Metadata);
        selectedAction.Should().Be(action1);
    }

    [Theory]
    [MemberData(nameof(Metadatas))]
    public void FirstOfTacticCombinator_WhenCalled_GivesExpectedTactic
        (Metadata metadata, string? expectedName, string? expectedDescription)
    {
        // Arrange
        Action<IBeliefSet> action1 = new(_ => { });
        Action<IBeliefSet> action2 = new(_ => { });
        // ReSharper disable once ConvertToLocalFunction
        System.Func<IBeliefSet, bool> guard = _ => false;

        // Act
        FirstOfTactic<IBeliefSet> firstOfTactic =
            FirstOf(metadata, guard, Primitive(action1, _ => false), Primitive(action2, _ => true));

        IAction<IBeliefSet>? selectedAction = firstOfTactic.GetAction(It.IsAny<IBeliefSet>());

        // Assert
        CheckMetadata(expectedName, expectedDescription, firstOfTactic.Metadata);
        selectedAction.Should().BeNull();
    }

    [Fact]
    public void FirstOfTacticCombinator_WithoutMetadata_GivesExpectedTactic()
    {
        // Arrange
        Action<IBeliefSet> action1 = new(_ => { });
        Action<IBeliefSet> action2 = new(_ => { });
        // ReSharper disable once ConvertToLocalFunction
        System.Func<IBeliefSet, bool> guard = _ => false;

        // Act
        FirstOfTactic<IBeliefSet> firstOfTactic =
            FirstOf(guard, Primitive(action1, _ => false), Primitive(action2, _ => true));

        IAction<IBeliefSet>? selectedAction = firstOfTactic.GetAction(It.IsAny<IBeliefSet>());

        // Assert
        CheckDefaultMetadata(firstOfTactic.Metadata);
        selectedAction.Should().BeNull();
    }

    [Theory]
    [MemberData(nameof(Metadatas))]
    public void FirstOfTacticCombinator_WithoutGuard_GivesExpectedTactic
        (Metadata metadata, string? expectedName, string? expectedDescription)
    {
        // Arrange
        Action<IBeliefSet> action1 = new(_ => { });
        Action<IBeliefSet> action2 = new(_ => { });

        // Act
        FirstOfTactic<IBeliefSet> firstOfTactic =
            FirstOf(metadata, Primitive(action1, _ => false), Primitive(action2, _ => true));

        IAction<IBeliefSet>? selectedAction = firstOfTactic.GetAction(It.IsAny<IBeliefSet>());

        // Assert
        CheckMetadata(expectedName, expectedDescription, firstOfTactic.Metadata);
        selectedAction.Should().Be(action2);
    }

    [Fact]
    public void FirstOfTacticCombinator_WithoutMetadataWithoutGuard_GivesExpectedTactic()
    {
        // Arrange
        Action<IBeliefSet> action1 = new(_ => { });
        Action<IBeliefSet> action2 = new(_ => { });

        // Act
        FirstOfTactic<IBeliefSet> firstOfTactic =
            FirstOf(Primitive(action1, _ => false), Primitive(action2, _ => true));

        IAction<IBeliefSet>? selectedAction = firstOfTactic.GetAction(It.IsAny<IBeliefSet>());

        // Assert
        CheckDefaultMetadata(firstOfTactic.Metadata);
        selectedAction.Should().Be(action2);
    }

    [Theory]
    [MemberData(nameof(Metadatas))]
    public void PrimitiveTacticCombinator_WhenCalled_GivesExpectedTactic
        (Metadata metadata, string? expectedName, string? expectedDescription)
    {
        // Arrange
        Action<IBeliefSet> action = new(_ => { });
        Mock<System.Func<IBeliefSet, bool>> guard = new();
        guard.SetupSequence(f => f(It.IsAny<IBeliefSet>())).Returns(false).Returns(true);

        // Act
        PrimitiveTactic<IBeliefSet> primitiveTactic = Primitive(metadata, action, guard.Object);

        IAction<IBeliefSet>? actionFromFirstCall = primitiveTactic.GetAction(It.IsAny<IBeliefSet>());
        IAction<IBeliefSet>? actionFromSecondCall = primitiveTactic.GetAction(It.IsAny<IBeliefSet>());

        // Assert
        CheckMetadata(expectedName, expectedDescription, primitiveTactic.Metadata);
        actionFromFirstCall.Should().BeNull();
        actionFromSecondCall.Should().Be(action);
    }

    [Fact]
    public void PrimitiveTacticCombinator_WithoutMetadata_GivesExpectedTactic()
    {
        // Arrange
        Action<IBeliefSet> action = new(_ => { });
        Mock<System.Func<IBeliefSet, bool>> guard = new();
        guard.SetupSequence(f => f(It.IsAny<IBeliefSet>())).Returns(false).Returns(true);

        // Act
        PrimitiveTactic<IBeliefSet> primitiveTactic = Primitive(action, guard.Object);

        IAction<IBeliefSet>? actionFromFirstCall = primitiveTactic.GetAction(It.IsAny<IBeliefSet>());
        IAction<IBeliefSet>? actionFromSecondCall = primitiveTactic.GetAction(It.IsAny<IBeliefSet>());

        // Assert
        CheckDefaultMetadata(primitiveTactic.Metadata);
        actionFromFirstCall.Should().BeNull();
        actionFromSecondCall.Should().Be(action);
    }

    [Theory]
    [MemberData(nameof(Metadatas))]
    public void PrimitiveTacticCombinator_WithoutGuard_GivesExpectedTactic
        (Metadata metadata, string? expectedName, string? expectedDescription)
    {
        // Arrange
        Action<IBeliefSet> action = new(_ => { });

        // Act
        PrimitiveTactic<IBeliefSet> primitiveTactic = Primitive(metadata, action);

        bool isActionable = primitiveTactic.IsActionable(It.IsAny<IBeliefSet>());
        IAction<IBeliefSet>? selectedAction = primitiveTactic.GetAction(It.IsAny<IBeliefSet>());

        // Assert
        CheckMetadata(expectedName, expectedDescription, primitiveTactic.Metadata);
        isActionable.Should().BeTrue();
        selectedAction.Should().Be(action);
    }

    [Fact]
    public void PrimitiveTacticCombinator_WithoutMetadataWithoutGuard_GivesExpectedTactic()
    {
        // Arrange
        Action<IBeliefSet> action = new(_ => { });

        // Act
        PrimitiveTactic<IBeliefSet> primitiveTactic = Primitive(action);

        bool isActionable = primitiveTactic.IsActionable(It.IsAny<IBeliefSet>());
        IAction<IBeliefSet>? selectedAction = primitiveTactic.GetAction(It.IsAny<IBeliefSet>());

        // Assert
        CheckDefaultMetadata(primitiveTactic.Metadata);
        isActionable.Should().BeTrue();
        selectedAction.Should().Be(action);
    }

    [Theory]
    [MemberData(nameof(Metadatas))]
    public void PrimitiveTacticCombinator_FromQueryable_GivesExpectedTactic
        (Metadata metadata, string? expectedName, string? expectedDescription)
    {
        // Arrange
        Mock<System.Func<IBeliefSet, int>> query = new();
        query.SetupSequence(f => f(It.IsAny<IBeliefSet>())).Returns(1).Returns(2);
        QueryAction<IBeliefSet, int> queryAction = new((_, _) => { }, query.Object);
        Mock<System.Func<IBeliefSet, bool>> guard = new();
        guard.SetupSequence(f => f(It.IsAny<IBeliefSet>())).Returns(false).Returns(true);

        // Act
        PrimitiveTactic<IBeliefSet> primitiveTactic = Primitive(metadata, queryAction, guard.Object);

        IAction<IBeliefSet>? actionFromFirstCall = primitiveTactic.GetAction(It.IsAny<IBeliefSet>());
        IAction<IBeliefSet>? actionFromSecondCall = primitiveTactic.GetAction(It.IsAny<IBeliefSet>());

        // Assert
        CheckMetadata(expectedName, expectedDescription, primitiveTactic.Metadata);
        actionFromFirstCall.Should().BeNull();
        actionFromSecondCall.Should().Be(queryAction);
    }

    [Fact]
    public void PrimitiveTacticCombinator_FromQueryableWithoutMetadata_GivesExpectedTactic()
    {
        // Arrange
        Mock<System.Func<IBeliefSet, int>> query = new();
        query.SetupSequence(f => f(It.IsAny<IBeliefSet>())).Returns(1).Returns(2);
        QueryAction<IBeliefSet, int> queryAction = new((_, _) => { }, query.Object);
        Mock<System.Func<IBeliefSet, bool>> guard = new();
        guard.SetupSequence(f => f(It.IsAny<IBeliefSet>())).Returns(false).Returns(true);

        // Act
        PrimitiveTactic<IBeliefSet> primitiveTactic = Primitive(queryAction, guard.Object);

        IAction<IBeliefSet>? actionFromFirstCall = primitiveTactic.GetAction(It.IsAny<IBeliefSet>());
        IAction<IBeliefSet>? actionFromSecondCall = primitiveTactic.GetAction(It.IsAny<IBeliefSet>());

        // Assert
        CheckDefaultMetadata(primitiveTactic.Metadata);
        actionFromFirstCall.Should().BeNull();
        actionFromSecondCall.Should().Be(queryAction);
    }

    [Theory]
    [MemberData(nameof(Metadatas))]
    public void PrimitiveTacticCombinator_FromQueryableWithoutGuard_GivesExpectedTactic
        (Metadata metadata, string? expectedName, string? expectedDescription)
    {
        // Arrange
        Mock<System.Func<IBeliefSet, int>> query = new();
        query.SetupSequence(f => f(It.IsAny<IBeliefSet>())).Returns(1).Returns(2);
        QueryAction<IBeliefSet, int> queryAction = new((_, _) => { }, query.Object);

        // Act
        PrimitiveTactic<IBeliefSet> primitiveTactic = Primitive(metadata, queryAction);

        bool isActionable = primitiveTactic.IsActionable(It.IsAny<IBeliefSet>());
        IAction<IBeliefSet>? selectedAction = primitiveTactic.GetAction(It.IsAny<IBeliefSet>());

        // Assert
        CheckMetadata(expectedName, expectedDescription, primitiveTactic.Metadata);
        isActionable.Should().BeTrue();
        selectedAction.Should().Be(queryAction);
    }

    [Fact]
    public void PrimitiveTacticCombinator_FromQueryableWithoutMetadataWithoutGuard_GivesExpectedTactic()
    {
        // Arrange
        Mock<System.Func<IBeliefSet, int>> query = new();
        query.SetupSequence(f => f(It.IsAny<IBeliefSet>())).Returns(1).Returns(2);
        QueryAction<IBeliefSet, int> queryAction = new((_, _) => { }, query.Object);

        // Act
        PrimitiveTactic<IBeliefSet> primitiveTactic = Primitive(queryAction);

        bool isActionable = primitiveTactic.IsActionable(It.IsAny<IBeliefSet>());
        IAction<IBeliefSet>? selectedAction = primitiveTactic.GetAction(It.IsAny<IBeliefSet>());

        // Assert
        CheckDefaultMetadata(primitiveTactic.Metadata);
        isActionable.Should().BeTrue();
        selectedAction.Should().Be(queryAction);
    }

    #endregion
}
