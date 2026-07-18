using VetCore.Anmv.Tests.utils;
using VetCore.Anmv.Utils;
using VetCore.Anmv.Xml.Descriptions;

namespace VetCore.Anmv.Tests._2026_06.Descriptions;

public class JsonValidationTests
{
    [Fact]
    public void Deserialize_description_then_convert_toJson_and_count_values()
    {
        // Arrange
        var xmlContent = UnitTestFileAccessor.GetXmlContent(AmnvFilesUnitTest.XML_AMM_Descriptions_2026_06);

        // Act
        var result = AnmvFileHandler.DeserializeDescriptionXmlString(xmlContent).ToJsonDto();

        // Assert
        Assert.NotNull(result);
        Assert.Equal(3, result.TermNat.Length); // Natures de médicaments
        Assert.Equal(674, result.TermTit.Length); // Titulaires d'AMM
        Assert.Equal(4, result.TermTypProc.Length); // Types de procédure
        Assert.Equal(17, result.TermStatAuto.Length); // Statuts d'autorisation
        Assert.Equal(286, result.TermFp.Length); // Formes pharmaceutiques
        Assert.Equal(2327, result.TermEsp.Length); // Espèces de destination
        Assert.Equal(3729, result.TermSa.Length); // Substances actives
        Assert.Equal(49, result.TermVa.Length); // Voies d'administration
        Assert.Equal(22, result.TermCd.Length); // Conditions de délivrance
        Assert.Equal(14, result.TermDenr.Length); // Denrées
        Assert.Equal(32, result.TermPres.Length); // Présentations
        Assert.Equal(69, result.TermUnite.Length); // Unités
        Assert.Equal(133, result.TermTitre.Length); // Titres paragraphes RCP
    }

    [Fact]
    public void Deserialize_description_then_convert_toJson_and_validate()
    {
        // Arrange
        var xmlContent = UnitTestFileAccessor.GetXmlContent(AmnvFilesUnitTest.XML_AMM_Descriptions_2026_06);

        // Act
        var result = AnmvFileHandler.DeserializeDescriptionXmlString(xmlContent).ToJsonDto();

        // Assert
        Assert.True(result.IsDtoValidAccordingToAttributes());
    }
}
