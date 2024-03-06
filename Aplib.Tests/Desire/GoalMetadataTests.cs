using Aplib.Core.Desire;
using FluentAssertions;

namespace Aplib.Tests.Desire;

public class GoalMetadataTests
{
    [Fact]
    public void GoalMetadata_WhenConstructed_ContainsCorrectMetaData()
    {
        // Arrange
        const string name = "A program without bugs";
        const string description = "This goal is unreachable \ud83d\ude22";

        // Act
        GoalMetadata goalData = new(name, description);

        // Assert
        goalData.Should().NotBeNull();
        goalData.Name.Should().Be(name);
        goalData.Description.Should().Be(description);
    }
}