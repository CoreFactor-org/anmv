using VetCore.Anmv.Tests.utils;
using VetCore.Anmv.Utils;
using VetCore.Anmv.Utils.Helpers;
using VetCore.Anmv.Xml.Descriptions;

namespace VetCore.Anmv.Tests._2026_06.Descriptions;

public sealed class DeserializeDescriptionTests
{
    [Fact]
    public void Deserialize_description_and_count_values()
    {
        // Arrange
        var xmlContent = UnitTestFileAccessor.GetXmlContent(AmnvFilesUnitTest.XML_AMM_Descriptions_2026_06);

        // Act
        var result = AnmvFileHandler.DeserializeDescriptionXmlString(xmlContent);

        // Assert
        Assert.NotNull(result);
        AssertDescriptionCounts(result);
    }

    [Fact]
    public void Deserialize_description_with_alternative_deserialization_and_count_values()
    {
        // Arrange
        var xmlContent = UnitTestFileAccessor.GetXmlContent(AmnvFilesUnitTest.XML_AMM_Descriptions_2026_06);

        // Act
        var result = XmlSerializerHelper.DeserializeFromXml<DonneesReferenceGroupDto>(xmlContent);

        // Assert
        Assert.NotNull(result);
        AssertDescriptionCounts(result);
    }

    [Fact]
    public void Deserialize_description_and_validate_content()
    {
        // Arrange
        var xmlContent = UnitTestFileAccessor.GetXmlContent(AmnvFilesUnitTest.XML_AMM_Descriptions_2026_06);
        var description = AnmvFileHandler.DeserializeDescriptionXmlString(xmlContent)!;

        // Act
        var isValid = description.Validate(out var errors);

        // Assert
        Assert.True(isValid, errors.PrintErrors(Environment.NewLine));
        Assert.Equal(0, errors.Count);
    }

    private static void AssertDescriptionCounts(DonneesReferenceGroupDto descriptions)
    {
        Assert.Equal(3, descriptions.TermNat.Count); // Natures de médicaments
        Assert.Equal(674, descriptions.TermTit.Count); // Titulaires d'AMM
        Assert.Equal(4, descriptions.TermTypProc.Count); // Types de procédure
        Assert.Equal(17, descriptions.TermStatAuto.Count); // Statuts d'autorisation
        Assert.Equal(286, descriptions.TermFp.Count); // Formes pharmaceutiques
        Assert.Equal(2327, descriptions.TermEsp.Count); // Espèces de destination
        Assert.Equal(3729, descriptions.TermSa.Count); // Substances actives
        Assert.Equal(49, descriptions.TermVa.Count); // Voies d'administration
        Assert.Equal(22, descriptions.TermCd.Count); // Conditions de délivrance
        Assert.Equal(14, descriptions.TermDenr.Count); // Denrées
        Assert.Equal(32, descriptions.TermPres.Count); // Présentations
        Assert.Equal(69, descriptions.TermUnite.Count); // Unités
        Assert.Equal(133, descriptions.TermTitre.Count); // Titres paragraphes RCP
    }
}
