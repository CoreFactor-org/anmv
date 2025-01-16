using VetCore.Anmv.Tests.utils;
using VetCore.Anmv.Utils;
using VetCore.Anmv.Utils.Xsd;

namespace VetCore.Anmv.Tests._2025_01_14.Data;

public sealed class XsdValidationTests
{
    /// <summary>
    /// Validate the xml Data file with provided xsd
    /// </summary>
    [Fact]
    public void AMNV_DATA_Validate_xml_with_xsd()
    {
        //Arrange
        var xml = UnitTestFileAccessor.GetFile(AmnvFilesUnitTest.XML_AMM_Data_2025_01_14);

        //Act
        var res = AnmvFileHandler.ValidateXml(xmlFilePath: xml.ToFileInfo(), AmnvFilesKey.Data_XSD_AMM);

        //Assert
        Assert.True(res.Errors.Count == 0 && res.Warnings.Count == 0, res.PrintErrorsAndWarnings(Environment.NewLine));
    }
}