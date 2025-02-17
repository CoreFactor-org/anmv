using System.IO.Compression;
using PRF.Utils.CoreComponents.Extensions;
using VetCore.Anmv.Tests.utils;
using VetCore.Anmv.Utils;
using VetCore.Anmv.Utils.Xsd;

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
        var zipFile = UnitTestFileAccessor.GetFile(AmnvFilesUnitTest.XML_AMM_Descriptions_2025_01_14);

        using var zipStream = zipFile.OpenRead();
        using var archive = new ZipArchive(zipStream, ZipArchiveMode.Read);
        var xmlDescriptions = archive.ReadEntryAsString("amm-vet-fr-v2-d.xml");

        //Act
        var res = AnmvFileHandler.ValidateXml(xmlDescriptions, AmnvFilesKey.Descriptions_XSD_AMM
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
        var zipFile = UnitTestFileAccessor.GetFile(AmnvFilesUnitTest.XML_AMM_Descriptions_2025_01_14);

        using var zipStream = zipFile.OpenRead();
        using var archive = new ZipArchive(zipStream, ZipArchiveMode.Read);
        var xmlDescriptions = archive.ReadEntryAsString("amm-vet-fr-v2-d.xml");

        //Act
        var res = AnmvFileHandler.ValidateXmlWithXsd(xmlDescriptions, xsdFileContent: xsdFile);

        //Assert
        Assert.True(res.Errors.Count == 0 && res.Warnings.Count == 0, res.PrintErrorsAndWarnings(Environment.NewLine));
    }
}