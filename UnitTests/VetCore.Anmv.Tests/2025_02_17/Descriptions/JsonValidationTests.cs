using System.IO.Compression;
using PRF.Utils.CoreComponents.Extensions;
using VetCore.Anmv.Tests.utils;
using VetCore.Anmv.Utils;
using VetCore.Anmv.Xml.Descriptions;

namespace VetCore.Anmv.Tests._2025_02_17.Descriptions;

public class JsonValidationTests
{
    [Fact]
    public void Deserialize_description_then_convert_toJson_and_count_values()
    {
        //Arrange
        var zipFile = UnitTestFileAccessor.GetFile(AmnvFilesUnitTest.XML_AMM_Descriptions_2025_02_17);
        using var zipStream = zipFile.OpenRead();
        using var archive = new ZipArchive(zipStream, ZipArchiveMode.Read);
        var xmlContent = archive.ReadEntryAsString("amm-vet-fr-v2-d.xml");

        // save it in a xml file
        using var dir = PathAndFileHelper.CreateTempUnitTestDirectory();
        var xmlFile = dir.Current.GetFile("amm-vet-fr-v2-d.xml");
        xmlFile.WriteAllText(xmlContent);

        //Act
        var xmlDto = AnmvFileHandler.DeserializeDescriptionFile(xmlFile.ToFileInfo());
        var res = xmlDto.ToJsonDto();

        //Assert
        Assert.NotNull(res);
        Assert.Equal(3, res.TermNat.Length); // Natures de médicaments
        Assert.Equal(649, res.TermTit.Length); // Titulaires d'AMM
        Assert.Equal(4, res.TermTypProc.Length); // Types de procédure
        Assert.Equal(17, res.TermStatAuto.Length); // Statuts d'autorisation
        Assert.Equal(285, res.TermFp.Length); // Formes pharmaceutiques
        Assert.Equal(2328, res.TermEsp.Length); // Espèces de destination
        Assert.Equal(3638, res.TermSa.Length); // Substances actives
        Assert.Equal(48, res.TermVa.Length); // Voies d'administration
        Assert.Equal(21, res.TermCd.Length); // Conditions de délivrance
        Assert.Equal(14, res.TermDenr.Length); // Denrées
        Assert.Equal(32, res.TermPres.Length); // Présentations
        Assert.Equal(67, res.TermUnite.Length); // Unités
        Assert.Equal(133, res.TermTitre.Length); // Titres paragraphes RCP
    }

    [Fact]
    public void Deserialize_description_then_convert_toJson_and_validate()
    {
        //Arrange
        var zipFile = UnitTestFileAccessor.GetFile(AmnvFilesUnitTest.XML_AMM_Descriptions_2025_02_17);
        using var zipStream = zipFile.OpenRead();
        using var archive = new ZipArchive(zipStream, ZipArchiveMode.Read);
        var xmlContent = archive.ReadEntryAsString("amm-vet-fr-v2-d.xml");

        // save it in a xml file
        using var dir = PathAndFileHelper.CreateTempUnitTestDirectory();
        var xmlFile = dir.Current.GetFile("amm-vet-fr-v2-d.xml");
        xmlFile.WriteAllText(xmlContent);

        //Act
        var xmlDto = AnmvFileHandler.DeserializeDescriptionFile(xmlFile.ToFileInfo());
        var jsonDto = xmlDto.ToJsonDto();
        var res = jsonDto.IsDtoValidAccordingToAttributes();

        //Assert
        Assert.True(res);
    }
}