using System.IO.Compression;
using PRF.Utils.CoreComponents.Extensions;
using VetCore.Anmv.Tests.utils;
using VetCore.Anmv.Utils;
using VetCore.Anmv.Utils.Xsd;

namespace VetCore.Anmv.Tests._2025_02_17.Data;

public sealed class XsdValidationTests
{
    /// <summary>
    /// Validate the xml Data file content with provided xsd
    /// </summary>
    [Fact]
    public void AMNV_DATA_Validate_xml_file_content_with_xsd()
    {
        //Arrange
        var zipFile = UnitTestFileAccessor.GetFile(AmnvFilesUnitTest.XML_AMM_Data_2025_02_17);

        using var zipStream = zipFile.OpenRead();
        using var archive = new ZipArchive(zipStream, ZipArchiveMode.Read);
        var xmlContent = archive.ReadEntryAsString("amm-vet-fr-v2-v.xml");

        //Act
        var res = AnmvFileHandler.ValidateXml(xmlContent, AmnvFilesKey.Data_XSD_AMM);

        //Assert
        Assert.True(res.Errors.Count == 0 && res.Warnings.Count == 0, res.PrintErrorsAndWarnings(Environment.NewLine));
    }

    /// <summary>
    /// Validate the xml Data file with provided xsd
    /// </summary>
    [Fact]
    public void AMNV_DATA_Validate_xml_with_xsd()
    {
        //Arrange
        var zipFile = UnitTestFileAccessor.GetFile(AmnvFilesUnitTest.XML_AMM_Data_2025_02_17);
        using var zipStream = zipFile.OpenRead();
        using var archive = new ZipArchive(zipStream, ZipArchiveMode.Read);
        var xmlContent = archive.ReadEntryAsString("amm-vet-fr-v2-v.xml");

        // save it in a xml file
        using var dir = PathAndFileHelper.CreateTempUnitTestDirectory();
        var xmlFile = dir.Current.GetFile("amm-vet-fr-v2-v.xml");
        xmlFile.WriteAllText(xmlContent);

        //Act
        var res = AnmvFileHandler.ValidateXml(xmlFilePath: xmlFile.ToFileInfo(), AmnvFilesKey.Data_XSD_AMM);

        //Assert
        Assert.True(res.Errors.Count == 0 && res.Warnings.Count == 0, res.PrintErrorsAndWarnings(Environment.NewLine));
    }
}