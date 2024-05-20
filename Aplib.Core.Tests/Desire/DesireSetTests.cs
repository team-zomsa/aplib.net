using Aplib.Core.Belief.BeliefSets;
using Aplib.Core.Desire.DesireSets;
using Aplib.Core.Desire.Goals;
using Aplib.Core.Desire.GoalStructures;
using FluentAssertions;
using Moq;
using System;

namespace Aplib.Core.Tests.Desire;

public class DesireSetTests
{
    /// <summary>
    /// Given a desire set with finished goals,
    /// When the desire set is updated and GetCurrentGoal method is called,
    /// Then an invalid operation exception is thrown.
    /// </summary>
    [Theory]
    [InlineData(CompletionStatus.Success)]
    [InlineData(CompletionStatus.Failure)]
    public void GetCurrentGoal_WhenDesireSetIsFinished_ShouldThrowInvalidOperationException(
        CompletionStatus finishedMainGoalStatus)
    {
        // Arrange
        Mock<IGoalStructure<IBeliefSet>> mainGoalStructure = new();
        mainGoalStructure.Setup(g => g.Status).Returns(finishedMainGoalStatus);

        DesireSet<IBeliefSet> desireSet = new(mainGoalStructure.Object);

        // Act
        desireSet.Update(It.IsAny<IBeliefSet>());
        Action getCurrentGoal = () => desireSet.GetCurrentGoal(It.IsAny<IBeliefSet>());

        // Assert
        getCurrentGoal.Should().Throw<InvalidOperationException>();
    }

    /// <summary>
    /// Given a desire set with only a main goal structure and no activatables,
    /// When the GetCurrentGoal method is called,
    /// Then the current goal of the main goal structure should be returned.
    /// </summary>
    [Fact]
    public void GetCurrentGoal_WhenOnlyMainGoal_ReturnsMainGoal()
    {
        // Arrange
        IGoal<IBeliefSet> goal = Mock.Of<IGoal<IBeliefSet>>();

        Mock<IGoalStructure<IBeliefSet>> mainGoalStructure = new();
        mainGoalStructure.Setup(g => g.GetCurrentGoal(It.IsAny<IBeliefSet>())).Returns(goal);

        DesireSet<IBeliefSet> desireSet = new(mainGoalStructure.Object);

        // Act
        IGoal<IBeliefSet> currentGoal = desireSet.GetCurrentGoal(It.IsAny<IBeliefSet>());

        // Assert
        currentGoal.Should().Be(goal);
    }

    /// <summary>
    /// Given a desire set with some activated side goal structure that is unfinished,
    /// When the desire set is updated and GetCurrentGoal method is called,
    /// Then the current goal of the side goal structure should be returned.
    /// </summary>
    [Fact]
    public void GetCurrentGoal_WhenUnfinishedSideGoalIsActivated_ReturnsSideGoal()
    {
        // Arrange
        IGoal<IBeliefSet> goal = Mock.Of<IGoal<IBeliefSet>>();

        Mock<IGoalStructure<IBeliefSet>> mainGoalStructure = new();
        mainGoalStructure.Setup(g => g.Status).Returns(CompletionStatus.Unfinished);

        Mock<IGoalStructure<IBeliefSet>> sideGoalStructure = new();
        sideGoalStructure.Setup(g => g.GetCurrentGoal(It.IsAny<IBeliefSet>())).Returns(goal);
        sideGoalStructure.Setup(g => g.Status).Returns(CompletionStatus.Unfinished);

        DesireSet<IBeliefSet> desireSet = new(mainGoalStructure.Object, (sideGoalStructure.Object, _ => true));

        // Act
        desireSet.Update(It.IsAny<IBeliefSet>());
        IGoal<IBeliefSet> currentGoal = desireSet.GetCurrentGoal(It.IsAny<IBeliefSet>());

        // Assert
        currentGoal.Should().Be(goal);
    }

    /// <summary>
    /// Given a desire set with an unfinished main goal
    /// and some activated side goal structures that are unfinished,
    /// When the desire set is updated and GetCurrentGoal method is called,
    /// Then the current goal of the main goal structure should be returned.
    /// </summary>
    [Fact]
    public void GetCurrentGoal_WhenUnfinishedSideGoalIsNotActivated_ReturnsMainGoal()
    {
        // Arrange
        IGoal<IBeliefSet> goal = Mock.Of<IGoal<IBeliefSet>>();

        Mock<IGoalStructure<IBeliefSet>> mainGoalStructure = new();
        mainGoalStructure.Setup(g => g.GetCurrentGoal(It.IsAny<IBeliefSet>())).Returns(goal);
        mainGoalStructure.Setup(g => g.Status).Returns(CompletionStatus.Unfinished);

        Mock<IGoalStructure<IBeliefSet>> sideGoalStructure = new();
        sideGoalStructure.Setup(g => g.Status).Returns(CompletionStatus.Unfinished);

        DesireSet<IBeliefSet> desireSet = new(mainGoalStructure.Object, (sideGoalStructure.Object, _ => false));

        // Act
        desireSet.Update(It.IsAny<IBeliefSet>());
        IGoal<IBeliefSet> currentGoal = desireSet.GetCurrentGoal(It.IsAny<IBeliefSet>());

        // Assert
        currentGoal.Should().Be(goal);
    }

    /// <summary>
    /// Given a desire set with some activated side goal structures that are unfinished,
    /// When the desire set is updated,
    /// Then the status should be unfinished.
    /// </summary>
    [Theory]
    [InlineData(CompletionStatus.Success)]
    [InlineData(CompletionStatus.Failure)]
    [InlineData(CompletionStatus.Unfinished)]
    public void Update_WhenActivatedSideGoalUnfinished_StatusShouldBeUnfinished(CompletionStatus mainGoalStatus)
    {
        // Arrange
        Mock<IGoalStructure<IBeliefSet>> mainGoalStructure = new();
        mainGoalStructure.Setup(g => g.Status).Returns(mainGoalStatus);

        Mock<IGoalStructure<IBeliefSet>> sideGoalStructure = new();
        sideGoalStructure.Setup(g => g.Status).Returns(CompletionStatus.Unfinished);

        DesireSet<IBeliefSet> desireSet = new(mainGoalStructure.Object, (sideGoalStructure.Object, _ => true));

        // Act
        desireSet.Update(It.IsAny<IBeliefSet>());
        CompletionStatus status = desireSet.Status;

        // Assert
        status.Should().Be(CompletionStatus.Unfinished);
    }

    /// <summary>
    /// Given a desire set with only a main goal structure and no activatables,
    /// When the desire set is updated,
    /// Then the status of the main goal structure should be updated.
    /// </summary>
    [Fact]
    public void Update_WhenOnlyMainGoal_ShouldUpdateMainGoalStructureStatus()
    {
        // Arrange
        Mock<IGoalStructure<IBeliefSet>> mainGoalStructure = new();
        DesireSet<IBeliefSet> desireSet = new(mainGoalStructure.Object);

        // Act
        desireSet.Update(It.IsAny<IBeliefSet>());

        // Assert
        mainGoalStructure.Verify(g => g.UpdateStatus(It.IsAny<IBeliefSet>()), Times.Once());
    }

    /// <summary>
    /// Given a desire set with only a main goal,
    /// When the desire set is updated,
    /// Then the status should be the same as the main goal structure status.
    /// </summary>
    [Theory]
    [InlineData(CompletionStatus.Success)]
    [InlineData(CompletionStatus.Failure)]
    [InlineData(CompletionStatus.Unfinished)]
    public void Update_WhenOnlyMainGoal_StatusShouldBeSameAsMainGoal(CompletionStatus mainGoalStatus)
    {
        // Arrange
        Mock<IGoalStructure<IBeliefSet>> mainGoalStructure = new();
        mainGoalStructure.Setup(g => g.Status).Returns(mainGoalStatus);
        DesireSet<IBeliefSet> desireSet = new(mainGoalStructure.Object);

        // Act
        desireSet.Update(It.IsAny<IBeliefSet>());
        CompletionStatus status = desireSet.Status;

        // Assert
        status.Should().Be(mainGoalStatus);
    }
}
