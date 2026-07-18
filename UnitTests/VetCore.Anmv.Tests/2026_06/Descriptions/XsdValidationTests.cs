using VetCore.Anmv.Tests.utils;
using VetCore.Anmv.Utils;
using VetCore.Anmv.Utils.Xsd;

namespace VetCore.Anmv.Tests._2026_06.Descriptions;

public sealed class XsdValidationTests
{
    [Fact]
    public void AMNV_DESCRIPTIONS_Validate_xml_content_with_xsd()
    {
        // Arrange
        var xmlContent = UnitTestFileAccessor.GetXmlContent(AmnvFilesUnitTest.XML_AMM_Descriptions_2026_06);

        // Act
        var result = AnmvFileHandler.ValidateXml(xmlContent, AmnvFilesKey.Descriptions_XSD_AMM);

        // Assert
        Assert.Empty(result.Errors);
        Assert.Empty(result.Warnings);
    }

    [Fact]
    public void AMNV_DESCRIPTIONS_Validate_xml_content_with_xsd_variant()
    {
        // Arrange
        var xmlContent = UnitTestFileAccessor.GetXmlContent(AmnvFilesUnitTest.XML_AMM_Descriptions_2026_06);
        var xsdContent = AmnvFilesKey.Descriptions_XSD_AMM.GetXsdContent();

        // Act
        var result = AnmvFileHandler.ValidateXmlWithXsd(xmlContent, xsdContent);

        // Assert
        Assert.Empty(result.Errors);
        Assert.Empty(result.Warnings);
    }
}
