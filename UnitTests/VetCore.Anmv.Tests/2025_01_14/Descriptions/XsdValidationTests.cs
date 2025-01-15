﻿using VetCore.Anmv.Utils;
using VetCore.Anmv.Utils.Xsd;
using VetCore.Anmv.Xml.Descriptions;

namespace VetCore.Anmv.Tests._2025_01_14.Descriptions;

public sealed class XsdValidationTests
{
    /// <summary>
    /// Validate the xml Descriptions file with provided xsd
    /// </summary>
    [Fact]
    public void AMNV_DESCRIPTIONS_Validate_xml_with_xsd()
    {
        //Arrange
        var xmlDescriptions = UnitTestFileAccessor.GetFile(AmnvFilesUnitTest.XML_AMM_Descriptions_2025_01_14);

        //Act
        var res = AnmvFileHandler.ValidateXml(
            xmlFilePath: xmlDescriptions.ToFileInfo(),
            AmnvFilesKey.Descriptions_XSD_AMM
        );

        //Assert
        Assert.True(res.Errors.Count == 0 && res.Warnings.Count == 0, res.PrintErrorsAndWarnings(Environment.NewLine));
    }

    /// <summary>
    /// Validate the xml Descriptions file with provided xsd
    /// </summary>
    [Fact]
    public void AMNV_DESCRIPTIONS_Validate_xml_with_xsd_variant()
    {
        //Arrange
        var xsdFile = AmnvFilesKey.Descriptions_XSD_AMM.GetXsdContent();
        var xmlDescriptions = UnitTestFileAccessor.GetFile(AmnvFilesUnitTest.XML_AMM_Descriptions_2025_01_14);

        //Act
        var res = AnmvFileHandler.ValidateXmlWithXsd(
            xmlFilePath: xmlDescriptions.ToFileInfo(),
            xsdFileContent: xsdFile
        );

        //Assert
        Assert.True(res.Errors.Count == 0 && res.Warnings.Count == 0, res.PrintErrorsAndWarnings(Environment.NewLine));
    }
}