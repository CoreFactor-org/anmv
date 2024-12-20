using VetCore.Anmv.Tests.data;
using VetCore.Anmv.Utils;
using VetCore.Anmv.Utils.Xsd;

namespace VetCore.Anmv.Tests._2024_12_10;

public sealed class OfficialValidationTests
{
    /// <summary>
    /// Validate the xml Descriptions file with official xsd
    /// </summary>
    [Fact]
    public void AMNV_DESCRIPTIONS_Validate_xml_with_official_xsd()
    {
        //Arrange
        var xmlDescriptions = UnitTestFileAccessor.GetFile(AmnvFilesUnitTest.XML_AMM_Descriptions_2024_12_10);
        var xsdDescriptions = AmnvFilesKey.Descriptions_XSD_AMM.GetFile();

        //Act
        var res = AnmvFileHandler.ValidateXmlWithXsd(
            xmlFilePath: xmlDescriptions.ToFileInfo(),
            xsdFilePath: xsdDescriptions
        );

        //Assert
        Assert.True(res.Errors.Count == 0, res.PrintErrorsAndWarnings(Environment.NewLine));
        Assert.True(res.Warnings.Count == 0, res.PrintErrorsAndWarnings(Environment.NewLine));
    }

}