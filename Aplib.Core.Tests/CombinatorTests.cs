using Aplib.Core.Belief.BeliefSets;
using Aplib.Core.Desire.Goals;
using Aplib.Core.Desire.GoalStructures;
using FluentAssertions;
using Moq;
using System.Collections.Generic;
using static Aplib.Core.Combinators;

namespace Aplib.Core.Tests;

public class CombinatorTests
{
    public static readonly IEnumerable<object?[]> Metadatas =
    [
        [new Metadata(System.Guid.Empty), null, null],
        [new Metadata(System.Guid.Empty, "my name"), "my name", null],
        [new Metadata(System.Guid.Empty, description: "my description"), null, "my description"],
        [new Metadata(System.Guid.Empty, "a name", "a description"), "a name", "a description"]
    ];

    private static void CheckMetadata(string? expectedName, string? expectedDescription, Metadata actual)
    {
        actual.Id.Should().BeEmpty();
        actual.Name.Should().Be(expectedName);
        actual.Description.Should().Be(expectedDescription);
    }

    private static void CheckDefaultMetadata(Metadata actual)
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
}
