using VetCore.Anmv.Tests.utils;
using VetCore.Anmv.Utils;
using VetCore.Anmv.Utils.Helpers;
using VetCore.Anmv.Xml.Descriptions;
using Xunit.Abstractions;

namespace VetCore.Anmv.Tests._2025_01_14.Descriptions;

public sealed class DeserializeDescriptionTests
{
    private readonly ITestOutputHelper _testOutputHelper;

    public DeserializeDescriptionTests(ITestOutputHelper testOutputHelper)
    {
        _testOutputHelper = testOutputHelper;
    }

    [Fact]
    public void Deserialize_description_and_count_values()
    {
        //Arrange
        var file = UnitTestFileAccessor.GetFile(AmnvFilesUnitTest.XML_AMM_Descriptions_2025_01_14);

        //Act
        var res = AnmvFileHandler.DeserializeDescriptionFile(file.ToFileInfo());

        //Assert
        Assert.NotNull(res);
        // Attention, puisque les xs:choice minOccurs="0" maxOccurs="unbounded" sont unbounded
        // dans les xsd, les racines peuvent être multiples et il faut donc faire un Single().
        // Cela semble probablement être un problème dans les données, mais bon...
        Assert.Equal(3, res.TermNat.Count); // Natures de médicaments
        Assert.Equal(647, res.TermTit.Count); // Titulaires d'AMM
        Assert.Equal(4, res.TermTypProc.Count); // Types de procédure
        Assert.Equal(17, res.TermStatAuto.Count); // Statuts d'autorisation
        Assert.Equal(285, res.TermFp.Count); // Formes pharmaceutiques
        Assert.Equal(2328, res.TermEsp.Count); // Espèces de destination
        Assert.Equal(3632, res.TermSa.Count); // Substances actives
        Assert.Equal(48, res.TermVa.Count); // Voies d'administration
        Assert.Equal(21, res.TermCd.Count); // Conditions de délivrance
        Assert.Equal(32, res.TermQsp.Count); // Excipients QSP
        Assert.Equal(14, res.TermDenr.Count); // Denrées
        Assert.Equal(32, res.TermPres.Count); // Présentations
        Assert.Equal(67, res.TermUnite.Count); // Unités
        Assert.Equal(133, res.TermTitre.Count); // Titres paragraphes RCP
    }

    [Fact]
    public void Deserialize_description_and_count_values_alternative_deserialization()
    {
        //Arrange
        var file = UnitTestFileAccessor.GetFile(AmnvFilesUnitTest.XML_AMM_Descriptions_2025_01_14);

        //Act
        var res = XmlSerializerHelper.DeserializeFromXml<DonneesReferenceGroupDto>(file.ReadAllText());

        //Assert
        Assert.NotNull(res);
        // Attention, puisque les xs:choice minOccurs="0" maxOccurs="unbounded" sont unbounded
        // dans les xsd, les racines peuvent être multiples et il faut donc faire un Single().
        // Cela semble probablement être un problème dans les données, mais bon...
        Assert.Equal(3, res.TermNat.Count); // Natures de médicaments
        Assert.Equal(647, res.TermTit.Count); // Titulaires d'AMM
        Assert.Equal(4, res.TermTypProc.Count); // Types de procédure
        Assert.Equal(17, res.TermStatAuto.Count); // Statuts d'autorisation
        Assert.Equal(285, res.TermFp.Count); // Formes pharmaceutiques
        Assert.Equal(2328, res.TermEsp.Count); // Espèces de destination
        Assert.Equal(3632, res.TermSa.Count); // Substances actives
        Assert.Equal(48, res.TermVa.Count); // Voies d'administration
        Assert.Equal(21, res.TermCd.Count); // Conditions de délivrance
        Assert.Equal(32, res.TermQsp.Count); // Excipients QSP
        Assert.Equal(14, res.TermDenr.Count); // Denrées
        Assert.Equal(32, res.TermPres.Count); // Présentations
        Assert.Equal(67, res.TermUnite.Count); // Unités
        Assert.Equal(133, res.TermTitre.Count); // Titres paragraphes RCP
    }

    [Fact]
    public void Deserialize_description_and_validate_content()
    {
        //Arrange
        var file = UnitTestFileAccessor.GetFile(AmnvFilesUnitTest.XML_AMM_Descriptions_2025_01_14);
        var dto = AnmvFileHandler.DeserializeDescriptionFile(file.ToFileInfo())!;

        //Act
        var res = dto.Validate(out var errors);

        //Assert
        Assert.True(res, errors.PrintErrors(Environment.NewLine));
        Assert.True(errors.Count == 0, errors.PrintErrors(Environment.NewLine));
    }
}