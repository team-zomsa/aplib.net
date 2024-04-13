using Aplib.Core;
using Moq;
using System.Collections;

namespace Aplib.Integrations.Unity.Tests;

public class UnitTest1
{
    [Fact]
    public void Test_ShouldCallAgentUpdate_WhenAgentStatusIsUnfinished()
    {
        // Arrange
        Mock<IAgent> mockAgent = new();
        mockAgent.SetupSequence(agent => agent.Status)
            .Returns(CompletionStatus.Unfinished)
            .Returns(CompletionStatus.Success);
        mockAgent.Setup(agent => agent.Update());

        AplibRunner aplibRunner = new(mockAgent.Object);

        // Act
        IEnumerator enumerator = aplibRunner.Test();
        while (enumerator.MoveNext())
        {
            // Do nothing
        }

        // Assert
        mockAgent.Verify(agent => agent.Update(), Times.Once);
    }
}
