using VetCore.Anmv.Tests.utils;
using VetCore.Anmv.Utils;
using VetCore.Anmv.Utils.Xsd;

namespace VetCore.Anmv.Tests._2026_06.Data;

public sealed class XsdValidationTests
{
    [Fact]
    public void AMNV_DATA_Validate_xml_content_with_xsd_and_detect_invalid_guid()
    {
        // Arrange
        var xmlContent = UnitTestFileAccessor.GetXmlContent(AmnvFilesUnitTest.XML_AMM_Data_2026_06);

        // Act
        var result = AnmvFileHandler.ValidateXml(xmlContent, AmnvFilesKey.Data_XSD_AMM);

        // Assert
        Assert.Contains(result.Errors, error => error.Contains("pack-id"));
        Assert.Empty(result.Warnings);
    }

    [Fact]
    public void AMNV_DATA_Validate_xml_file_with_xsd_and_detect_invalid_guid()
    {
        // Arrange
        var xmlContent = UnitTestFileAccessor.GetXmlContent(AmnvFilesUnitTest.XML_AMM_Data_2026_06);
        using var directory = PathAndFileHelper.CreateTempUnitTestDirectory();
        var xmlFile = directory.Current.GetFile("amm-vet-fr-v2-v.xml");
        xmlFile.WriteAllText(xmlContent);

        // Act
        var result = AnmvFileHandler.ValidateXml(xmlFile.ToFileInfo(), AmnvFilesKey.Data_XSD_AMM);

        // Assert
        Assert.Contains(result.Errors, error => error.Contains("pack-id"));
        Assert.Empty(result.Warnings);
    }
}
