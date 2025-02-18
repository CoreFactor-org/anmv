using System.Text.Json;
using VetCore.Anmv.Json.Data;
using VetCore.Anmv.Json.Description;
using VetCore.Anmv.Xml.Data;
using VetCore.Anmv.Xml.Descriptions;

namespace VetCore.Anmv.Tests.Serialization;

public sealed class JsonDtoInitialStateTests
{
    [Fact]
    public void MedicinalProductGroupDtoJson_Should_Have_Empty_Arrays_On_Instantiation()
    {
        // Act
        var sut = new MedicinalProductGroupDtoJson();

        // Assert
        Assert.NotNull(sut.MedicinalProducts);
        Assert.Empty(sut.MedicinalProducts);
    }

    [Fact]
    public void MedicinalProductDtoJson_Should_Have_Empty_Arrays_On_Instantiation()
    {
        // Act
        var sut = new MedicinalProductDtoJson();

        // Assert
        Assert.NotNull(sut.Compositions);
        Assert.Empty(sut.Compositions);

        Assert.NotNull(sut.VoiesAdmin);
        Assert.Empty(sut.VoiesAdmin);

        Assert.NotNull(sut.ModeleDestineVente);
        Assert.Empty(sut.ModeleDestineVente);

        Assert.NotNull(sut.MdvCodesGtin);
        Assert.Empty(sut.MdvCodesGtin);

        Assert.NotNull(sut.AtcvetCodes);
        Assert.Empty(sut.AtcvetCodes);

        Assert.NotNull(sut.ParagraphesRcp);
        Assert.Empty(sut.ParagraphesRcp);
    }

    [Fact]
    public void MedicinalProductGroupDtoJson_Should_Have_Empty_Arrays_When_Deserialized()
    {
        // Arrange
        const string JSON = "{}";

        // Act
        var sut = JsonSerializer.Deserialize<MedicinalProductGroupDtoJson>(JSON);

        // Assert
        Assert.NotNull(sut);
        Assert.NotNull(sut.MedicinalProducts);
        Assert.Empty(sut.MedicinalProducts);
    }

    [Fact]
    public void MedicinalProductDtoJson_Should_Have_Empty_Arrays_When_Deserialized()
    {
        // Arrange
        const string JSON = "{}";

        // Act
        var sut = JsonSerializer.Deserialize<MedicinalProductDtoJson>(JSON);

        // Assert
        Assert.NotNull(sut);
        Assert.NotNull(sut.Compositions);
        Assert.Empty(sut.Compositions);

        Assert.NotNull(sut.VoiesAdmin);
        Assert.Empty(sut.VoiesAdmin);

        Assert.NotNull(sut.ModeleDestineVente);
        Assert.Empty(sut.ModeleDestineVente);

        Assert.NotNull(sut.MdvCodesGtin);
        Assert.Empty(sut.MdvCodesGtin);

        Assert.NotNull(sut.AtcvetCodes);
        Assert.Empty(sut.AtcvetCodes);

        Assert.NotNull(sut.ParagraphesRcp);
        Assert.Empty(sut.ParagraphesRcp);
    }

    [Fact]
    public void ToJsonDto_Should_Convert_Null_Lists_To_Empty_Arrays_for_data()
    {
        // Arrange
        var xmlDto = new MedicinalProductGroupDto
        {
            Informations = null,
            MedicinalProducts = null,
        };

        // Act
        var sut = xmlDto.ToJsonDto();

        // Assert
        Assert.NotNull(sut);
        Assert.Null(sut.Informations);
        Assert.NotNull(sut.MedicinalProducts);
        Assert.Empty(sut.MedicinalProducts);
    }

    [Fact]
    public void ToJsonDto_Should_Convert_Null_Lists_To_Empty_Arrays_In_MedicinalProductDto()
    {
        // Arrange
        var xmlDto = new MedicinalProductDto
        {
            Compositions = null,
            VoiesAdmin = null,
            ModeleDestineVente = null,
            MdvCodesGtin = null,
            AtcvetCodes = null,
            ParagraphesRcp = null,
        };

        // Act
        var sut = xmlDto.ToJsonDto();

        // Assert
        Assert.NotNull(sut);
        Assert.NotNull(sut.Compositions);
        Assert.Empty(sut.Compositions);

        Assert.NotNull(sut.VoiesAdmin);
        Assert.Empty(sut.VoiesAdmin);

        Assert.NotNull(sut.ModeleDestineVente);
        Assert.Empty(sut.ModeleDestineVente);

        Assert.NotNull(sut.MdvCodesGtin);
        Assert.Empty(sut.MdvCodesGtin);

        Assert.NotNull(sut.AtcvetCodes);
        Assert.Empty(sut.AtcvetCodes);

        Assert.NotNull(sut.ParagraphesRcp);
        Assert.Empty(sut.ParagraphesRcp);
    }

    [Fact]
    public void DonneesReferenceGroupDtoJson_Should_Have_Empty_Arrays_On_Instantiation()
    {
        // Act
        var sut = new DonneesReferenceGroupDtoJson();

        // Assert
        Assert.NotNull(sut.TermNat);
        Assert.Empty(sut.TermNat);

        Assert.NotNull(sut.TermTit);
        Assert.Empty(sut.TermTit);

        Assert.NotNull(sut.TermTypProc);
        Assert.Empty(sut.TermTypProc);

        Assert.NotNull(sut.TermStatAuto);
        Assert.Empty(sut.TermStatAuto);

        Assert.NotNull(sut.TermFp);
        Assert.Empty(sut.TermFp);

        Assert.NotNull(sut.TermEsp);
        Assert.Empty(sut.TermEsp);

        Assert.NotNull(sut.TermSa);
        Assert.Empty(sut.TermSa);

        Assert.NotNull(sut.TermVa);
        Assert.Empty(sut.TermVa);

        Assert.NotNull(sut.TermCd);
        Assert.Empty(sut.TermCd);

        Assert.NotNull(sut.TermDenr);
        Assert.Empty(sut.TermDenr);

        Assert.NotNull(sut.TermPres);
        Assert.Empty(sut.TermPres);

        Assert.NotNull(sut.TermUnite);
        Assert.Empty(sut.TermUnite);

        Assert.NotNull(sut.TermTitre);
        Assert.Empty(sut.TermTitre);
    }

    [Fact]
    public void DonneesReferenceGroupDtoJson_Should_Serialize_Non_Empty_Arrays()
    {
        // Arrange
        var sut = new DonneesReferenceGroupDtoJson
        {
            TermNat = [new TermNatDtoJson()],
            TermTit = [new TermTitDtoJson()],
            TermTypProc = [new TermTypProcDtoJson()],
            TermStatAuto = [new TermStatAutoDtoJson()],
            TermFp = [new TermFpDtoJson()],
            TermEsp = [new TermEspDtoJson()],
            TermSa = [new TermSaDtoJson()],
            TermVa = [new TermVaDtoJson()],
            TermCd = [new TermCdDtoJson()],
            TermDenr = [new TermDenrDtoJson()],
            TermPres = [new TermPresDtoJson()],
            TermUnite = [new TermUniteDtoJson()],
            TermTitre = [new TermTitreDtoJson()],
        };

        // Act
        var json = JsonSerializer.Serialize(sut);

        // Assert
        Assert.Contains("\"termNat\"", json);
        Assert.Contains("\"termTit\"", json);
        Assert.Contains("\"termTypProc\"", json);
        Assert.Contains("\"termStatAuto\"", json);
        Assert.Contains("\"termFp\"", json);
        Assert.Contains("\"termEsp\"", json);
        Assert.Contains("\"termSa\"", json);
        Assert.Contains("\"termVa\"", json);
        Assert.Contains("\"termCd\"", json);
        Assert.Contains("\"termDenr\"", json);
        Assert.Contains("\"termPres\"", json);
        Assert.Contains("\"termUnite\"", json);
        Assert.Contains("\"termTitre\"", json);
    }

    [Fact]
    public void ToJsonDto_Should_Convert_Null_Lists_To_Empty_Arrays()
    {
        // Arrange
        var xmlDto = new DonneesReferenceGroupDto
        {
            TermNat = null,
            TermTit = null,
            TermTypProc = null,
            TermStatAuto = null,
            TermFp = null,
            TermEsp = null,
            TermSa = null,
            TermVa = null,
            TermCd = null,
            TermDenr = null,
            TermPres = null,
            TermUnite = null,
            TermTitre = null,
        };

        // Act
        var sut = xmlDto.ToJsonDto();

        // Assert
        Assert.NotNull(sut.TermNat);
        Assert.Empty(sut.TermNat);

        Assert.NotNull(sut.TermTit);
        Assert.Empty(sut.TermTit);

        Assert.NotNull(sut.TermTypProc);
        Assert.Empty(sut.TermTypProc);

        Assert.NotNull(sut.TermStatAuto);
        Assert.Empty(sut.TermStatAuto);

        Assert.NotNull(sut.TermFp);
        Assert.Empty(sut.TermFp);

        Assert.NotNull(sut.TermEsp);
        Assert.Empty(sut.TermEsp);

        Assert.NotNull(sut.TermSa);
        Assert.Empty(sut.TermSa);

        Assert.NotNull(sut.TermVa);
        Assert.Empty(sut.TermVa);

        Assert.NotNull(sut.TermCd);
        Assert.Empty(sut.TermCd);

        Assert.NotNull(sut.TermDenr);
        Assert.Empty(sut.TermDenr);

        Assert.NotNull(sut.TermPres);
        Assert.Empty(sut.TermPres);

        Assert.NotNull(sut.TermUnite);
        Assert.Empty(sut.TermUnite);

        Assert.NotNull(sut.TermTitre);
        Assert.Empty(sut.TermTitre);
    }
}