using FluentAssertions;

namespace Aplib.Core.Tests;

public class MetadataTests
{
    [Fact]
    public void Metadata_WhenConstructed_ContainsCorrectMetaData()
    {
        // Arrange
        const string name = "A program without bugs";
        const string description = "This goal is unreachable \ud83d\ude22";

        // Act
        Metadata data = new(name, description);

        // Assert
        data.Should().NotBeNull();
        data.Id.Should().NotBeEmpty();
        data.Name.Should().Be(name);
        data.Description.Should().Be(description);
    }

    [Fact]
    public void Metadata_WithoutDescription_HasNoDescription()
    {
        // Arrange
        const string name = "Self-evident";

        // Act
        Metadata data = new(name);

        // Assert
        data.Should().NotBeNull();
        data.Id.Should().NotBeEmpty();
        data.Name.Should().Be(name);
        data.Description.Should().BeNull();
    }

    [Fact]
    public void Metadata_WithoutName_HasNoName()
    {
        // Arrange
        const string description = "An unnamed component";

        // Act
        Metadata data = new(null, description);

        // Assert
        data.Should().NotBeNull();
        data.Id.Should().NotBeEmpty();
        data.Name.Should().BeNull();
        data.Description.Should().Be(description);
    }
}
