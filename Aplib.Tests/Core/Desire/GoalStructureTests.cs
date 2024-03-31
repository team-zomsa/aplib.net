using Aplib.Core.Belief;
using Aplib.Core.Desire;
using Aplib.Core.Desire.Goals;
using FluentAssertions;
using Moq;
using Moq.Protected;

namespace Aplib.Tests.Core.Desire;

public class GoalStructureTests
{
    [Fact]
    public void FirstOfGoalStructure_WhenAllGoalsFail_ShouldReturnFailure()
    {
        Mock<IGoalStructure<BeliefSet>> goalStructure1 = new();
        goalStructure1.SetupGet(g => g.State).Returns(CompletionStatus.Failure);
        Mock<IGoalStructure<BeliefSet>> goalStructure2 = new();
        goalStructure2.SetupGet(g => g.State).Returns(CompletionStatus.Failure);

        BeliefSet beliefSet = Mock.Of<BeliefSet>();
        FirstOfGoalStructure<BeliefSet> firstOfGoalStructure = new(new List<IGoalStructure<BeliefSet>>
        {
            goalStructure1.Object, goalStructure2.Object
        });

        // Act
        firstOfGoalStructure.UpdateState(beliefSet);

        // Assert
        firstOfGoalStructure.State.Should().Be(CompletionStatus.Failure);
    }

    [Fact]
    public void FirstOfGoalStructure_WhenDisposing_ShouldDisposeChildren()
    {
        // Arrange
        Mock<IGoalStructure<BeliefSet>> goalStructure1 = new();
        Mock<IGoalStructure<BeliefSet>> goalStructure2 = new();

        Mock<FirstOfGoalStructure<BeliefSet>> firstOfGoalStructure =
            new(new List<IGoalStructure<BeliefSet>> { goalStructure1.Object, goalStructure2.Object })
            {
                CallBase = true
            };

        // Act
        firstOfGoalStructure.Object.Dispose();

        // Assert
        firstOfGoalStructure.Protected().Verify("Dispose", Times.Once(), ItExpr.IsAny<bool>());
    }

    [Fact]
    public void FirstOfGoalStructure_WhenFinished_ShouldEarlyExit()
    {
        Mock<IGoalStructure<BeliefSet>> goalStructure1 = new();
        goalStructure1.SetupGet(g => g.State).Returns(CompletionStatus.Success);

        Mock<IGoalStructure<BeliefSet>> goalStructure2 = new();
        goalStructure2.SetupGet(g => g.State).Returns(CompletionStatus.Success);

        BeliefSet beliefSet = Mock.Of<BeliefSet>();
        FirstOfGoalStructure<BeliefSet> firstOfGoalStructure = new(new List<IGoalStructure<BeliefSet>>
        {
            goalStructure1.Object, goalStructure2.Object
        });

        firstOfGoalStructure.UpdateState(beliefSet);

        // Act
        firstOfGoalStructure.UpdateState(beliefSet);

        // Assert
        firstOfGoalStructure.State.Should().Be(CompletionStatus.Success);
        goalStructure1.Verify(x => x.UpdateState(It.IsAny<BeliefSet>()), Times.Once);
    }

    [Fact]
    public void FirstOfGoalStructure_WhenFirstGoalIsFailure_ShouldReturnSecondGoal()
    {
        // Arrange
        Mock<IGoalStructure<BeliefSet>> goalStructure1 = new();
        goalStructure1.SetupGet(g => g.State).Returns(CompletionStatus.Failure);
        Mock<IGoalStructure<BeliefSet>> goalStructure2 = new();
        IGoal goal = Mock.Of<IGoal>();
        goalStructure2
            .Setup(g => g.GetCurrentGoal(It.IsAny<BeliefSet>()))
            .Returns(goal);

        BeliefSet beliefSet = Mock.Of<BeliefSet>();
        FirstOfGoalStructure<BeliefSet> firstOfGoalStructure = new(new List<IGoalStructure<BeliefSet>>
        {
            goalStructure1.Object, goalStructure2.Object
        });

        // Act
        firstOfGoalStructure.UpdateState(beliefSet);
        IGoal currentGoal = firstOfGoalStructure.GetCurrentGoal(beliefSet)!;

        // Assert
        firstOfGoalStructure.State.Should().Be(CompletionStatus.Unfinished);
        currentGoal.Should().Be(goal);
    }

    [Fact]
    public void FirstOfGoalStructure_WhenFirstGoalIsFinished_ShouldReturnSuccess()
    {
        // Arrange
        Mock<IGoalStructure<BeliefSet>> goalStructure1 = new();
        goalStructure1.SetupGet(g => g.State).Returns(CompletionStatus.Success);
        Mock<IGoalStructure<BeliefSet>> goalStructure2 = new();
        BeliefSet beliefSet = Mock.Of<BeliefSet>();
        FirstOfGoalStructure<BeliefSet> firstOfGoalStructure = new(new List<IGoalStructure<BeliefSet>>
        {
            goalStructure1.Object, goalStructure2.Object
        });

        // Act
        firstOfGoalStructure.UpdateState(beliefSet);

        // Assert
        firstOfGoalStructure.State.Should().Be(CompletionStatus.Success);
    }

    [Fact]
    public void FirstOfGoalStructure_WhenFirstGoalIsUnfinished_ShouldReturnUnfinished()
    {
        // Arrange
        Mock<IGoal> goal = new();
        Mock<IGoalStructure<BeliefSet>> goalStructure1 = new();
        goalStructure1
            .Setup(g => g.GetCurrentGoal(It.IsAny<BeliefSet>()))
            .Returns(goal.Object);

        goalStructure1
            .SetupGet(g => g.State)
            .Returns(CompletionStatus.Unfinished);

        Mock<IGoalStructure<BeliefSet>> goalStructure2 = new();
        BeliefSet beliefSet = Mock.Of<BeliefSet>();
        FirstOfGoalStructure<BeliefSet> firstOfGoalStructure = new(new List<IGoalStructure<BeliefSet>>
        {
            goalStructure1.Object, goalStructure2.Object
        });

        // Act
        firstOfGoalStructure.UpdateState(beliefSet);
        IGoal? currentGoal = firstOfGoalStructure.GetCurrentGoal(beliefSet);

        // Assert
        firstOfGoalStructure.State.Should().Be(CompletionStatus.Unfinished);
        currentGoal.Should().Be(goal.Object);
    }

    [Fact]
    public void FirstOfGoalStructure_WhenGoalIsUnfinished_ShouldReturnGoal()
    {
        Mock<IGoalStructure<BeliefSet>> goalStructure1 = new();
        goalStructure1.SetupGet(g => g.State).Returns(CompletionStatus.Unfinished);

        IGoal goal = Mock.Of<IGoal>();
        goalStructure1.Setup(g => g.GetCurrentGoal(It.IsAny<BeliefSet>())).Returns(goal);

        Mock<IGoalStructure<BeliefSet>> goalStructure2 = new();
        goalStructure2.SetupGet(g => g.State).Returns(CompletionStatus.Unfinished);

        BeliefSet beliefSet = Mock.Of<BeliefSet>();
        FirstOfGoalStructure<BeliefSet> firstOfGoalStructure = new(new List<IGoalStructure<BeliefSet>>
        {
            goalStructure1.Object, goalStructure2.Object
        });

        // Act
        firstOfGoalStructure.UpdateState(beliefSet);
        IGoal? currentGoal = firstOfGoalStructure.GetCurrentGoal(beliefSet);

        // Assert
        firstOfGoalStructure.State.Should().Be(CompletionStatus.Unfinished);
        currentGoal.Should().Be(goal);
    }

    [Theory]
    [InlineData(CompletionStatus.Success, CompletionStatus.Success)]
    [InlineData(CompletionStatus.Failure, CompletionStatus.Unfinished)]
    [InlineData(CompletionStatus.Unfinished, CompletionStatus.Unfinished)]
    public void FirstOfGoalStructure_WhenInterrupted_ShouldRecalculateState(CompletionStatus input,
        CompletionStatus expected)
    {
        // Arrange
        Mock<IGoalStructure<BeliefSet>> goalStructure1 = new();
        goalStructure1.SetupGet(g => g.State).Returns(input);
        IGoal goal = Mock.Of<IGoal>();
        goalStructure1
            .Setup(g => g.GetCurrentGoal(It.IsAny<BeliefSet>()))
            .Returns(goal);

        Mock<IGoalStructure<BeliefSet>> goalStructure2 = new();
        goalStructure2.SetupGet(g => g.State).Returns(CompletionStatus.Success);

        BeliefSet beliefSet = Mock.Of<BeliefSet>();
        FirstOfGoalStructure<BeliefSet> firstOfGoalStructure = new(new List<IGoalStructure<BeliefSet>>
        {
            goalStructure1.Object, goalStructure2.Object
        });
        firstOfGoalStructure.UpdateState(beliefSet);

        // Act
        firstOfGoalStructure.Interrupt(beliefSet);
        firstOfGoalStructure.Reinstate(beliefSet);
        IGoal currentGoal = firstOfGoalStructure.GetCurrentGoal(beliefSet)!;

        // Assert
        firstOfGoalStructure.State.Should().Be(expected);
        currentGoal.Should().Be(goal);
    }

    [Theory]
    [InlineData(GoalState.Success, CompletionStatus.Success)]
    [InlineData(GoalState.Failure, CompletionStatus.Failure)]
    [InlineData(GoalState.Unfinished, CompletionStatus.Unfinished)]
    public void PrimitiveGoalStructure_WhenGoalHasState_ShouldHaveSameState(GoalState state,
        CompletionStatus expected)
    {
        // Arrange
        Mock<IGoal> goal = new();
        goal.Setup(g => g.GetState(It.IsAny<IBeliefSet>())).Returns(state);
        BeliefSet beliefSet = Mock.Of<BeliefSet>();
        PrimitiveGoalStructure<BeliefSet> primitiveGoalStructure = new(goal.Object);

        // Act
        primitiveGoalStructure.UpdateState(beliefSet);

        // Assert
        primitiveGoalStructure.State.Should().Be(expected);
    }

    [Fact]
    public void PrimitiveGoalStructure_WhenGoalIsNotFinished_ShouldReturnGoal()
    {
        // Arrange
        Mock<IGoal> goal = new();
        goal.Setup(g => g.GetState(It.IsAny<IBeliefSet>())).Returns(GoalState.Unfinished);
        BeliefSet beliefSet = Mock.Of<BeliefSet>();
        PrimitiveGoalStructure<BeliefSet> primitiveGoalStructure = new(goal.Object);

        // Act
        primitiveGoalStructure.UpdateState(beliefSet);
        IGoal? currentGoal = primitiveGoalStructure.GetCurrentGoal(beliefSet);

        // Assert
        currentGoal.Should().Be(goal.Object);
    }

    [Fact]
    public void RepeatGoalStructure_WhenGoalIsNotFinished_ShouldReturnGoal()
    {
        // Arrange
        Mock<IGoal> goal = new();
        goal.Setup(g => g.GetState(It.IsAny<IBeliefSet>())).Returns(GoalState.Unfinished);
        BeliefSet beliefSet = Mock.Of<BeliefSet>();
        PrimitiveGoalStructure<BeliefSet> primitiveGoalStructure = new(goal.Object);
        RepeatGoalStructure<BeliefSet> repeatGoalStructure = new(primitiveGoalStructure);

        // Act
        repeatGoalStructure.UpdateState(beliefSet);
        IGoal? currentGoal = repeatGoalStructure.GetCurrentGoal(beliefSet);

        // Assert
        repeatGoalStructure.State.Should().Be(CompletionStatus.Unfinished);
        currentGoal.Should().Be(goal.Object);
    }

    [Fact]
    public void RepeatGoalStructure_WhenGoalStructureHasFailed_ShouldReturnGoal()
    {
        // Arrange
        Mock<IGoal> goal = new();
        goal.Setup(g => g.GetState(It.IsAny<IBeliefSet>())).Returns(GoalState.Failure);
        BeliefSet beliefSet = Mock.Of<BeliefSet>();
        PrimitiveGoalStructure<BeliefSet> primitiveGoalStructure = new(goal.Object);
        RepeatGoalStructure<BeliefSet> repeatGoalStructure = new(primitiveGoalStructure);

        // Act
        repeatGoalStructure.UpdateState(beliefSet);
        IGoal? currentGoal = repeatGoalStructure.GetCurrentGoal(beliefSet);

        // Assert
        repeatGoalStructure.State.Should().Be(CompletionStatus.Unfinished);
        currentGoal.Should().Be(goal.Object);
    }

    [Fact]
    public void RepeatGoalStructure_WhenGoalStructureHasSucceeded_ShouldSucceed()
    {
        // Arrange
        Mock<IGoal> goal = new();
        goal.Setup(g => g.GetState(It.IsAny<IBeliefSet>())).Returns(GoalState.Success);
        BeliefSet beliefSet = Mock.Of<BeliefSet>();
        PrimitiveGoalStructure<BeliefSet> primitiveGoalStructure = new(goal.Object);
        RepeatGoalStructure<BeliefSet> repeatGoalStructure = new(primitiveGoalStructure);

        // Act
        repeatGoalStructure.UpdateState(beliefSet);
        IGoal? currentGoal = repeatGoalStructure.GetCurrentGoal(beliefSet);

        // Assert
        repeatGoalStructure.State.Should().Be(CompletionStatus.Success);
        currentGoal.Should().Be(goal.Object);
    }

    [Fact]
    public void SequentialGoalStructure_WhenDisposing_ShouldDisposeChildren()
    {
        // Arrange
        Mock<IGoalStructure<BeliefSet>> goalStructure1 = new();
        Mock<IGoalStructure<BeliefSet>> goalStructure2 = new();

        Mock<SequentialGoalStructure<BeliefSet>> sequentialGoalStructure =
            new(new List<IGoalStructure<BeliefSet>> { goalStructure1.Object, goalStructure2.Object })
            {
                CallBase = true
            };

        // Act
        sequentialGoalStructure.Object.Dispose();

        // Assert
        sequentialGoalStructure.Protected().Verify("Dispose", Times.Once(), ItExpr.IsAny<bool>());
    }

    [Fact]
    public void SequentialGoalStructure_WhenFinished_ShouldEarlyExit()
    {
        Mock<IGoalStructure<BeliefSet>> goalStructure1 = new();
        goalStructure1.SetupGet(g => g.State).Returns(CompletionStatus.Success);

        Mock<IGoalStructure<BeliefSet>> goalStructure2 = new();
        goalStructure2.SetupGet(g => g.State).Returns(CompletionStatus.Success);

        BeliefSet beliefSet = Mock.Of<BeliefSet>();
        SequentialGoalStructure<BeliefSet> sequentialGoalStructure = new(new List<IGoalStructure<BeliefSet>>
        {
            goalStructure1.Object, goalStructure2.Object
        });

        sequentialGoalStructure.UpdateState(beliefSet);

        // Act
        sequentialGoalStructure.UpdateState(beliefSet);

        // Assert
        sequentialGoalStructure.State.Should().Be(CompletionStatus.Success);
        goalStructure1.Verify(x => x.UpdateState(It.IsAny<BeliefSet>()), Times.Once);
    }

    [Fact]
    public void SequentialGoalStructure_WhenFirstGoalIsFinished_ShouldReturnUnfinished()
    {
        // Arrange
        Mock<IGoalStructure<BeliefSet>> goalStructure1 = new();
        goalStructure1.SetupGet(g => g.State).Returns(CompletionStatus.Success);
        Mock<IGoalStructure<BeliefSet>> goalStructure2 = new();
        BeliefSet beliefSet = Mock.Of<BeliefSet>();
        SequentialGoalStructure<BeliefSet> sequentialGoalStructure = new(new List<IGoalStructure<BeliefSet>>
        {
            goalStructure1.Object, goalStructure2.Object
        });

        // Act
        sequentialGoalStructure.UpdateState(beliefSet);

        // Assert
        sequentialGoalStructure.State.Should().Be(CompletionStatus.Unfinished);
    }

    [Fact]
    public void SequentialGoalStructure_WhenGoalFails_ShouldReturnFailure()
    {
        // Arrange
        Mock<IGoalStructure<BeliefSet>> goalStructure1 = new();
        goalStructure1.SetupGet(g => g.State).Returns(CompletionStatus.Failure);
        Mock<IGoalStructure<BeliefSet>> goalStructure2 = new();
        BeliefSet beliefSet = Mock.Of<BeliefSet>();
        SequentialGoalStructure<BeliefSet> sequentialGoalStructure = new(new List<IGoalStructure<BeliefSet>>
        {
            goalStructure1.Object, goalStructure2.Object
        });

        // Act
        sequentialGoalStructure.UpdateState(beliefSet);

        // Assert
        sequentialGoalStructure.State.Should().Be(CompletionStatus.Failure);
    }

    [Fact]
    public void SequentialGoalStructure_WhenInterruptedWithChanges_ShouldReturnSameGoal()
    {
        // Arrange
        Mock<IGoalStructure<BeliefSet>> goalStructure1 = new();
        goalStructure1.SetupGet(g => g.State).Returns(CompletionStatus.Success);
        IGoal expected = Mock.Of<IGoal>();
        goalStructure1.Setup(g => g.GetCurrentGoal(It.IsAny<BeliefSet>())).Returns(expected);

        Mock<IGoalStructure<BeliefSet>> goalStructure2 = new();
        goalStructure2.SetupGet(g => g.State).Returns(CompletionStatus.Unfinished);

        BeliefSet beliefSet = Mock.Of<BeliefSet>();
        SequentialGoalStructure<BeliefSet> sequentialGoalStructure = new(new List<IGoalStructure<BeliefSet>>
        {
            goalStructure1.Object, goalStructure2.Object
        });
        sequentialGoalStructure.UpdateState(beliefSet);
        goalStructure1.SetupGet(g => g.State).Returns(CompletionStatus.Unfinished);

        // Act
        sequentialGoalStructure.Interrupt(beliefSet);
        sequentialGoalStructure.Reinstate(beliefSet);
        IGoal currentGoal = sequentialGoalStructure.GetCurrentGoal(beliefSet)!;

        // Assert
        sequentialGoalStructure.State.Should().Be(CompletionStatus.Unfinished);
        currentGoal.Should().Be(expected);
    }

    [Fact]
    public void SequentialGoalStructure_WhenInterruptedWithNoChanges_ShouldReturnExactSameGoal()
    {
        // Arrange
        Mock<IGoalStructure<BeliefSet>> goalStructure1 = new();
        goalStructure1.SetupGet(g => g.State).Returns(CompletionStatus.Success);
        Mock<IGoalStructure<BeliefSet>> goalStructure2 = new();
        goalStructure2.SetupGet(g => g.State).Returns(CompletionStatus.Unfinished);
        IGoal expected = Mock.Of<IGoal>();
        goalStructure2.Setup(g => g.GetCurrentGoal(It.IsAny<BeliefSet>())).Returns(expected);
        BeliefSet beliefSet = Mock.Of<BeliefSet>();
        SequentialGoalStructure<BeliefSet> sequentialGoalStructure = new(new List<IGoalStructure<BeliefSet>>
        {
            goalStructure1.Object, goalStructure2.Object
        });
        sequentialGoalStructure.UpdateState(beliefSet);

        // Act
        sequentialGoalStructure.Interrupt(beliefSet);
        sequentialGoalStructure.Reinstate(beliefSet);
        IGoal currentGoal = sequentialGoalStructure.GetCurrentGoal(beliefSet)!;

        // Assert
        sequentialGoalStructure.State.Should().Be(CompletionStatus.Unfinished);
        currentGoal.Should().Be(expected);
    }

    [Fact]
    public void SequentialGoalStructure_WhenProvidingNoGoalStructure_ShouldThrowException()
    {
        // Arrange 
        List<IGoalStructure<BeliefSet>> goalStructures = new();

        // Act
        Func<SequentialGoalStructure<BeliefSet>> act = () => new SequentialGoalStructure<BeliefSet>(goalStructures);

        // Assert
        act.Should().Throw<ArgumentException>();
    }
}
