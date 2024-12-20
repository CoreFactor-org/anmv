using VetCore.Anmv.Tests.data;
using VetCore.Anmv.Utils;

namespace VetCore.Anmv.Tests._2024_12_10.Data;

public sealed class XsdValidationTests
{
    /// <summary>
    /// Validate the xml Data file with provided xsd
    /// </summary>
    [Fact]
    public void AMNV_DATA_Validate_xml_with_xsd()
    {
        //Arrange
        var xml = UnitTestFileAccessor.GetFile(AmnvFilesUnitTest.XML_AMM_Data_2024_12_10);
        var xsd = UnitTestFileAccessor.GetFile(AmnvFilesUnitTest.XSD_AMM_Data_2024_12_10);

        //Act
        var res = AnmvFileHandler.ValidateXmlWithXsd(xmlFilePath: xml.ToFileInfo(), xsdFilePath: xsd.ToFileInfo());

        //Assert
        Assert.True(res.Errors.Count == 0 && res.Warnings.Count == 0, res.PrintErrorsAndWarnings(Environment.NewLine));
    }
}