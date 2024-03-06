using Aplib.Core;
using FluentAssertions;

namespace Aplib.Tests.Core;

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
        data.Name.Should().Be(name);
        data.Description.Should().BeNull();
    }
}
