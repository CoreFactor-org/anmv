using System.Text.Json;
using VetCore.Anmv.RepMedVet;
using VetCore.Anmv.RepMedVet.Json;
using VetCore.Anmv.RepMedVet.Xml;

namespace VetCore.Anmv.Tests.Serialization;

public sealed class RepMedVetDtoConversionTests
{
    [Fact]
    public void XmlToJsonConversion_ShouldConvertCorrectly()
    {
        // Arrange
        var xmlDto = CreateRepository();

        // Act
        var jsonDto = xmlDto.ToJsonDto();

        // Assert
        Assert.Equal(new DateTimeOffset(2026, 10, 22, 5, 3, 13, TimeSpan.FromHours(2)), jsonDto.ExchangedDocument.IssueDatetime);
        Assert.Equal("MED 0.9", jsonDto.ExchangedDocument.ExchangeScenario);
        Assert.Equal("13001202400100", jsonDto.Sender.IdParty.Value);
        var product = Assert.Single(jsonDto.Products);
        Assert.True(product.IsSubjectToReporting);
        Assert.Equal(85, product.Product.Id);
        Assert.Equal("mL", product.QspUnit.Label);
        Assert.Equal("03411113021061", Assert.Single(product.Drugs).GtinCode);
        Assert.Equal("532,6", Assert.Single(product.ActiveSubstances).ActiveSubstanceConcentration.Quantity);
        Assert.Equal("Aucune.", product.SummaryOfProductCharacteristics.SpecialPrecautionsForUse.OtherPrecautions);
    }

    [Fact]
    public void JsonToXmlConversion_ShouldConvertCorrectly()
    {
        // Arrange
        var xmlDto = CreateRepository();

        // Act
        var result = xmlDto.ToJsonDto().ToXmlDto();

        // Assert
        Assert.Equal(xmlDto.SerializeToXml(), result.SerializeToXml());
    }

    [Theory]
    [InlineData(false, SubjectToReportingDto.No)]
    [InlineData(true, SubjectToReportingDto.Yes)]
    public void JsonToXmlConversion_ShouldConvertTheReportingFlag(bool json, SubjectToReportingDto expected)
    {
        // Arrange
        var jsonDto = new ProductDtoJson { IsSubjectToReporting = json };

        // Act
        var result = jsonDto.ToXmlDto();

        // Assert
        Assert.Equal(expected, result.IsSubjectToReporting);
    }

    [Fact]
    public void Conversions_ShouldReturnNull_WhenTheSourceIsNull()
    {
        // Arrange
        var xmlDto = (MedecineRepositoryDto)null!;
        var jsonDto = (MedecineRepositoryDtoJson)null!;

        // Act / Assert
        Assert.Null(xmlDto.ToJsonDto());
        Assert.Null(jsonDto.ToXmlDto());
    }

    [Fact]
    public void Conversions_ShouldKeepTheOptionalPartsNull()
    {
        // Arrange
        var xmlDto = new ProductDto { Product = new CodedValueDto { Id = 85, Label = "GLEPTOSIL" } };

        // Act
        var jsonDto = xmlDto.ToJsonDto();
        var result = jsonDto.ToXmlDto();

        // Assert
        Assert.Null(jsonDto.AuthorisationStatus);
        Assert.Null(jsonDto.AuthorisationStatusDate);
        Assert.Null(jsonDto.QspUnit);
        Assert.Null(jsonDto.SummaryOfProductCharacteristics);
        Assert.Empty(jsonDto.Drugs);
        Assert.Null(result.AuthorisationStatusDate);
        Assert.Null(result.SummaryOfProductCharacteristics);
    }

    [Fact]
    public void Serialize_ShouldFollowTheFileFormat()
    {
        // Arrange
        var dto = new MedecineRepositoryDto
        {
            Products =
            [
                new ProductDto
                {
                    Product = new CodedValueDto { Id = 85, Label = "GLEPTOSIL" },
                    AtcVetCodes = ["QB03AC90"],
                    AuthorisationStatusDate = new DateTime(2009, 7, 23),
                    IsSubjectToReporting = SubjectToReportingDto.Yes,
                    Drugs = [new DrugDto { Id = new GtinDto { GtinCode = "03411113021061" }, Name = "GLEPTOSIL - Flacon", AuthorisationNumber = "", Packaging = "Flacon" }],
                    PharmaceuticalForm = new CodedValueDto { Id = 190, Label = "Solution injectable" },
                },
            ],
        };

        // Act
        var result = dto.SerializeToXml(indent: true);

        // Assert
        Assert.Equal(
            """
            <MedecineRepository>
              <Product>
                <Product ID="85">GLEPTOSIL</Product>
                <AtcVetCode>QB03AC90</AtcVetCode>
                <AuthorisationStatusDate>2009-07-23</AuthorisationStatusDate>
                <IsSubjectToReporting>1</IsSubjectToReporting>
                <Drug>
                  <ID GTINCode="03411113021061" />
                  <Name>GLEPTOSIL - Flacon</Name>
                  <AuthorisationNumber />
                  <Packaging>Flacon</Packaging>
                </Drug>
                <PharmaceuticalForm ID="190">Solution injectable</PharmaceuticalForm>
              </Product>
            </MedecineRepository>
            """.ReplaceLineEndings(),
            result.ReplaceLineEndings());
    }

    [Fact]
    public void Serialize_ShouldNotWriteAnAbsentDate()
    {
        // Arrange
        var dto = new MedecineRepositoryDto { Products = [new ProductDto()] };

        // Act
        var result = dto.SerializeToXml();

        // Assert : le fichier source omet l'élément, il ne le marque jamais xsi:nil.
        Assert.DoesNotContain("AuthorisationStatusDate", result);
        Assert.DoesNotContain("nil", result);
    }

    [Fact]
    public void ProductDtoJson_ShouldHaveEmptyArraysOnInstantiation()
    {
        // Act
        var sut = new ProductDtoJson();

        // Assert
        Assert.Empty(sut.AtcVetCodes);
        Assert.Empty(sut.Drugs);
        Assert.Empty(sut.ActiveSubstances);
        Assert.Empty(sut.TargetSpecies);
        Assert.Empty(sut.WithdrawalPeriods);
    }

    [Fact]
    public void MedecineRepositoryDtoJson_ShouldHaveEmptyArraysWhenDeserialized()
    {
        // Act
        var sut = JsonSerializer.Deserialize<MedecineRepositoryDtoJson>("{}");

        // Assert
        Assert.NotNull(sut);
        Assert.Empty(sut.Products);
    }

    [Fact]
    public void ProductDtoJson_ShouldRefuseAMissingReportingFlag()
    {
        // Arrange
        const string JSON = """{"product":{"id":85,"label":"GLEPTOSIL"}}""";

        // Act / Assert : une absence ne doit jamais se lire comme « non déclarable ».
        Assert.Throws<JsonException>(() => JsonSerializer.Deserialize<ProductDtoJson>(JSON));
    }

    private static MedecineRepositoryDto CreateRepository()
    {
        return new MedecineRepositoryDto
        {
            ExchangedDocument = new ExchangedDocumentDto
            {
                IssueDatetime = new DateTimeOffset(2026, 10, 22, 5, 3, 13, TimeSpan.FromHours(2)),
                FileName = "REPMEDVET",
                TypeCode = "REPMEC",
                ExchangeScenario = "MED 0.9",
            },
            Sender = new SenderDto
            {
                IdParty = new PartyIdDto { TypeId = "SIRET", Value = "13001202400100" },
                Name = "ANMV",
            },
            Products =
            [
                new ProductDto
                {
                    Product = new CodedValueDto { Id = 85, Label = "GLEPTOSIL" },
                    MarketingAuthorisationHolder = new CodedValueDto { Id = 1659, Label = "CEVA SANTE ANIMALE" },
                    AtcVetCodes = ["QB03AC90"],
                    AuthorisationStatus = new CodedValueDto { Id = 4, Label = "AMM illimitée" },
                    AuthorisationStatusDate = new DateTime(2009, 7, 23),
                    QspUnit = new CodedValueDto { Id = 11, Label = "mL" },
                    IsSubjectToReporting = SubjectToReportingDto.Yes,
                    Drugs =
                    [
                        new DrugDto
                        {
                            Id = new GtinDto { GtinCode = "03411113021061" },
                            Name = "GLEPTOSIL - Boîte de 1 flacon rigide de 250 mL",
                            AuthorisationNumber = "FR/V/7008561 5/1979",
                            Packaging = "Boîte de 1 flacon rigide de 250 mL",
                        },
                    ],
                    ActiveSubstances =
                    [
                        new ActiveSubstanceDto
                        {
                            ActiveSubstance = new CodedValueDto { Id = 1006, Label = "Gleptoferron" },
                            ActiveSubstanceConcentration = new ConcentrationDto { Quantity = "532,6", Unit = new CodedValueDto { Id = 10, Label = "mg" } },
                            ActiveFraction = new CodedValueDto { Id = 905, Label = "Fer (III)" },
                            ActiveFractionConcentration = new ConcentrationDto { Quantity = "200", Unit = new CodedValueDto { Id = 10, Label = "mg" } },
                        },
                    ],
                    TargetSpecies = [new CodedValueDto { Id = 59, Label = "Porcins" }],
                    WithdrawalPeriods =
                    [
                        new WithdrawalPeriodDto
                        {
                            RouteOfAdministration = new CodedValueDto { Id = 9, Label = "Intramusculaire" },
                            TargetSpecies = new CodedValueDto { Id = 59, Label = "Porcins" },
                            Tissue = new CodedValueDto { Id = 14, Label = "viande et abats" },
                            Quantity = "0",
                            Unit = new CodedValueDto { Id = 22, Label = "jour" },
                            Comment = "",
                        },
                    ],
                    PharmaceuticalForm = new CodedValueDto { Id = 190, Label = "Solution injectable" },
                    SummaryOfProductCharacteristics = new SummaryOfProductCharacteristicsDto
                    {
                        IndicationsForUse = "Chez les porcelets",
                        SpecialPrecautionsForUse = new SpecialPrecautionsForUseDto { OtherPrecautions = "Aucune." },
                    },
                },
            ],
        };
    }
}
