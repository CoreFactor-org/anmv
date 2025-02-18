using System.IO.Compression;
using PRF.Utils.CoreComponents.Extensions;
using VetCore.Anmv.Tests.utils;
using VetCore.Anmv.Utils;
using VetCore.Anmv.Xml.Data;

namespace VetCore.Anmv.Tests._2025_02_17.Data;

public class JsonValidationTests
{
    [Fact]
    public void Deserialize_data_then_convert_toJson_and_count_values()
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
        var xmlDto = AnmvFileHandler.DeserializeDataFile(xmlFile.ToFileInfo());
        var res = xmlDto.ToJsonDto();

        //Assert
        Assert.NotNull(res);
        Assert.Equal(3098, res.MedicinalProducts.Length);
        Assert.Equal(DateTime.Parse("2025-02-11T09:30:38.0000000"), res.Informations.DateJeuDeDonnees);
        var aggregated = res.MedicinalProducts.Aggregate(
            new
            {
                CompositionCount = 0,
                AtcvetCodeCount = 0,
                ParagraphesRcpCount = 0,
                VoiesAdminCount = 0,
                MdvCodesGtinCount = 0,
                ModeleDestineVenteCount = 0,
            },
            (acc, product) => new
            {
                CompositionCount = acc.CompositionCount + product.Compositions.Length,
                AtcvetCodeCount = acc.AtcvetCodeCount + product.AtcvetCodes.Length,
                ParagraphesRcpCount = acc.ParagraphesRcpCount + product.ParagraphesRcp.Length,
                VoiesAdminCount = acc.VoiesAdminCount + product.VoiesAdmin.Length,
                MdvCodesGtinCount = acc.MdvCodesGtinCount + product.MdvCodesGtin.Length,
                ModeleDestineVenteCount = acc.ModeleDestineVenteCount + product.ModeleDestineVente.Length,
            });

        // count aggregated total
        Assert.Equal(4579, aggregated.CompositionCount);
        Assert.Equal(3138, aggregated.AtcvetCodeCount);
        Assert.Equal(87492, aggregated.ParagraphesRcpCount);
        Assert.Equal(9499, aggregated.VoiesAdminCount);
        Assert.Equal(14922, aggregated.MdvCodesGtinCount);
        Assert.Equal(14922, aggregated.ModeleDestineVenteCount);
    }

    [Fact]
    public void Deserialize_data_then_convert_toJson_and_validate()
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
        var xmlDto = AnmvFileHandler.DeserializeDataFile(xmlFile.ToFileInfo());
        var jsonDto = xmlDto.ToJsonDto();
        var res = jsonDto.IsDtoValidAccordingToAttributes();

        //Assert
        Assert.True(res);
    }

    [Fact]
    public void GetMax_VoieAdministration_QteTa_max_lengh()
    {
        //Arrange
        var zipFile = UnitTestFileAccessor.GetFile(AmnvFilesUnitTest.XML_AMM_Data_2025_02_17);
        using var zipStream = zipFile.OpenRead();
        using var archive = new ZipArchive(zipStream, ZipArchiveMode.Read);
        var xmlContent = archive.ReadEntryAsString("amm-vet-fr-v2-v.xml");

        //Act
        var res = AnmvFileHandler.DeserializeDataXmlString(xmlContent).ToJsonDto();

        //Assert
        Assert.NotNull(res);
        Assert.Equal(3098, res.MedicinalProducts.Length);
        var allVoiesAdmin = res.MedicinalProducts.SelectMany(o => o.VoiesAdmin).ToArray();

        // count aggregated total
        Assert.Equal(9499, allVoiesAdmin.Length);
        var maxLenght = allVoiesAdmin.Select(o => o.QteTa?.Length ?? 0).Max();
        Assert.Equal(5, maxLenght);
    }

    [Fact]
    public void Count_distinct_excipients()
    {
        //Arrange
        var zipFile = UnitTestFileAccessor.GetFile(AmnvFilesUnitTest.XML_AMM_Data_2025_02_17);
        using var zipStream = zipFile.OpenRead();
        using var archive = new ZipArchive(zipStream, ZipArchiveMode.Read);
        var xmlContent = archive.ReadEntryAsString("amm-vet-fr-v2-v.xml");

        //Act
        var res = AnmvFileHandler.DeserializeDataXmlString(xmlContent).ToJsonDto();

        //Assert
        Assert.NotNull(res);
        Assert.Equal(3098, res.MedicinalProducts.Length);
        var excipientDistinct = res.MedicinalProducts
            .Select(o => o.ExcipientQsp?.TermUnite).ToHashSet();

        // count aggregated total
    }
}