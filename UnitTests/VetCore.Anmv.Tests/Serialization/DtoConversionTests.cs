using VetCore.Anmv.Json.Description;
using VetCore.Anmv.Xml.Descriptions;

namespace VetCore.Anmv.Tests.Serialization;

public class DtoConversionTests
{
    [Fact]
    public void XmlToJsonConversion_ShouldConvertCorrectly()
    {
        // Arrange
        var xmlDto = new EntryDto
        {
            SourceCode = 123,
            SourceDesc = "Example Description",
        };

        // Act
        var jsonDto = xmlDto.ToJsonTermSaDto();

        // Assert
        Assert.NotNull(jsonDto);
        Assert.Equal(xmlDto.SourceCode, jsonDto.SourceCode);
        Assert.Equal(xmlDto.SourceDesc, jsonDto.SourceDesc);
    }

    [Fact]
    public void JsonToXmlConversion_ShouldConvertCorrectly()
    {
        // Arrange
        var jsonDto = new TermTypProcDtoJson
        {
            SourceCode = 456,
            SourceDesc = "Another Example",
        };

        // Act
        var xmlDto = jsonDto.ToXmlDto();

        // Assert
        Assert.NotNull(xmlDto);
        Assert.Equal(jsonDto.SourceCode, xmlDto.SourceCode);
        Assert.Equal(jsonDto.SourceDesc, xmlDto.SourceDesc);
    }

    [Fact]
    public void XmlToJsonConversion_NullInput_ShouldReturnNull()
    {
        // Arrange
        EntryDto? xmlDto = null;

        // Act
        var jsonDto = xmlDto.ToJsonTermUniteDto();

        // Assert
        Assert.Null(jsonDto);
    }

    [Fact]
    public void JsonToXmlConversion_NullInput_ShouldReturnNull()
    {
        // Arrange
        EntryBase? jsonDto = null;

        // Act
        var xmlDto = jsonDto.ToXmlDto();

        // Assert
        Assert.Null(xmlDto);
    }
}