using VetCore.Anmv.Tests.utils;
using VetCore.Anmv.Utils;

namespace VetCore.Anmv.Tests._2026_06;

public class FullValidationTests
{
    [Fact]
    public void Deserialize_description_and_validate_content()
    {
        // Arrange
        var descriptionDto = AnmvFileHandler.DeserializeDescriptionXmlString(
            UnitTestFileAccessor.GetXmlContent(AmnvFilesUnitTest.XML_AMM_Descriptions_2026_06))!;
        var dataDto = AnmvFileHandler.DeserializeDataXmlString(
            UnitTestFileAccessor.GetXmlContent(AmnvFilesUnitTest.XML_AMM_Data_2026_06))!;

        // Act
        var isValid = dataDto.ValidateDataWithRelatedDescription(descriptionDto, out var errors);

        // Assert
        Assert.True(isValid, errors.PrintErrors(Environment.NewLine));
        Assert.Equal(0, errors.Count);
    }
}
