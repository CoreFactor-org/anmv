using PRF.Utils.CoreComponents.IO;
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
        var xml = UnitTestFileAccessor.GetFile(AmnvFilesUnitTest.XML_AMM_Descriptions_2024_12_10);
        using var dir = PathAndFileHelper.CreateTempUnitTestDirectory();
        var xsd = dir.Current.GetFile("validation.xsd");
        xsd.WriteAllText(AmnvFilesKey.Descriptions_XSD_AMM.GetXsdContent());

        //Act
        var res = AnmvFileHandler.ValidateXmlWithXsd(
            xmlFilePath: xml.ToFileInfo(),
            xsdFilePath: xsd.ToFileInfo()
        );

        //Assert
        Assert.True(res.Errors.Count == 0 && res.Warnings.Count == 0, res.PrintErrorsAndWarnings(Environment.NewLine));
    }

    /// <summary>
    /// Validate the xml Data file with official xsd header
    /// </summary>
    [Fact]
    public void AMNV_DATA_Validate_xml_with_official_header_xsd()
    {
        //Arrange
        var xml = UnitTestFileAccessor.GetFile(AmnvFilesUnitTest.XML_AMM_Data_header_2024_12_10);
        using var dir = PathAndFileHelper.CreateTempUnitTestDirectory();
        var xsd = dir.Current.GetFile("validation.xsd");
        xsd.WriteAllText(AmnvFilesKey.Data_XSD_AMM.GetXsdContent());

        //Act
        var res = AnmvFileHandler.ValidateXmlWithXsd(
            xmlFilePath: xml.ToFileInfo(),
            xsdFilePath: xsd.ToFileInfo()
        );

        //Assert
        Assert.True(res.Errors.Count == 0 && res.Warnings.Count == 0, res.PrintErrorsAndWarnings(Environment.NewLine));
    }

    /// <summary>
    /// Validate the xml Data file with official xsd short
    /// </summary>
    [Fact]
    public void AMNV_DATA_Validate_xml_with_official_short_xsd()
    {
        //Arrange
        var xml = UnitTestFileAccessor.GetFile(AmnvFilesUnitTest.XML_AMM_Data_short_2024_12_10);
        using var dir = PathAndFileHelper.CreateTempUnitTestDirectory();
        var xsd = dir.Current.GetFile("validation.xsd");
        xsd.WriteAllText(AmnvFilesKey.Data_XSD_AMM.GetXsdContent());

        //Act
        var res = AnmvFileHandler.ValidateXmlWithXsd(
            xmlFilePath: xml.ToFileInfo(),
            xsdFilePath: xsd.ToFileInfo()
        );

        //Assert
        Assert.True(res.Errors.Count == 0 && res.Warnings.Count == 0, res.PrintErrorsAndWarnings(Environment.NewLine));
    }

    /// <summary>
    /// Validate the xml Data file with official xsd
    /// </summary>
    [Fact]
    public void AMNV_DATA_Validate_xml_with_official_xsd()
    {
        //Arrange
        var xml = UnitTestFileAccessor.GetFile(AmnvFilesUnitTest.XML_AMM_Data_2024_12_10);
        using var dir = PathAndFileHelper.CreateTempUnitTestDirectory();
        var xsd = dir.Current.GetFile("validation.xsd");
        xsd.WriteAllText(AmnvFilesKey.Data_XSD_AMM.GetXsdContent());

        //Act
        var res = AnmvFileHandler.ValidateXmlWithXsd(
            xmlFilePath: xml.ToFileInfo(),
            xsdFilePath: xsd.ToFileInfo()
        );

        //Assert
        Assert.True(res.Errors.Count == 0 && res.Warnings.Count == 0, res.PrintErrorsAndWarnings(Environment.NewLine));
    }
}