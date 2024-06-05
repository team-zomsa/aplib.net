using Aplib.Core.Belief.BeliefSets;
using Aplib.Core.Desire.Goals;
using Aplib.Core.Desire.GoalStructures;
using FluentAssertions;
using Moq;
using Moq.Protected;

namespace Aplib.Core.Tests.Desire;

public class GoalStructureTests
{
    [Fact]
    public void PrimitiveGoalStructure_ConstructedWithMetadata_HasCorrectMetadata()
    {
        // Arrange
        Metadata metadata = new("My GoalStructure", "The best GoalStructure");
        Mock<IGoalStructure<IBeliefSet>> goalStructure1 = new();

        // Act
        FirstOfGoalStructure<IBeliefSet> goalStructure = new(metadata, goalStructure1.Object);

        // Assert
        goalStructure.Metadata.Name.Should().Be("My GoalStructure");
        goalStructure.Metadata.Description.Should().Be("The best GoalStructure");
    }

    [Fact]
    public void FirstOfGoalStructure_WhenAllGoalsFail_ShouldReturnFailure()
    {
        Mock<IGoalStructure<IBeliefSet>> goalStructure1 = new();
        goalStructure1.SetupGet(g => g.Status).Returns(CompletionStatus.Failure);
        Mock<IGoalStructure<IBeliefSet>> goalStructure2 = new();
        goalStructure2.SetupGet(g => g.Status).Returns(CompletionStatus.Failure);

        IBeliefSet beliefSet = Mock.Of<IBeliefSet>();
        FirstOfGoalStructure<IBeliefSet> firstOfGoalStructure = new(goalStructure1.Object, goalStructure2.Object);

        // Act
        firstOfGoalStructure.UpdateStatus(beliefSet);

        // Assert
        firstOfGoalStructure.Status.Should().Be(CompletionStatus.Failure);
    }

    [Fact]
    public void FirstOfGoalStructure_WhenDisposing_ShouldDisposeChildren()
    {
        // Arrange
        Mock<IGoalStructure<IBeliefSet>> goalStructure1 = new();
        Mock<IGoalStructure<IBeliefSet>> goalStructure2 = new();

        Mock<FirstOfGoalStructure<IBeliefSet>> firstOfGoalStructure
            = new(goalStructure1.Object, goalStructure2.Object) { CallBase = true };

        // Act
        firstOfGoalStructure.Object.Dispose();

        // Assert
        firstOfGoalStructure.Protected().Verify("Dispose", Times.Once(), ItExpr.IsAny<bool>());
    }

    [Fact]
    public void FirstOfGoalStructure_WhenFinished_ShouldEarlyExit()
    {
        Mock<IGoalStructure<IBeliefSet>> goalStructure1 = new();
        goalStructure1.SetupGet(g => g.Status).Returns(CompletionStatus.Success);

        Mock<IGoalStructure<IBeliefSet>> goalStructure2 = new();
        goalStructure2.SetupGet(g => g.Status).Returns(CompletionStatus.Success);

        IBeliefSet beliefSet = Mock.Of<IBeliefSet>();
        FirstOfGoalStructure<IBeliefSet> firstOfGoalStructure = new(goalStructure1.Object, goalStructure2.Object);

        // Act
        firstOfGoalStructure.UpdateStatus(beliefSet);
        firstOfGoalStructure.UpdateStatus(beliefSet);

        // Assert
        firstOfGoalStructure.Status.Should().Be(CompletionStatus.Success);
        goalStructure1.Verify(x => x.UpdateStatus(It.IsAny<IBeliefSet>()), Times.Once);
        goalStructure2.Verify(x => x.UpdateStatus(It.IsAny<IBeliefSet>()), Times.Never);
    }

    [Fact]
    public void FirstOfGoalStructure_WhenFirstGoalIsFailure_ShouldReturnSecondGoal()
    {
        // Arrange
        Mock<IGoalStructure<IBeliefSet>> goalStructure1 = new();
        goalStructure1.SetupGet(g => g.Status).Returns(CompletionStatus.Failure);
        Mock<IGoalStructure<IBeliefSet>> goalStructure2 = new();
        IGoal<IBeliefSet> goal = Mock.Of<IGoal<IBeliefSet>>();
        goalStructure2
            .Setup(g => g.GetCurrentGoal(It.IsAny<IBeliefSet>()))
            .Returns(goal);

        IBeliefSet beliefSet = Mock.Of<IBeliefSet>();
        FirstOfGoalStructure<IBeliefSet> firstOfGoalStructure = new(goalStructure1.Object, goalStructure2.Object);

        // Act
        firstOfGoalStructure.UpdateStatus(beliefSet);
        IGoal<IBeliefSet> currentGoal = firstOfGoalStructure.GetCurrentGoal(beliefSet);

        // Assert
        firstOfGoalStructure.Status.Should().Be(CompletionStatus.Unfinished);
        currentGoal.Should().Be(goal);
    }

    [Fact]
    public void FirstOfGoalStructure_WhenFirstGoalIsFinished_ShouldReturnSuccess()
    {
        // Arrange
        Mock<IGoalStructure<IBeliefSet>> goalStructure1 = new();
        goalStructure1.SetupGet(g => g.Status).Returns(CompletionStatus.Success);
        Mock<IGoalStructure<IBeliefSet>> goalStructure2 = new();
        IBeliefSet beliefSet = Mock.Of<IBeliefSet>();
        FirstOfGoalStructure<IBeliefSet> firstOfGoalStructure = new(goalStructure1.Object, goalStructure2.Object);

        // Act
        firstOfGoalStructure.UpdateStatus(beliefSet);

        // Assert
        firstOfGoalStructure.Status.Should().Be(CompletionStatus.Success);
    }

    [Fact]
    public void FirstOfGoalStructure_WhenFirstGoalIsUnfinished_ShouldReturnUnfinished()
    {
        // Arrange
        Mock<IGoal<IBeliefSet>> goal = new();
        Mock<IGoalStructure<IBeliefSet>> goalStructure1 = new();
        goalStructure1
            .Setup(g => g.GetCurrentGoal(It.IsAny<IBeliefSet>()))
            .Returns(goal.Object);

        goalStructure1
            .SetupGet(g => g.Status)
            .Returns(CompletionStatus.Unfinished);

        Mock<IGoalStructure<IBeliefSet>> goalStructure2 = new();
        IBeliefSet beliefSet = Mock.Of<IBeliefSet>();
        FirstOfGoalStructure<IBeliefSet> firstOfGoalStructure = new(goalStructure1.Object, goalStructure2.Object);

        // Act
        firstOfGoalStructure.UpdateStatus(beliefSet);
        IGoal<IBeliefSet> currentGoal = firstOfGoalStructure.GetCurrentGoal(beliefSet);

        // Assert
        firstOfGoalStructure.Status.Should().Be(CompletionStatus.Unfinished);
        currentGoal.Should().Be(goal.Object);
    }

    [Fact]
    public void FirstOfGoalStructure_WhenGoalIsUnfinished_ShouldReturnGoal()
    {
        Mock<IGoalStructure<IBeliefSet>> goalStructure1 = new();
        goalStructure1.SetupGet(g => g.Status).Returns(CompletionStatus.Unfinished);

        IGoal<IBeliefSet> goal = Mock.Of<IGoal<IBeliefSet>>();
        goalStructure1.Setup(g => g.GetCurrentGoal(It.IsAny<IBeliefSet>())).Returns(goal);

        Mock<IGoalStructure<IBeliefSet>> goalStructure2 = new();
        goalStructure2.SetupGet(g => g.Status).Returns(CompletionStatus.Unfinished);

        IBeliefSet beliefSet = Mock.Of<IBeliefSet>();
        FirstOfGoalStructure<IBeliefSet> firstOfGoalStructure = new(goalStructure1.Object, goalStructure2.Object);

        // Act
        firstOfGoalStructure.UpdateStatus(beliefSet);
        IGoal<IBeliefSet> currentGoal = firstOfGoalStructure.GetCurrentGoal(beliefSet);

        // Assert
        firstOfGoalStructure.Status.Should().Be(CompletionStatus.Unfinished);
        currentGoal.Should().Be(goal);
    }

    [Theory]
    [InlineData(CompletionStatus.Success, CompletionStatus.Success)]
    [InlineData(CompletionStatus.Failure, CompletionStatus.Failure)]
    [InlineData(CompletionStatus.Unfinished, CompletionStatus.Unfinished)]
    public void PrimitiveGoalStructure_WhenGoalHasState_ShouldHaveSameState(CompletionStatus status,
        CompletionStatus expected)
    {
        // Arrange
        Mock<IGoal<IBeliefSet>> goal = new();
        goal.Setup(g => g.GetStatus(It.IsAny<IBeliefSet>())).Returns(status);
        IBeliefSet beliefSet = Mock.Of<IBeliefSet>();
        PrimitiveGoalStructure<IBeliefSet> primitiveGoalStructure = new(goal.Object);

        // Act
        primitiveGoalStructure.UpdateStatus(beliefSet);

        // Assert
        primitiveGoalStructure.Status.Should().Be(expected);
    }

    [Fact]
    public void PrimitiveGoalStructure_WhenGoalIsNotFinished_ShouldReturnGoal()
    {
        // Arrange
        Mock<IGoal<IBeliefSet>> goal = new();
        goal.Setup(g => g.GetStatus(It.IsAny<IBeliefSet>())).Returns(CompletionStatus.Unfinished);
        IBeliefSet beliefSet = Mock.Of<IBeliefSet>();
        PrimitiveGoalStructure<IBeliefSet> primitiveGoalStructure = new(goal.Object);

        // Act
        primitiveGoalStructure.UpdateStatus(beliefSet);
        IGoal<IBeliefSet> currentGoal = primitiveGoalStructure.GetCurrentGoal(beliefSet);

        // Assert
        currentGoal.Should().Be(goal.Object);
    }

    [Fact]
    public void RepeatGoalStructure_WhenGoalIsNotFinished_ShouldReturnGoal()
    {
        // Arrange
        Mock<IGoal<IBeliefSet>> goal = new();
        goal.Setup(g => g.GetStatus(It.IsAny<IBeliefSet>())).Returns(CompletionStatus.Unfinished);
        IBeliefSet beliefSet = Mock.Of<IBeliefSet>();
        PrimitiveGoalStructure<IBeliefSet> primitiveGoalStructure = new(goal.Object);
        RepeatGoalStructure<IBeliefSet> repeatGoalStructure = new(primitiveGoalStructure);

        // Act
        repeatGoalStructure.UpdateStatus(beliefSet);
        IGoal<IBeliefSet> currentGoal = repeatGoalStructure.GetCurrentGoal(beliefSet);

        // Assert
        repeatGoalStructure.Status.Should().Be(CompletionStatus.Unfinished);
        currentGoal.Should().Be(goal.Object);
    }

    [Fact]
    public void RepeatGoalStructure_WhenGoalStructureHasFailed_ShouldReturnGoal()
    {
        // Arrange
        Mock<IGoal<IBeliefSet>> goal = new();
        goal.Setup(g => g.GetStatus(It.IsAny<IBeliefSet>())).Returns(CompletionStatus.Failure);
        IBeliefSet beliefSet = Mock.Of<IBeliefSet>();
        PrimitiveGoalStructure<IBeliefSet> primitiveGoalStructure = new(goal.Object);
        RepeatGoalStructure<IBeliefSet> repeatGoalStructure = new(primitiveGoalStructure);

        // Act
        repeatGoalStructure.UpdateStatus(beliefSet);
        IGoal<IBeliefSet> currentGoal = repeatGoalStructure.GetCurrentGoal(beliefSet);

        // Assert
        repeatGoalStructure.Status.Should().Be(CompletionStatus.Unfinished);
        currentGoal.Should().Be(goal.Object);
    }

    [Fact]
    public void RepeatGoalStructure_WhenGoalStructureHasSucceeded_ShouldSucceed()
    {
        // Arrange
        Mock<IGoal<IBeliefSet>> goal = new();
        goal.Setup(g => g.GetStatus(It.IsAny<IBeliefSet>())).Returns(CompletionStatus.Success);
        IBeliefSet beliefSet = Mock.Of<IBeliefSet>();
        PrimitiveGoalStructure<IBeliefSet> primitiveGoalStructure = new(goal.Object);
        RepeatGoalStructure<IBeliefSet> repeatGoalStructure = new(primitiveGoalStructure);

        // Act
        repeatGoalStructure.UpdateStatus(beliefSet);
        IGoal<IBeliefSet> currentGoal = repeatGoalStructure.GetCurrentGoal(beliefSet);

        // Assert
        repeatGoalStructure.Status.Should().Be(CompletionStatus.Success);
        currentGoal.Should().Be(goal.Object);
    }

    [Fact]
    public void SequentialGoalStructure_WhenDisposing_ShouldDisposeChildren()
    {
        // Arrange
        Mock<IGoalStructure<IBeliefSet>> goalStructure1 = new();
        Mock<IGoalStructure<IBeliefSet>> goalStructure2 = new();

        Mock<SequentialGoalStructure<IBeliefSet>> sequentialGoalStructure
            = new(goalStructure1.Object, goalStructure2.Object) { CallBase = true };

        // Act
        sequentialGoalStructure.Object.Dispose();

        // Assert
        sequentialGoalStructure.Protected().Verify("Dispose", Times.Once(), ItExpr.IsAny<bool>());
    }

    [Fact]
    public void SequentialGoalStructure_WhenFirstGoalIsFinished_ShouldNotUseFirstGoalAgain()
    {
        Mock<IGoalStructure<IBeliefSet>> goalStructure1 = new();
        goalStructure1.SetupGet(g => g.Status).Returns(CompletionStatus.Success);

        Mock<IGoalStructure<IBeliefSet>> goalStructure2 = new();
        goalStructure2.SetupGet(g => g.Status).Returns(CompletionStatus.Success);

        IBeliefSet beliefSet = Mock.Of<IBeliefSet>();
        SequentialGoalStructure<IBeliefSet> sequentialGoalStructure =
            new(goalStructure1.Object, goalStructure2.Object);

        // Act
        sequentialGoalStructure.UpdateStatus(beliefSet);
        sequentialGoalStructure.UpdateStatus(beliefSet);

        // Assert
        sequentialGoalStructure.Status.Should().Be(CompletionStatus.Success);
        goalStructure1.Verify(x => x.UpdateStatus(It.IsAny<IBeliefSet>()), Times.Once);
    }

    [Fact]
    public void SequentialGoalStructure_WhenFirstGoalIsFinished_ShouldReturnUnfinished()
    {
        // Arrange
        Mock<IGoalStructure<IBeliefSet>> goalStructure1 = new();
        goalStructure1.SetupGet(g => g.Status).Returns(CompletionStatus.Success);
        Mock<IGoalStructure<IBeliefSet>> goalStructure2 = new();
        IBeliefSet beliefSet = Mock.Of<IBeliefSet>();
        SequentialGoalStructure<IBeliefSet> sequentialGoalStructure =
            new(goalStructure1.Object, goalStructure2.Object);

        // Act
        sequentialGoalStructure.UpdateStatus(beliefSet);

        // Assert
        sequentialGoalStructure.Status.Should().Be(CompletionStatus.Unfinished);
    }

    [Fact]
    public void SequentialGoalStructure_WhenGoalFails_ShouldReturnFailure()
    {
        // Arrange
        Mock<IGoalStructure<IBeliefSet>> goalStructure1 = new();
        goalStructure1.SetupGet(g => g.Status).Returns(CompletionStatus.Failure);
        Mock<IGoalStructure<IBeliefSet>> goalStructure2 = new();
        IBeliefSet beliefSet = Mock.Of<IBeliefSet>();
        SequentialGoalStructure<IBeliefSet> sequentialGoalStructure =
            new(goalStructure1.Object, goalStructure2.Object);

        // Act
        sequentialGoalStructure.UpdateStatus(beliefSet);

        // Assert
        sequentialGoalStructure.Status.Should().Be(CompletionStatus.Failure);
    }

    [Fact]
    public void SequentialGoalStructure_WhenProvidingNoGoalStructure_ShouldThrowException()
    {
        // Arrange
        IGoalStructure<IBeliefSet>[] goalStructures = [];

        // Act
        System.Func<SequentialGoalStructure<IBeliefSet>> act =
            () => new SequentialGoalStructure<IBeliefSet>(goalStructures);

        // Assert
        act.Should().Throw<System.ArgumentException>();
    }
}
