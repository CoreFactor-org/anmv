using VetCore.Anmv.Utils;
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
    public void Deserialize_description_and_count_values_diff()
    {
        //Arrange
        var fileCurrent = UnitTestFileAccessor.GetFile(AmnvFilesUnitTest.XML_AMM_Descriptions_2025_01_14);
        var filePrevious = UnitTestFileAccessor.GetFile(AmnvFilesUnitTest.XML_AMM_Descriptions_2024_12_10);

        //Act
        var resCurrent = AnmvFileHandler.DeserializeDescriptionFile(fileCurrent.ToFileInfo())!;
        var resPrevious = AnmvFileHandler.DeserializeDescriptionFile(filePrevious.ToFileInfo())!;

        //Assert
        // Attention, puisque les xs:choice minOccurs="0" maxOccurs="unbounded" sont unbounded
        // dans les xsd, les racines peuvent être multiples et il faut donc faire un Single().
        // Cela semble probablement être un problème dans les données, mais bon...
        Assert.Equal(resPrevious.TermNat.Count, resCurrent.TermNat.Count); // Natures de médicaments
        Assert.Equal(resPrevious.TermTypProc.Count, resCurrent.TermTypProc.Count); // Types de procédure
        Assert.Equal(resPrevious.TermStatAuto.Count, resCurrent.TermStatAuto.Count); // Statuts d'autorisation
        Assert.Equal(resPrevious.TermFp.Count, resCurrent.TermFp.Count); // Formes pharmaceutiques
        Assert.Equal(resPrevious.TermEsp.Count, resCurrent.TermEsp.Count); // Espèces de destination
        Assert.Equal(resPrevious.TermVa.Count, resCurrent.TermVa.Count); // Voies d'administration
        Assert.Equal(resPrevious.TermCd.Count, resCurrent.TermCd.Count); // Conditions de délivrance
        Assert.Equal(resPrevious.TermQsp.Count, resCurrent.TermQsp.Count); // Excipients QSP
        Assert.Equal(resPrevious.TermDenr.Count, resCurrent.TermDenr.Count); // Denrées
        Assert.Equal(resPrevious.TermPres.Count, resCurrent.TermPres.Count); // Présentations
        Assert.Equal(resPrevious.TermUnite.Count, resCurrent.TermUnite.Count); // Unités
        Assert.Equal(resPrevious.TermTitre.Count, resCurrent.TermTitre.Count); // Titres paragraphes RCP

      // followings are different
        _testOutputHelper.CompareAndLogDifferences(resPrevious.TermSa, resCurrent.TermSa, "TermSa"); // Substances actives
        _testOutputHelper.CompareAndLogDifferences(resPrevious.TermTit, resCurrent.TermTit, "TermTit");// Titulaires d'AMM
    }

    [Fact]
    public void Serialize_description_should_returns_expected_structure()
    {
        //Arrange
        var dto = new DonneesReferenceGroupDto
        {
            TermNat = [new EntryDto { SourceCode = 78, SourceDesc = "foo" }],
            TermTit = [new EntryDto { SourceCode = 78, SourceDesc = "foo" }],
            TermTypProc = [new EntryDto { SourceCode = 78, SourceDesc = "foo" }],
            TermStatAuto = [new EntryDto { SourceCode = 78, SourceDesc = "foo" }],
            TermFp = [new EntryDto { SourceCode = 78, SourceDesc = "foo" }],
            TermEsp = [new EntryDto { SourceCode = 78, SourceDesc = "foo" }],
            TermSa = [new EntryDto { SourceCode = 78, SourceDesc = "foo" }],
            TermVa = [new EntryDto { SourceCode = 78, SourceDesc = "foo" }],
            TermCd = [new EntryDto { SourceCode = 78, SourceDesc = "foo" }],
            TermQsp = [new EntryDto { SourceCode = 78, SourceDesc = "foo" }],
            TermDenr = [new EntryDto { SourceCode = 78, SourceDesc = "foo" }],
            TermPres = [new EntryDto { SourceCode = 78, SourceDesc = "foo" }],
            TermUnite = [new EntryDto { SourceCode = 78, SourceDesc = "foo" }],
            TermTitre = [new EntryOrdreDto { SourceCode = 78, SourceDesc = "foo", Ordre = 42 }],
        };

        //Act
        var res = dto.SerializeAmnvToXml(indent: true);

        //Assert
        Assert.Equal(
            """
            <donnees-reference-group>
              <term-nat>
                <entry>
                  <source-code>78</source-code>
                  <source-desc>foo</source-desc>
                </entry>
              </term-nat>
              <term-tit>
                <entry>
                  <source-code>78</source-code>
                  <source-desc>foo</source-desc>
                </entry>
              </term-tit>
              <term-typ-proc>
                <entry>
                  <source-code>78</source-code>
                  <source-desc>foo</source-desc>
                </entry>
              </term-typ-proc>
              <term-stat-auto>
                <entry>
                  <source-code>78</source-code>
                  <source-desc>foo</source-desc>
                </entry>
              </term-stat-auto>
              <term-fp>
                <entry>
                  <source-code>78</source-code>
                  <source-desc>foo</source-desc>
                </entry>
              </term-fp>
              <term-esp>
                <entry>
                  <source-code>78</source-code>
                  <source-desc>foo</source-desc>
                </entry>
              </term-esp>
              <term-sa>
                <entry>
                  <source-code>78</source-code>
                  <source-desc>foo</source-desc>
                </entry>
              </term-sa>
              <term-va>
                <entry>
                  <source-code>78</source-code>
                  <source-desc>foo</source-desc>
                </entry>
              </term-va>
              <term-cd>
                <entry>
                  <source-code>78</source-code>
                  <source-desc>foo</source-desc>
                </entry>
              </term-cd>
              <term-qsp>
                <entry>
                  <source-code>78</source-code>
                  <source-desc>foo</source-desc>
                </entry>
              </term-qsp>
              <term-denr>
                <entry>
                  <source-code>78</source-code>
                  <source-desc>foo</source-desc>
                </entry>
              </term-denr>
              <term-pres>
                <entry>
                  <source-code>78</source-code>
                  <source-desc>foo</source-desc>
                </entry>
              </term-pres>
              <term-unite>
                <entry>
                  <source-code>78</source-code>
                  <source-desc>foo</source-desc>
                </entry>
              </term-unite>
              <term-titre>
                <entry>
                  <source-code>78</source-code>
                  <source-desc>foo</source-desc>
                  <ordre>42</ordre>
                </entry>
              </term-titre>
            </donnees-reference-group>
            """,
            res);
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