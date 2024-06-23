using Aplib.Core.Belief.BeliefSets;
using Aplib.Extensions.Actions;
using Moq;

namespace Aplib.Extensions.Tests;

public class PathfindingActionTests
{
    /// <summary>
    /// Given a PathfinderAction with a mocked IPathfinder that always finds a path,
    /// When the Execute method is called,
    /// Then the current location should be updated to the next location provided by the pathfinder.
    /// </summary>
    [Fact]
    public void Execute_WithStandardPathfinder_ShouldFindPath()
    {
        // Arrange
        Mock<IPathfinder<int>> pathfinderMock = new();
        int output = 2;
        pathfinderMock.Setup(p => p.TryGetNextStep(It.IsAny<int>(), It.IsAny<int>(), out output)).Returns(true);

        IBeliefSet beliefSet = Mock.Of<IBeliefSet>();

        int currentLocation = 1;

        var action = new PathfinderAction<IBeliefSet, int>(
            pathfinder: pathfinderMock.Object,
            getCurrentLocation: _ => currentLocation,
            getTargetLocation: _ => 3,
            effectWithNextStep: (_, location) =>
            {
                currentLocation = location;
            }
        );

        // Act
        action.Execute(beliefSet);

        // Assert
        Assert.Equal(2, currentLocation);
    }

    /// <summary>
    /// Given a PathfinderAction with a mocked IPathfinder that never finds a path,
    /// When the Execute method is called,
    /// Then the current location should NOT be updated to the next location provided by the pathfinder.
    /// </summary>
    [Fact]
    public void Execute_WithStandardPathfinderCannotFindPath_ShouldNotUpdate()
    {
        // Arrange
        Mock<IPathfinder<int>> pathfinderMock = new();
        int output = 2;
        pathfinderMock.Setup(p => p.TryGetNextStep(It.IsAny<int>(), It.IsAny<int>(), out output)).Returns(false);

        IBeliefSet beliefSet = Mock.Of<IBeliefSet>();

        int currentLocation = 1;

        var action = new PathfinderAction<IBeliefSet, int>(
            pathfinder: pathfinderMock.Object,
            getCurrentLocation: _ => currentLocation,
            getTargetLocation: _ => 3,
            effectWithNextStep: (_, location) =>
            {
                currentLocation = location;
            }
        );

        // Act
        action.Execute(beliefSet);

        // Assert
        Assert.Equal(1, currentLocation);
    }
}
