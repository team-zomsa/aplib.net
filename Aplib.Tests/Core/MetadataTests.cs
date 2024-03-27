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

    [Fact]
    public void Metadata_MultipleInstances_HaveUniqueIds()
    {
        // Act
        Metadata data1 = new();
        Metadata data2 = new();

        // Assert
        data1.Should().NotBeNull();
        data1.Id.Should().NotBeEmpty();
        data1.Name.Should().BeNull();
        data1.Description.Should().BeNull();

        data2.Should().NotBeNull();
        data2.Id.Should().NotBeEmpty();
        data2.Name.Should().BeNull();
        data2.Description.Should().BeNull();

        data1.Id.Should().NotBe(data2.Id);
    }

    [Fact]
    public void Metadata_SameInstance_HasSameId()
    {
        // Act
        Metadata data = new();
        // ReSharper disable once InlineTemporaryVariable
        Metadata copy = data;

        // Assert
        data.Should().NotBeNull();
        data.Id.Should().NotBeEmpty();
        data.Name.Should().BeNull();
        data.Description.Should().BeNull();

        copy.Should().NotBeNull();
        copy.Id.Should().NotBeEmpty();
        copy.Name.Should().BeNull();
        copy.Description.Should().BeNull();

        data.Id.Should().Be(copy.Id);
    }
}
