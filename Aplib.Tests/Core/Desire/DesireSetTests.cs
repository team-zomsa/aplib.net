using Aplib.Core;
using Aplib.Core.Belief;
using Aplib.Core.Desire;
using Aplib.Core.Desire.Goals;
using FluentAssertions;
using Moq;

namespace Aplib.Core.Tests.Core.Desire;

public class DesireSetTests
{
    /// <summary>
    /// Given a desire set,
    /// When the GetCurrentGoal method is called,
    /// Then the desire set should return the current goal of the current goal structure.
    /// </summary>
    [Fact]
    public void DesireSet_WhenGetCurrentGoalIsCalled_ReturnsCurrentGoal()
    {
        // Arrange
        IBeliefSet beliefSet = Mock.Of<IBeliefSet>();
        IGoal goal = Mock.Of<IGoal>();
        Mock<IGoalStructure<IBeliefSet>> goalStructure = new();
        goalStructure
            .Setup(g => g.GetCurrentGoal(It.IsAny<IBeliefSet>()))
            .Returns(goal);
        Mock<DesireSet<IBeliefSet>> desireSet = new(goalStructure.Object);

        // Act
        IGoal currentGoal = desireSet.Object.GetCurrentGoal(beliefSet);

        // Assert
        currentGoal.Should().Be(goal);
    }

    /// <summary>
    /// Given a desire set,
    /// When the status is updated,
    /// Then the status of the goal structures are updated.
    /// </summary>
    [Fact]
    public void DesireSet_WhenStatusUpdated_ShouldUpdateGoalStructuresStatus()
    {
        // Arrange
        Mock<IGoalStructure<IBeliefSet>> goalStructure = new();
        Mock<DesireSet<IBeliefSet>> desireSet = new(goalStructure.Object);

        // Act
        desireSet.Object.UpdateStatus(It.IsAny<IBeliefSet>());

        // Assert
        goalStructure.Verify(g => g.UpdateStatus(It.IsAny<IBeliefSet>()), Times.Once());
    }

    /// <summary>
    /// Given a desire set,
    /// When the status is checked,
    /// Then the status should be the same as the main goal structure status.
    /// </summary>
    [Fact]
    public void DesireSet_WhenStatusIsChecked_ShouldBeSameAsMainGoal()
    {
        // Arrange
        Mock<IGoalStructure<IBeliefSet>> goalStructure = new();
        goalStructure.Setup(g => g.Status).Returns(CompletionStatus.Success);
        Mock<DesireSet<IBeliefSet>> desireSet = new(goalStructure.Object);

        // Act
        CompletionStatus status = desireSet.Object.Status;
        CompletionStatus expectedStatus = goalStructure.Object.Status;

        // Assert
        status.Should().Be(expectedStatus);
    }
}
