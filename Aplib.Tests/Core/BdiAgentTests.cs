using Aplib.Core;
using Aplib.Core.Belief;
using Aplib.Core.Desire;
using Aplib.Core.Desire.Goals;
using Aplib.Core.Intent.Tactics;
using Moq;

namespace Aplib.Tests.Core;

public class BdiAgentTests
{
    [Fact]
    public void Update_ShouldUpdateBeliefSet()
    {
        // Arrange
        Mock<BeliefSet> beliefSetMock = new();
        Mock<DesireSet> desireSetMock = new();
        BdiAgent agent = new(beliefSetMock.Object, desireSetMock.Object);

        // Act
        agent.Update();

        // Assert
        beliefSetMock.Verify(b => b.UpdateBeliefs(), Times.Once);
    }

    [Fact]
    public void Update_ShouldGetGoalTacticActionAndExecute()
    {
        // Arrange
        Mock<BeliefSet> beliefSetMock = new();
        Mock<DesireSet> desireSetMock = new();
        BdiAgent agent = new(beliefSetMock.Object, desireSetMock.Object);
        Mock<Goal> goalMock = new();
        Mock<Tactic> tacticMock = new();
        Mock<Aplib.Core.Intent.Actions.Action> actionMock = new();

        _ = desireSetMock.Setup(d => d.GetCurrentGoal()).Returns(goalMock.Object);
        _ = goalMock.Setup(g => g.Tactic).Returns(tacticMock.Object);
        _ = tacticMock.Setup(t => t.GetAction()).Returns(actionMock.Object);

        // Act
        agent.Update();

        // Assert
        tacticMock.Verify(t => t.GetAction(), Times.Once);
        actionMock.Verify(a => a.Execute(), Times.Once);
    }
}
