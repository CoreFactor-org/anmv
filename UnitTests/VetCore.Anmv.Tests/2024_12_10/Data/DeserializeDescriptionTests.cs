using VetCore.Anmv.Tests.data;
using VetCore.Anmv.Utils;
using VetCore.Anmv.Utils.Helpers;
using VetCore.Anmv.Xml.Data;

namespace VetCore.Anmv.Tests._2024_12_10.Data;

public sealed class DeserializeDataTests
{

    [Fact]
    public void Deserialize_data_short()
    {
        //Arrange
        var file = UnitTestFileAccessor.GetFile(AmnvFilesUnitTest.XML_AMM_Data_short_2024_12_10);

        //Act
        var res = AnmvFileHandler.DeserializeDataFile(file.ToFileInfo());

        //Assert
        Assert.NotNull(res);
        Assert.Equal(120, res.MedicinalProducts.Count);
    }

    [Fact]
    public void Deserialize_data_and_count_values()
    {
        //Arrange
        var file = UnitTestFileAccessor.GetFile(AmnvFilesUnitTest.XML_AMM_Data_2024_12_10);

        //Act
        var res = AnmvFileHandler.DeserializeDataFile(file.ToFileInfo());

        //Assert
        Assert.NotNull(res);
    }

    [Fact]
    public void Serialize_data_should_returns_expected_structure()
    {
        //Arrange
        var dto = new MedicinalProductGroupDto
        {
            Informations = new InformationsDto
            {
                DateJeuDeDonnees = new DateTime(2024, 6, 30),
            },
            MedicinalProducts =
            [
                new MedicinalProductDto
                {
                    SrcId = 447,
                    Nom = "NEOMYCINE",
                    Num = "0033630",
                    TermTit = 294,
                    TermNat = 1,
                    TermTypProc = 1,
                    TermStatAuto = 4,
                    DateAmm = new DateTime(1992, 6, 30),
                    TermFp = 148,
                    NumAmm = "FR/V/2666590 4/1992",
                    PermId = "600000093156",
                    ProdId = new Guid("010e5bd2-2449-40bb-8511-9572efa82be0"),
                    MajRcp = new DateTime(1998, 6, 30),
                    LienRcp = "http://www.ircp.anmv.anses.fr/rcp.aspx?NomMedicament=NEOMYCINE",
                    Compositions =
                    [
                        new CompositionDto
                        {
                            Sa = new SaDto
                            {
                                TermSa = 1420,
                                Quantite = "56",
                                TermUnite = 7225,
                            },
                            Fraction = new FractionDto
                            {
                                TermSa = 1420,
                                Quantite = "1858",
                                TermUnite = 7225,
                            },
                        },
                    ],
                    VoiesAdmin =
                    [
                        new VoieAdministrationDto
                        {
                            TermVa = 24,
                            TermEsp = 22,
                            TermDenr = 10,
                            QteTa = "1",
                            TermUnite = 22,
                        },
                    ],
                    ModeleDestineVente =
                    [
                        new ModeleDestineVenteDto
                        {
                            LibMod = "Boîte de 10 sachets de 100 g",
                            NbUnit = "10",
                            TermPres = 1,
                            TermCd = 3,
                            LibCondp = "Flacon verre",
                        },
                    ],
                    MdvCodesGtin =
                    [
                        new MdvCodesGtinDto
                        {
                            LibMod = "Boîte de 10 seringues de 1 dose",
                            PackId = new Guid("4771d347-40bb-4615-9655-6372947eb405"),
                            CodeGtin = "09088881342311",
                            NumAmm = "EU/2/96/002/001",
                        },
                    ],
                    ExcipientQsp = new ExcipientQspDto
                    {
                        QteQsp = "1",
                        TermPres = 2,
                        TermUnite = "ml",
                    },
                    AtcvetCode = ["QJ01EQ03"],
                    ParagraphesRcp =
                    [
                        new ParaRcpDto
                        {
                            TermTitre = 1,
                            Contenu = "Texte du paragraphe.",
                        },
                    ],
                },
            ],
        };

        //Act
        var res = dto.SerializeAmnvToXml(indent: true);

        //Assert
        Assert.Equal(
            """
            <medicinal-product-group>
              <Informations>
                <date-jeu-de-donnees>2024-06-30T00:00:00</date-jeu-de-donnees>
              </Informations>
              <medicinal-product>
                <src-id>447</src-id>
                <nom>NEOMYCINE</nom>
                <num>0033630</num>
                <term-tit>294</term-tit>
                <term-nat>1</term-nat>
                <term-typ-proc>1</term-typ-proc>
                <term-stat-auto>4</term-stat-auto>
                <date-amm>1992-06-30T00:00:00</date-amm>
                <term-fp>148</term-fp>
                <num-amm>FR/V/2666590 4/1992</num-amm>
                <perm-id>600000093156</perm-id>
                <prod-id>010e5bd2-2449-40bb-8511-9572efa82be0</prod-id>
                <maj-rcp>1998-06-30T00:00:00</maj-rcp>
                <lien-rcp>http://www.ircp.anmv.anses.fr/rcp.aspx?NomMedicament=NEOMYCINE</lien-rcp>
                <composition>
                  <compo>
                    <sa>
                      <term-sa>1420</term-sa>
                      <quantite>56</quantite>
                      <term-unite>7225</term-unite>
                    </sa>
                    <fraction>
                      <term-sa>1420</term-sa>
                      <quantite>1858</quantite>
                      <term-unite>7225</term-unite>
                    </fraction>
                  </compo>
                </composition>
                <voie-administration>
                  <voie-admin>
                    <term-va>24</term-va>
                    <term-esp>22</term-esp>
                    <term-denr>10</term-denr>
                    <qte-ta>1</qte-ta>
                    <term-unite>22</term-unite>
                  </voie-admin>
                </voie-administration>
                <modele-destine-vente>
                  <mod-vte>
                    <lib-mod>Boîte de 10 sachets de 100 g</lib-mod>
                    <nb-unit>10</nb-unit>
                    <term-pres>1</term-pres>
                    <term-cd>3</term-cd>
                    <lib-condp>Flacon verre</lib-condp>
                  </mod-vte>
                </modele-destine-vente>
                <mdv-codes-gtin>
                  <mod-vte>
                    <lib-mod>Boîte de 10 seringues de 1 dose</lib-mod>
                    <pack-id>4771d347-40bb-4615-9655-6372947eb405</pack-id>
                    <code-gtin>09088881342311</code-gtin>
                    <num-amm>EU/2/96/002/001</num-amm>
                  </mod-vte>
                </mdv-codes-gtin>
                <excipient-qsp>
                  <qte-qsp>1</qte-qsp>
                  <term-pres>2</term-pres>
                  <term-unite>ml</term-unite>
                </excipient-qsp>
                <atcvet-code>
                  <code-atcvet>QJ01EQ03</code-atcvet>
                </atcvet-code>
                <paragraphes-rcp>
                  <para-rcp>
                    <term-titre>1</term-titre>
                    <contenu>Texte du paragraphe.</contenu>
                  </para-rcp>
                </paragraphes-rcp>
              </medicinal-product>
            </medicinal-product-group>
            """,
            res);
    }

    [Fact]
    public void Serialize_CompositionDto_should_ignore_fraction_if_empty_false()
    {
        //Arrange
        var dto = new CompositionDto
        {
            Sa = new SaDto
            {
                TermSa = 1420,
                Quantite = "56",
                TermUnite = 7225,
            },
            Fraction = new FractionDto
            {
                TermSa = 1420,
                Quantite = "1858",
                TermUnite = 7225,
            },
        };

        //Act
        var res = XmlSerializerHelper.SerializeToXml(dto, indent: true);

        //Assert
        Assert.Equal(
            """
            <CompositionDto>
              <sa>
                <term-sa>1420</term-sa>
                <quantite>56</quantite>
                <term-unite>7225</term-unite>
              </sa>
              <fraction>
                <term-sa>1420</term-sa>
                <quantite>1858</quantite>
                <term-unite>7225</term-unite>
              </fraction>
            </CompositionDto>
            """,
            res);
    }

    [Fact]
    public void Serialize_CompositionDto_should_ignore_fraction_if_empty_true()
    {
        //Arrange
        var dto = new CompositionDto
        {
            Sa = new SaDto
            {
                TermSa = 1420,
                Quantite = "56",
                TermUnite = 7225,
            },
            // empty fraction
        };

        //Act
        var res = XmlSerializerHelper.SerializeToXml(dto, indent: true);

        //Assert
        Assert.Equal(
            """
            <CompositionDto>
              <sa>
                <term-sa>1420</term-sa>
                <quantite>56</quantite>
                <term-unite>7225</term-unite>
              </sa>
            </CompositionDto>
            """,
            res);
    }
}