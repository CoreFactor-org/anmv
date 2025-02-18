using VetCore.Anmv.Json.Description;
using VetCore.Anmv.Xml.Descriptions;

namespace VetCore.Anmv.Tests.Serialization;

public class EntryOrdreDtoConversionTests
{
    [Fact]
    public void XmlToJsonConversion_ShouldConvertCorrectly()
    {
        // Arrange
        var xmlDto = new EntryOrdreDto
        {
            SourceCode = 789,
            SourceDesc = "Derived Description",
            Ordre = 42,
        };

        // Act
        var jsonDto = xmlDto.ToJsonTermTitreDto();

        // Assert
        Assert.NotNull(jsonDto);
        Assert.Equal(xmlDto.SourceCode, jsonDto.SourceCode);
        Assert.Equal(xmlDto.SourceDesc, jsonDto.SourceDesc);
        Assert.Equal(xmlDto.Ordre, jsonDto.Ordre);
    }

    [Fact]
    public void JsonToXmlConversion_ShouldConvertCorrectly()
    {
        // Arrange
        var jsonDto = new TermTitreDtoJson
        {
            SourceCode = 101,
            SourceDesc = "Another Derived Description",
            Ordre = 99,
        };

        // Act
        var xmlDto = jsonDto.ToXmlDto();

        // Assert
        Assert.NotNull(xmlDto);
        Assert.Equal(jsonDto.SourceCode, xmlDto.SourceCode);
        Assert.Equal(jsonDto.SourceDesc, xmlDto.SourceDesc);
        Assert.Equal(jsonDto.Ordre, xmlDto.Ordre);
    }

    [Fact]
    public void XmlToJsonConversion_NullInput_ShouldReturnNull()
    {
        // Arrange
        EntryOrdreDto? xmlDto = null;

        // Act
        var jsonDto = xmlDto.ToJsonTermCdDto();

        // Assert
        Assert.Null(jsonDto);
    }

    [Fact]
    public void JsonToXmlConversion_NullInput_ShouldReturnNull()
    {
        // Arrange
        TermDenrDtoJson? jsonDto = null;

        // Act
        var xmlDto = jsonDto.ToXmlDto();

        // Assert
        Assert.Null(xmlDto);
    }
}