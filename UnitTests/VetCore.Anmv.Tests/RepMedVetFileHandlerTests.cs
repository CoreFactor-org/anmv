using System.Text;
using VetCore.Anmv.RepMedVet;
using VetCore.Anmv.RepMedVet.Json;
using VetCore.Anmv.RepMedVet.Xml;

namespace VetCore.Anmv.Tests;

public sealed class RepMedVetFileHandlerTests
{
    private const string VALID_XML =
        """
        <?xml version="1.0" encoding="UTF-8"?>
        <MedecineRepository xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" xmlns:xsd="http://www.w3.org/2001/XMLSchema">
          <ACExchangedDocumentSimple>
            <IssueDatetime>2026-10-22T05:03:13.1297425+02:00</IssueDatetime>
            <FileName>REPMEDVET</FileName>
            <TypeCode>REPMEC</TypeCode>
            <ExchangeScenario>MED 0.9</ExchangeScenario>
          </ACExchangedDocumentSimple>
          <Sender>
            <IDParty TypeID="SIRET">13001202400100</IDParty>
            <Name>ANMV</Name>
          </Sender>
          <Product>
            <Product ID="85">GLEPTOSIL</Product>
            <MarketingAuthorisationHolder ID="1659">CEVA SANTE ANIMALE</MarketingAuthorisationHolder>
            <AtcVetCode>QB03AC90</AtcVetCode>
            <IsSubjectToReporting>1</IsSubjectToReporting>
            <Drug>
              <ID GTINCode="03411113021061"/>
              <Name>GLEPTOSIL - Flacon</Name>
              <AuthorisationNumber/>
              <Packaging>Flacon</Packaging>
            </Drug>
            <PharmaceuticalForm ID="190">Solution injectable</PharmaceuticalForm>
          </Product>
        </MedecineRepository>
        """;

    [Fact]
    public void DeserializeXmlString_ShouldReadAValidDocument()
    {
        // Act
        var result = RepMedVetFileHandler.DeserializeXmlString(VALID_XML);

        // Assert
        var product = Assert.Single(result.Products);
        Assert.Equal(SubjectToReportingDto.Yes, product.IsSubjectToReporting);
        Assert.Null(product.AuthorisationStatusDate);
        Assert.Null(product.QspUnit);
        Assert.Equal(string.Empty, Assert.Single(product.Drugs).AuthorisationNumber);
    }

    [Fact]
    public void DeserializeStream_ShouldLeaveTheStreamOpen()
    {
        // Arrange
        using var stream = new MemoryStream(Encoding.UTF8.GetBytes(VALID_XML));

        // Act
        var result = RepMedVetFileHandler.DeserializeStream(stream);

        // Assert
        Assert.Single(result.Products);
        Assert.True(stream.CanRead);
    }

    [Fact]
    public void DeserializeFile_ShouldReadTheFile()
    {
        // Arrange
        var file = new FileInfo(Path.GetTempFileName());
        try
        {
            File.WriteAllText(file.FullName, VALID_XML, new UTF8Encoding(false));

            // Act
            var result = RepMedVetFileHandler.DeserializeFile(file);

            // Assert
            Assert.Equal("GLEPTOSIL", Assert.Single(result.Products).Product.Label);
        }
        finally
        {
            file.Delete();
        }
    }

    [Theory]
    [InlineData("<IsSubjectToReporting>1</IsSubjectToReporting>", "<IsSubjectToReporting>1</IsSubjectToReporting><NewField>x</NewField>", "NewField")]
    [InlineData("<Drug>", "<Drug Scope=\"FR\">", "Scope")]
    [InlineData("<Name>ANMV</Name>", "<Name>ANMV</Name>stray text", "Unexpected text")]
    public void Deserialize_ShouldRefuseAFormatItDoesNotKnow(string fragment, string replacement, string expectedMessage)
    {
        // Arrange
        var xml = VALID_XML.Replace(fragment, replacement);

        // Act
        var exception = Assert.Throws<RepMedVetFormatException>(() => RepMedVetFileHandler.DeserializeXmlString(xml));

        // Assert
        Assert.Contains(expectedMessage, exception.Message);
    }

    [Theory]
    [InlineData("<IsSubjectToReporting>1<", "<IsSubjectToReporting>2<")]
    [InlineData("<Product ID=\"85\">", "<Product ID=\"abc\">")]
    [InlineData("<IssueDatetime>2026-10-22T05:03:13.1297425+02:00<", "<IssueDatetime>22/10/2026<")]
    [InlineData("MedecineRepository", "OtherRepository")]
    public void Deserialize_ShouldRefuseAnUnreadableDocument(string fragment, string replacement)
    {
        // Arrange
        var xml = VALID_XML.Replace(fragment, replacement);

        // Act / Assert
        Assert.Throws<RepMedVetFormatException>(() => RepMedVetFileHandler.DeserializeXmlString(xml));
    }

    [Fact]
    public void Deserialize_ShouldRefuseADtd()
    {
        // Arrange
        var xml = VALID_XML.Replace("<MedecineRepository ", "<!DOCTYPE MedecineRepository [<!ENTITY name \"value\">]><MedecineRepository ");

        // Act / Assert
        Assert.Throws<RepMedVetFormatException>(() => RepMedVetFileHandler.DeserializeXmlString(xml));
    }

    [Theory]
    [InlineData("")]
    [InlineData("not xml")]
    public void Deserialize_ShouldRefuseSomethingElseThanXml(string content)
    {
        // Act / Assert
        Assert.Throws<RepMedVetFormatException>(() => RepMedVetFileHandler.DeserializeXmlString(content));
    }

    [Fact]
    public void Validate_ShouldAcceptAValidDocument()
    {
        // Arrange
        var dto = RepMedVetFileHandler.DeserializeXmlString(VALID_XML).ToJsonDto();

        // Act
        var isValid = dto.Validate(out var errors);

        // Assert
        Assert.True(isValid);
        Assert.Empty(errors);
    }

    [Theory]
    [InlineData("0341111302106")]
    [InlineData("0341111302106A")]
    [InlineData("")]
    public void Validate_ShouldRefuseAnInvalidGtin(string gtin)
    {
        // Arrange
        var dto = RepMedVetFileHandler.DeserializeXmlString(VALID_XML).ToJsonDto();
        dto.Products[0].Drugs[0].GtinCode = gtin;

        // Act
        var isValid = dto.Validate(out var errors);

        // Assert
        Assert.False(isValid);
        Assert.Contains("$.Products[0].Drugs[0]", Assert.Single(errors));
    }

    [Fact]
    public void Validate_ShouldRefuseAProductWithoutPresentation()
    {
        // Arrange
        var dto = RepMedVetFileHandler.DeserializeXmlString(VALID_XML).ToJsonDto();
        dto.Products[0].Drugs = [];

        // Act
        var isValid = dto.Validate(out var errors);

        // Assert
        Assert.False(isValid);
        Assert.Contains("$.Products[0]", Assert.Single(errors));
    }

    [Fact]
    public void Validate_ShouldRefuseAMissingRequiredPart()
    {
        // Arrange
        var dto = RepMedVetFileHandler.DeserializeXmlString(VALID_XML).ToJsonDto();
        dto.Sender = null;
        dto.Products[0].PharmaceuticalForm = null;

        // Act
        var isValid = dto.Validate(out var errors);

        // Assert
        Assert.False(isValid);
        Assert.Equal(2, errors.Count);
    }

    [Fact]
    public void Validate_ShouldRefuseADocumentWithoutProduct()
    {
        // Arrange
        var dto = RepMedVetFileHandler.DeserializeXmlString(VALID_XML).ToJsonDto();
        dto.Products = [];

        // Act
        var isValid = dto.Validate(out _);

        // Assert
        Assert.False(isValid);
    }

    [Fact]
    public void Validate_ShouldRefuseALabelAboveItsBound()
    {
        // Arrange
        var dto = RepMedVetFileHandler.DeserializeXmlString(VALID_XML).ToJsonDto();
        dto.Products[0].Product.Label = new string('X', RepMedVetConstants.LABEL_MAX_LENGTH + 1);

        // Act
        var isValid = dto.Validate(out var errors);

        // Assert
        Assert.False(isValid);
        Assert.Contains("$.Products[0].Product", Assert.Single(errors));
    }

    [Fact]
    public void Deserialize_ShouldRefuseAMissingReportingFlag()
    {
        // Arrange
        var xml = VALID_XML.Replace("<IsSubjectToReporting>1</IsSubjectToReporting>", string.Empty);

        // Act
        var exception = Assert.Throws<RepMedVetFormatException>(() => RepMedVetFileHandler.DeserializeXmlString(xml));

        // Assert
        Assert.Contains("IsSubjectToReporting", exception.Message);
    }

    [Fact]
    public void Validate_ShouldRefuseANullEntry()
    {
        // Arrange
        var dto = RepMedVetFileHandler.DeserializeXmlString(VALID_XML).ToJsonDto();
        dto.Products[0].Drugs = [null!];

        // Act
        var isValid = dto.Validate(out var errors);

        // Assert
        Assert.False(isValid);
        Assert.Contains("$.Products[0].Drugs[0]", Assert.Single(errors));
    }

    [Fact]
    public void Handler_ShouldRefuseNullArguments()
    {
        // Arrange
        var dto = (MedecineRepositoryDto)null!;
        var jsonDto = (MedecineRepositoryDtoJson)null!;

        // Act / Assert
        Assert.Throws<ArgumentNullException>(() => RepMedVetFileHandler.DeserializeXmlString(null!));
        Assert.Throws<ArgumentNullException>(() => RepMedVetFileHandler.DeserializeStream(null!));
        Assert.Throws<ArgumentNullException>(() => RepMedVetFileHandler.DeserializeFile(null!));
        Assert.Throws<ArgumentNullException>(() => dto.SerializeToXml());
        Assert.Throws<ArgumentNullException>(() => jsonDto.Validate(out _));
    }
}
