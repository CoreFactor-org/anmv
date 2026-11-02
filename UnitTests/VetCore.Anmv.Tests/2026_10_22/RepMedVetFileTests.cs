using System.Text.Json;
using System.Xml.Linq;
using VetCore.Anmv.RepMedVet;
using VetCore.Anmv.RepMedVet.Json;
using VetCore.Anmv.RepMedVet.Xml;
using VetCore.Anmv.Tests.utils;

namespace VetCore.Anmv.Tests._2026_10_22;

public sealed class RepMedVetFileTests
{
    private static readonly Lazy<string> XML_CONTENT = new(() => UnitTestFileAccessor.GetXmlContent(AmnvFilesUnitTest.XML_RepMedVet_2026_10_22));
    private static readonly Lazy<MedecineRepositoryDto> REPOSITORY = new(() => RepMedVetFileHandler.DeserializeXmlString(XML_CONTENT.Value));

    [Fact]
    public void Deserialize_file_into_DTO()
    {
        // Arrange
        var xmlContent = XML_CONTENT.Value;

        // Act
        var result = RepMedVetFileHandler.DeserializeXmlString(xmlContent);

        // Assert
        Assert.Equal(DateTimeOffset.Parse("2026-10-22T05:03:13.1297425+02:00"), result.ExchangedDocument.IssueDatetime);
        Assert.Equal("REPMEDVET", result.ExchangedDocument.FileName);
        Assert.Equal("REPMEC", result.ExchangedDocument.TypeCode);
        Assert.Equal("MED 0.9", result.ExchangedDocument.ExchangeScenario);
        Assert.Equal("SIRET", result.Sender.IdParty.TypeId);
        Assert.Equal("13001202400100", result.Sender.IdParty.Value);
        Assert.Equal("ANMV", result.Sender.Name);

        var products = result.Products;
        Assert.Equal(2368, products.Count);
        Assert.Equal(4258, products.Sum(product => product.Drugs.Count));
        Assert.Equal(2396, products.Sum(product => product.AtcVetCodes.Count));
        Assert.Equal(3666, products.Sum(product => product.ActiveSubstances.Count));
        Assert.Equal(991, products.Sum(product => product.ActiveSubstances.Count(substance => substance.ActiveFraction != null)));
        Assert.Equal(4716, products.Sum(product => product.TargetSpecies.Count));
        Assert.Equal(7969, products.Sum(product => product.WithdrawalPeriods.Count));
        Assert.Equal(2360, products.Count(product => product.AuthorisationStatus != null));
        Assert.Equal(2360, products.Count(product => product.AuthorisationStatusDate.HasValue));
        Assert.Equal(2339, products.Count(product => product.QspUnit != null));
        Assert.Equal(1949, products.Count(product => product.SummaryOfProductCharacteristics != null));
        Assert.Equal(614, products.Count(product => product.IsSubjectToReporting == SubjectToReportingDto.Yes));
    }

    [Fact]
    public void Deserialize_should_read_every_part_of_a_product()
    {
        // Act
        var product = REPOSITORY.Value.Products[0];

        // Assert
        Assert.Equal(85, product.Product.Id);
        Assert.Equal("GLEPTOSIL", product.Product.Label);
        Assert.Equal("CEVA SANTE ANIMALE", product.MarketingAuthorisationHolder.Label);
        Assert.Equal(["QB03AC90"], product.AtcVetCodes);
        Assert.Equal(4, product.AuthorisationStatus.Id);
        Assert.Equal(new DateTime(2009, 7, 23), product.AuthorisationStatusDate);
        Assert.Equal(11, product.QspUnit.Id);
        Assert.Equal("mL", product.QspUnit.Label);
        Assert.Equal(SubjectToReportingDto.No, product.IsSubjectToReporting);

        var drug = Assert.Single(product.Drugs);
        Assert.Equal("03411113021061", drug.Id.GtinCode);
        Assert.Equal("GLEPTOSIL - Boîte de 1 flacon rigide de 250 mL", drug.Name);
        Assert.Equal("FR/V/7008561 5/1979", drug.AuthorisationNumber);
        Assert.Equal("Boîte de 1 flacon rigide de 250 mL", drug.Packaging);

        var substance = Assert.Single(product.ActiveSubstances);
        Assert.Equal("Gleptoferron", substance.ActiveSubstance.Label);
        Assert.Equal("532,6", substance.ActiveSubstanceConcentration.Quantity);
        Assert.Equal("mg", substance.ActiveSubstanceConcentration.Unit.Label);
        Assert.Equal("Fer (III) (sous forme de gleptoferron)", substance.ActiveFraction.Label);
        Assert.Equal("200", substance.ActiveFractionConcentration.Quantity);

        Assert.Equal("Porcins", Assert.Single(product.TargetSpecies).Label);
        var withdrawal = Assert.Single(product.WithdrawalPeriods);
        Assert.Equal("Intramusculaire", withdrawal.RouteOfAdministration.Label);
        Assert.Equal("Porcins", withdrawal.TargetSpecies.Label);
        Assert.Equal("viande et abats", withdrawal.Tissue.Label);
        Assert.Equal("0", withdrawal.Quantity);
        Assert.Equal("jour", withdrawal.Unit.Label);
        Assert.Equal(string.Empty, withdrawal.Comment);

        Assert.Equal("Solution injectable", product.PharmaceuticalForm.Label);
    }

    [Fact]
    public void Deserialize_should_read_the_whole_RCP()
    {
        // Act
        var rcp = REPOSITORY.Value.Products[0].SummaryOfProductCharacteristics;

        // Assert
        Assert.StartsWith("Chez les porcelets", rcp.IndicationsForUse);
        Assert.Equal("Aucune.", rcp.Contraindications);
        Assert.Equal("Aucune.", rcp.SpecialWarnings);
        Assert.StartsWith("Respecter strictement", rcp.SpecialPrecautionsForUse.SpecialPrecautionsForUseInAnimals);
        Assert.Equal("Sans objet.", rcp.SpecialPrecautionsForUse.SpecialPrecautionsByThePersonAdministering);
        Assert.Equal("Aucune.", rcp.SpecialPrecautionsForUse.OtherPrecautions);
        Assert.StartsWith("Porcins", rcp.AdverseEvents);
        Assert.False(string.IsNullOrEmpty(rcp.InteractionsWithOtherMedicinalProducts));
        Assert.False(string.IsNullOrEmpty(rcp.PosologyAndRoutesOfAdministration));
        Assert.False(string.IsNullOrEmpty(rcp.SpecialPrecautionsForStorage));
    }

    [Theory]
    [InlineData("03605874415683", "Comprimé", SubjectToReportingDto.Yes)] // CLAVASEPTIN P 62,5 MG
    [InlineData("03597132219013", "mL", SubjectToReportingDto.Yes)] // SHOTAPEN
    [InlineData("05414736007380", "Seringue", SubjectToReportingDto.Yes)] // AMPICLOX
    [InlineData("03597132316286", "g", SubjectToReportingDto.Yes)] // PULMODOX DOXYCYCLINE 50 PORC
    [InlineData("03411113021061", "mL", SubjectToReportingDto.No)] // GLEPTOSIL
    public void Deserialize_should_read_the_QSP_unit_and_the_reporting_flag(string gtin, string expectedQspUnit, SubjectToReportingDto expectedFlag)
    {
        // Act
        var product = REPOSITORY.Value.Products.Single(item => item.Drugs.Any(drug => drug.Id.GtinCode == gtin));

        // Assert
        Assert.Equal(expectedQspUnit, product.QspUnit.Label);
        Assert.Equal(expectedFlag, product.IsSubjectToReporting);
    }

    [Theory]
    [InlineData("CORTIZEME EMULSION POUR APPLICATION CUTANEE", "QD07CA03")]
    [InlineData("PREDNIDERM", "QD07CA03")]
    [InlineData("ORIBIOTIC", "QS02DC")]
    public void Deserialize_should_keep_a_reporting_flag_that_the_ATCvet_rule_of_the_specification_does_not_give(string name, string atcVetCode)
    {
        // Act
        var product = REPOSITORY.Value.Products.Single(item => item.Product.Label == name);

        // Assert : ces médicaments sont déclarables alors que leur code ne figure pas dans la liste de préfixes publiée par l'ANMV.
        Assert.Equal([atcVetCode], product.AtcVetCodes);
        Assert.Equal(SubjectToReportingDto.Yes, product.IsSubjectToReporting);
    }

    [Fact]
    public void Serialize_should_keep_every_element_of_the_file()
    {
        // Arrange
        var source = XDocument.Parse(XML_CONTENT.Value);

        // Act
        var serialized = XDocument.Parse(REPOSITORY.Value.SerializeToXml());

        // Assert : sans schéma public, cet aller-retour prouve qu'aucun élément, attribut ou texte du fichier n'échappe aux DTO.
        AssertSameElement(source.Root!, serialized.Root!);
    }

    [Fact]
    public void Json_conversion_should_keep_the_whole_content()
    {
        // Arrange
        var expected = REPOSITORY.Value.SerializeToXml();

        // Act
        var json = JsonSerializer.Serialize(REPOSITORY.Value.ToJsonDto());
        var result = JsonSerializer.Deserialize<MedecineRepositoryDtoJson>(json)!.ToXmlDto().SerializeToXml();

        // Assert
        Assert.Equal(expected, result);
    }

    [Fact]
    public void Validate_should_accept_the_file()
    {
        // Arrange
        var dto = REPOSITORY.Value.ToJsonDto();

        // Act
        var isValid = dto.Validate(out var errors);

        // Assert
        Assert.True(isValid, string.Join(Environment.NewLine, errors));
    }

    private static void AssertSameElement(XElement expected, XElement actual)
    {
        Assert.Equal(expected.Name.LocalName, actual.Name.LocalName);
        Assert.Equal(ReadAttributes(expected), ReadAttributes(actual));

        var expectedChildren = expected.Elements().ToArray();
        var actualChildren = actual.Elements().ToArray();
        Assert.True(expectedChildren.Length == actualChildren.Length, $"Children of '{expected.Name.LocalName}': {expectedChildren.Length} expected, {actualChildren.Length} found.");
        if (expectedChildren.Length == 0)
        {
            Assert.Equal(expected.Value, actual.Value);
        }

        for (var index = 0; index < expectedChildren.Length; index++)
        {
            AssertSameElement(expectedChildren[index], actualChildren[index]);
        }
    }

    private static string[] ReadAttributes(XElement element)
    {
        return element.Attributes()
            .Where(attribute => !attribute.IsNamespaceDeclaration)
            .Select(attribute => $"{attribute.Name.LocalName}={attribute.Value}")
            .ToArray();
    }
}
