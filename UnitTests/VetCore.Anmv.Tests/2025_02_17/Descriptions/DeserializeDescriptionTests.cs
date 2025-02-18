using System.IO.Compression;
using PRF.Utils.CoreComponents.Extensions;
using VetCore.Anmv.Tests.utils;
using VetCore.Anmv.Utils;
using VetCore.Anmv.Utils.Helpers;
using VetCore.Anmv.Xml.Descriptions;

namespace VetCore.Anmv.Tests._2025_02_17.Descriptions;

public sealed class DeserializeDescriptionTests
{
    [Fact]
    public void Deserialize_description_and_count_values()
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
        var res = AnmvFileHandler.DeserializeDescriptionFile(xmlFile.ToFileInfo());

        //Assert
        Assert.NotNull(res);
        Assert.Equal(3, res.TermNat.Count); // Natures de médicaments
        Assert.Equal(649, res.TermTit.Count); // Titulaires d'AMM
        Assert.Equal(4, res.TermTypProc.Count); // Types de procédure
        Assert.Equal(17, res.TermStatAuto.Count); // Statuts d'autorisation
        Assert.Equal(285, res.TermFp.Count); // Formes pharmaceutiques
        Assert.Equal(2328, res.TermEsp.Count); // Espèces de destination
        Assert.Equal(3638, res.TermSa.Count); // Substances actives
        Assert.Equal(48, res.TermVa.Count); // Voies d'administration
        Assert.Equal(21, res.TermCd.Count); // Conditions de délivrance
        Assert.Equal(14, res.TermDenr.Count); // Denrées
        Assert.Equal(32, res.TermPres.Count); // Présentations
        Assert.Equal(67, res.TermUnite.Count); // Unités
        Assert.Equal(133, res.TermTitre.Count); // Titres paragraphes RCP
    }

    [Fact]
    public void Deserialize_description_and_count_values_alternative_deserialization()
    {
        //Arrange
        var zipFile = UnitTestFileAccessor.GetFile(AmnvFilesUnitTest.XML_AMM_Descriptions_2025_02_17);

        using var zipStream = zipFile.OpenRead();
        using var archive = new ZipArchive(zipStream, ZipArchiveMode.Read);
        var xmlContent = archive.ReadEntryAsString("amm-vet-fr-v2-d.xml");

        //Act
        var res = XmlSerializerHelper.DeserializeFromXml<DonneesReferenceGroupDto>(xmlContent);

        //Assert
        Assert.NotNull(res);
        Assert.Equal(3, res.TermNat.Count); // Natures de médicaments
        Assert.Equal(649, res.TermTit.Count); // Titulaires d'AMM
        Assert.Equal(4, res.TermTypProc.Count); // Types de procédure
        Assert.Equal(17, res.TermStatAuto.Count); // Statuts d'autorisation
        Assert.Equal(285, res.TermFp.Count); // Formes pharmaceutiques
        Assert.Equal(2328, res.TermEsp.Count); // Espèces de destination
        Assert.Equal(3638, res.TermSa.Count); // Substances actives
        Assert.Equal(48, res.TermVa.Count); // Voies d'administration
        Assert.Equal(21, res.TermCd.Count); // Conditions de délivrance
        Assert.Equal(14, res.TermDenr.Count); // Denrées
        Assert.Equal(32, res.TermPres.Count); // Présentations
        Assert.Equal(67, res.TermUnite.Count); // Unités
        Assert.Equal(133, res.TermTitre.Count); // Titres paragraphes RCP
    }

    [Fact]
    public void Deserialize_description_and_validate_content()
    {
        //Arrange
        var zipFile = UnitTestFileAccessor.GetFile(AmnvFilesUnitTest.XML_AMM_Descriptions_2025_02_17);
        using var zipStream = zipFile.OpenRead();
        using var archive = new ZipArchive(zipStream, ZipArchiveMode.Read);
        var xmlContent = archive.ReadEntryAsString("amm-vet-fr-v2-d.xml");

        var dto = AnmvFileHandler.DeserializeDescriptionXmlString(xmlContent)!;

        //Act
        var res = dto.Validate(out var errors);

        //Assert
        Assert.True(res, errors.PrintErrors(Environment.NewLine));
        Assert.True(errors.Count == 0, errors.PrintErrors(Environment.NewLine));
    }
}