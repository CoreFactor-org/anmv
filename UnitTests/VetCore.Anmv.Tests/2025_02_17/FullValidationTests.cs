using System.IO.Compression;
using PRF.Utils.CoreComponents.Extensions;
using VetCore.Anmv.Tests.utils;
using VetCore.Anmv.Utils;

namespace VetCore.Anmv.Tests._2025_02_17;

public class FullValidationTests
{
    [Fact]
    public void Deserialize_description_and_validate_content()
    {
        //Arrange
        var zipFile = UnitTestFileAccessor.GetFile(AmnvFilesUnitTest.XML_AMM_Descriptions_2025_02_17);
        using var zipStream = zipFile.OpenRead();
        using var archive = new ZipArchive(zipStream, ZipArchiveMode.Read);
        var xmlDescription = archive.ReadEntryAsString("amm-vet-fr-v2-d.xml");
        var descriptionDto = AnmvFileHandler.DeserializeDescriptionXmlString(xmlDescription)!;

        var zipFileData = UnitTestFileAccessor.GetFile(AmnvFilesUnitTest.XML_AMM_Data_2025_02_17);
        using var zipDataStream = zipFileData.OpenRead();
        using var archiveData = new ZipArchive(zipDataStream, ZipArchiveMode.Read);
        var xmlData = archiveData.ReadEntryAsString("amm-vet-fr-v2-v.xml");
        var dataDto = AnmvFileHandler.DeserializeDataXmlString(xmlData)!;

        //Act
        var res = dataDto.ValidateDataWithRelatedDescription(descriptionDto, out var errors);

        //Assert
        Assert.True(res, errors.PrintErrors(Environment.NewLine));
        Assert.True(errors.Count == 0, errors.PrintErrors(Environment.NewLine));
    }
}