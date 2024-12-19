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
        var xmlDescriptions = UnitTestFileAccessor.GetFile(AmnvFilesUnitTest.XML_AMM_Data_2024_12_10);
        var xsdDescriptions = UnitTestFileAccessor.GetFile(AmnvFilesUnitTest.XSD_AMM_Data_2024_12_10);

        //Act
        var res = AnmvFileHandler.ValidateXmlWithXsd(
            xmlFilePath: xmlDescriptions.ToFileInfo(),
            xsdFilePath: xsdDescriptions.ToFileInfo()
        );

        //Assert
        Assert.True(res.Errors.Count == 0, res.PrintErrorsAndWarnings(Environment.NewLine));
        Assert.True(res.Warnings.Count == 0, res.PrintErrorsAndWarnings(Environment.NewLine));
    }
}