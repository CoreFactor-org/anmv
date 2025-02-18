using VetCore.Anmv.Json.Data;
using VetCore.Anmv.Xml.Data;

namespace VetCore.Anmv.Tests.Serialization;

public class MedicinalProductGroupDtoConversionTests
{
    [Fact]
    public void XmlToJsonConversion_ShouldConvertCorrectly()
    {
        // Arrange
        var xmlDto = new MedicinalProductGroupDto
        {
            Informations = new InformationsDto
            {
                DateJeuDeDonnees = new DateTime(2025, 01, 01),
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
                    ProdId = Guid.NewGuid(),
                    MajRcp = DateTime.Now,
                    LienRcp = "http://www.ircp.anmv.anses.fr/rcp.aspx?NomMedicament=NEOMYCINE",
                    Compositions =
                    [
                        new CompositionDto
                        {
                            Sa = new SaDto { TermSa = 1420, Quantite = "56,80", TermUnite = 7225 },
                            Fraction = new FractionDto { TermSa = 1420, Quantite = "1,858", TermUnite = 7225 },
                        },
                    ],
                    VoiesAdmin = [new VoieAdministrationDto { TermVa = 24, TermEsp = 22, TermDenr = 10, QteTa = "Dose", TermUnite = 22 }],
                    ModeleDestineVente = [new ModeleDestineVenteDto { LibMod = "Boîte de 10 sachets", NbUnit = "10", TermPres = 1, TermCd = 10, LibCondp = "Flacon verre" }],
                    MdvCodesGtin = [new MdvCodesGtinDto { LibMod = "Boîte", PackId = Guid.NewGuid(), CodeGtin = "09088881342311", NumAmm = "EU/2/96/002/001" }],
                    ExcipientQsp = new ExcipientQspDto { QteQsp = "100", TermPres = 1, TermUnite = 2 },
                    AtcvetCodes = ["ATC123"],
                    ParagraphesRcp = [new ParaRcpDto { TermTitre = 1, Contenu = "Contenu du paragraphe" }],
                },
            ],
        };

        // Act
        var jsonDto = xmlDto.ToJsonDto();

        // Assert
        Assert.NotNull(jsonDto);
        Assert.Equal(xmlDto.Informations.DateJeuDeDonnees, jsonDto.Informations.DateJeuDeDonnees);
        Assert.Equal(xmlDto.MedicinalProducts.Count, jsonDto.MedicinalProducts.Length);
        Assert.Equal(xmlDto.MedicinalProducts.First().Nom, jsonDto.MedicinalProducts.First().Nom);
    }

    [Fact]
    public void JsonToXmlConversion_ShouldConvertCorrectly()
    {
        // Arrange
        var jsonDto = new MedicinalProductGroupDtoJson
        {
            Informations = new InformationsDtoJson { DateJeuDeDonnees = new DateTime(2025, 01, 01) },
            MedicinalProducts =
            [
                new MedicinalProductDtoJson
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
                    ProdId = Guid.NewGuid(),
                    MajRcp = DateTime.Now,
                    LienRcp = "http://www.ircp.anmv.anses.fr/rcp.aspx?NomMedicament=NEOMYCINE",
                    Compositions =
                    [
                        new CompositionDtoJson
                        {
                            Sa = new SaDtoJson { TermSa = 1420, Quantite = "56,80", TermUnite = 7225 },
                            Fraction = new FractionDtoJson { TermSa = 1420, Quantite = "1,858", TermUnite = 7225 },
                        },
                    ],
                    VoiesAdmin =
                    [
                        new VoieAdministrationDtoJson { TermVa = 24, TermEsp = 22, TermDenr = 10, QteTa = "Dose", TermUnite = 22 },
                    ],
                    ModeleDestineVente =
                    [
                        new ModeleDestineVenteDtoJson { LibMod = "Boîte de 10 sachets", NbUnit = "10", TermPres = 1, TermCd = 10, LibCondp = "Flacon verre" },
                    ],
                    MdvCodesGtin =
                    [
                        new MdvCodesGtinDtoJson { LibMod = "Boîte", PackId = Guid.NewGuid(), CodeGtin = "09088881342311", NumAmm = "EU/2/96/002/001" },
                    ],
                    ExcipientQsp = new ExcipientQspDtoJson
                    {
                        QteQsp = "100",
                        TermPres = 1,
                        TermUnite = 3,
                    },
                    AtcvetCodes = ["ATC123"],
                    ParagraphesRcp =
                    [
                        new ParaRcpDtoJson { TermTitre = 1, Contenu = "Contenu du paragraphe" },
                    ],
                },
            ],
        };

        // Act
        var xmlDto = jsonDto.ToXmlDto().ToJsonDto().ToXmlDto();

        // Assert
        Assert.NotNull(xmlDto);
        Assert.Equal(jsonDto.Informations.DateJeuDeDonnees, xmlDto.Informations.DateJeuDeDonnees);
        Assert.Equal(jsonDto.MedicinalProducts.Length, xmlDto.MedicinalProducts.Count);
        Assert.Equal(jsonDto.MedicinalProducts.First().Nom, xmlDto.MedicinalProducts.First().Nom);
    }

    [Fact]
    public void XmlToJsonConversion_NullInput_ShouldReturnNull()
    {
        // Arrange
        MedicinalProductGroupDto? xmlDto = null;

        // Act
        var jsonDto = xmlDto.ToJsonDto();

        // Assert
        Assert.Null(jsonDto);
    }

    [Fact]
    public void JsonToXmlConversion_NullInput_ShouldReturnNull()
    {
        // Arrange
        MedicinalProductGroupDtoJson? jsonDto = null;

        // Act
        var xmlDto = jsonDto.ToXmlDto();

        // Assert
        Assert.Null(xmlDto);
    }
}