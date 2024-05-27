using Aplib.Core.Agents;
using Aplib.Core.Belief.BeliefSets;
using Aplib.Core.Desire.DesireSets;
using Aplib.Core.Desire.Goals;
using Aplib.Core.Intent.Actions;
using Aplib.Core.Intent.Tactics;
using FluentAssertions;
using Moq;

namespace Aplib.Core.Tests;

public class BdiAgentTests
{
    [Theory]
    [InlineData(CompletionStatus.Success)]
    [InlineData(CompletionStatus.Failure)]
    [InlineData(CompletionStatus.Unfinished)]
    public void Agent_WhenStatusIsChecked_ShouldBeSameAsDesireSet(CompletionStatus desireSetStatus)
    {
        // Arrange
        Mock<IDesireSet<IBeliefSet>> desireSet = new();
        desireSet.Setup(d => d.Status).Returns(desireSetStatus);
        Mock<BdiAgent<IBeliefSet>> agent = new(It.IsAny<IBeliefSet>(), desireSet.Object);

        // Act
        CompletionStatus agentStatus = agent.Object.Status;

        // Assert
        agentStatus.Should().Be(desireSetStatus);
    }

    [Theory]
    [InlineData(CompletionStatus.Failure)]
    [InlineData(CompletionStatus.Success)]
    public void Update_WhenFinished_ShouldNotUpdateBeliefSet(CompletionStatus completionStatus)
    {
        // Arrange
        Mock<IBeliefSet> beliefSetMock = new();
        beliefSetMock.Setup(b => b.UpdateBeliefs());
        Mock<IDesireSet<IBeliefSet>> desireSetMock = new();
        desireSetMock.Setup(d => d.Status).Returns(completionStatus);

        // Mock the desire set to return a goal
        IAction<IBeliefSet> action = Mock.Of<IAction<IBeliefSet>>();
        Mock<ITactic<IBeliefSet>> tacticMock = new();
        tacticMock.Setup(t => t.GetAction(It.IsAny<IBeliefSet>())).Returns(action);

        Mock<IGoal<IBeliefSet>> goalMock = new();
        goalMock.Setup(g => g.Tactic).Returns(tacticMock.Object);

        desireSetMock.Setup(d => d.GetCurrentGoal(It.IsAny<IBeliefSet>()))
            .Returns(goalMock.Object);

        // Create the agent
        BdiAgent<IBeliefSet> agent = new(beliefSetMock.Object, desireSetMock.Object);

        // Act
        agent.Update();

        // Assert
        desireSetMock.Verify(b => b.GetCurrentGoal(It.IsAny<IBeliefSet>()), Times.Never);
    }

    [Fact]
    public void Update_WhenNotFinished_ShouldExecuteAction()
    {
        // Arrange
        Mock<IBeliefSet> beliefSetMock = new();
        beliefSetMock.Setup(b => b.UpdateBeliefs());
        Mock<IDesireSet<IBeliefSet>> desireSetMock = new();
        desireSetMock.Setup(d => d.Status).Returns(CompletionStatus.Unfinished);

        // Mock the desire set to return a goal
        Mock<IAction<IBeliefSet>> action = new();

        Mock<ITactic<IBeliefSet>> tacticMock = new();
        tacticMock.Setup(t => t.GetAction(It.IsAny<IBeliefSet>())).Returns(action.Object);

        Mock<IGoal<IBeliefSet>> goalMock = new();
        goalMock.Setup(g => g.Tactic).Returns(tacticMock.Object);

        desireSetMock.Setup(d => d.GetCurrentGoal(It.IsAny<IBeliefSet>()))
            .Returns(goalMock.Object);

        // Create the agent
        BdiAgent<IBeliefSet> agent = new(beliefSetMock.Object, desireSetMock.Object);

        // Act
        agent.Update();

        // Assert
        action.Verify(b => b.Execute(It.IsAny<IBeliefSet>()), Times.Once);
    }

    [Fact]
    public void Update_WhenNotFinished_ShouldUpdateBeliefSet()
    {
        // Arrange
        Mock<IBeliefSet> beliefSetMock = new();
        beliefSetMock.Setup(b => b.UpdateBeliefs());
        Mock<IDesireSet<IBeliefSet>> desireSetMock = new();
        desireSetMock.Setup(d => d.Status).Returns(CompletionStatus.Unfinished);

        // Mock the desire set to return a goal
        IAction<IBeliefSet> action = Mock.Of<IAction<IBeliefSet>>();

        Mock<ITactic<IBeliefSet>> tacticMock = new();
        tacticMock.Setup(t => t.GetAction(It.IsAny<IBeliefSet>())).Returns(action);

        Mock<IGoal<IBeliefSet>> goalMock = new();
        goalMock.Setup(g => g.Tactic).Returns(tacticMock.Object);

        desireSetMock.Setup(d => d.GetCurrentGoal(It.IsAny<IBeliefSet>()))
            .Returns(goalMock.Object);

        // Create the agent
        BdiAgent<IBeliefSet> agent = new(beliefSetMock.Object, desireSetMock.Object);

        // Act
        agent.Update();

        // Assert
        desireSetMock.Verify(b => b.GetCurrentGoal(It.IsAny<IBeliefSet>()), Times.Once);
    }
}
