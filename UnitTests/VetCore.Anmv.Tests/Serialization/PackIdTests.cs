using System.Text.Json;
using System.Xml.Serialization;
using VetCore.Anmv.Json.Data;
using VetCore.Anmv.Tests.utils;
using VetCore.Anmv.Utils;
using VetCore.Anmv.Utils.Xsd;
using VetCore.Anmv.Xml.Data;

namespace VetCore.Anmv.Tests.Serialization;

public sealed class PackIdTests
{
    [Theory]
    [InlineData("1EC2116F-84FF-AEBA-E063-0E2A11ACBC")]
    [InlineData("1EC2116F-84FE-AEBA-E063-0E2A11ACBC37")]
    [InlineData("source-package-42")]
    [InlineData(" source-package-42 ")]
    [InlineData("00000000-0000-0000-0000-000000000000")]
    [InlineData("")]
    [InlineData(null)]
    public void XmlAndJsonRoundTrip_PreservesSourcePackId(string? packId)
    {
        var source = new MdvCodesGtinDto { LibMod = "Boîte", PackId = packId };
        var serializer = new XmlSerializer(typeof(MdvCodesGtinDto));
        using var writer = new StringWriter();
        serializer.Serialize(writer, source);
        using var reader = new StringReader(writer.ToString());

        var xml = Assert.IsType<MdvCodesGtinDto>(serializer.Deserialize(reader));
        var json = JsonSerializer.Deserialize<MdvCodesGtinDtoJson>(JsonSerializer.Serialize(xml.ToJsonDto()));

        Assert.Equal(packId, xml.PackId);
        Assert.NotNull(json);
        Assert.Equal(packId, json.PackId);
        Assert.Equal(packId, json.ToXmlDto().PackId);
    }

    [Fact]
    public void OctoberArchive_PreservesAllSourcePackIdsIncludingIncompleteIdentifier()
    {
        var xml = UnitTestFileAccessor.GetXmlContent(AmnvFilesUnitTest.XML_AMM_Data_2026_10_15);

        var data = AnmvFileHandler.DeserializeDataXmlString(xml);

        Assert.NotNull(data);
        Assert.Equal(3221, data.MedicinalProducts.Count);
        Assert.Equal(new DateTime(2026, 10, 15, 9, 30, 4), data.Informations.DateJeuDeDonnees);
        var product = Assert.Single(data.MedicinalProducts, item => item.Num == "0888699");
        var package = Assert.Single(product.MdvCodesGtin, item => item.NumAmm == "EU/2/21/270/041");
        Assert.Equal("Boîte en carton contenant 5 comprimés", package.LibMod);
        Assert.Equal("1EC2116F-84FF-AEBA-E063-0E2A11ACBC", package.PackId);
        var sourceIds = System.Xml.Linq.XDocument.Parse(xml).Descendants("pack-id").Select(element => element.Value);
        var parsedIds = data.MedicinalProducts.SelectMany(item => item.MdvCodesGtin).Where(item => item.PackId != null).Select(item => item.PackId);
        Assert.Equal(sourceIds, parsedIds);
    }

    [Fact]
    public void OctoberProduct_XsdDoesNotRequireGuidForPackId()
    {
        var xml = UnitTestFileAccessor.GetXmlContent(AmnvFilesUnitTest.XML_AMM_Data_2026_10_15);
        var document = System.Xml.Linq.XDocument.Parse(xml);
        var product = document.Root!.Elements("medicinal-product").Single(item => item.Element("num")!.Value == "0888699");
        var selected = new System.Xml.Linq.XElement("medicinal-product-group", document.Root.Element("Informations"), product);

        var result = AnmvFileHandler.ValidateXml(selected.ToString(), AmnvFilesKey.Data_XSD_AMM);

        Assert.True(result.Errors.Count == 0 && result.Warnings.Count == 0, result.PrintErrorsAndWarnings(Environment.NewLine));
    }

    [Fact]
    public void OctoberProduct_DtoValidationAcceptsSourcePackId()
    {
        var data = AnmvFileHandler.DeserializeDataXmlString(UnitTestFileAccessor.GetXmlContent(AmnvFilesUnitTest.XML_AMM_Data_2026_10_15))!;
        var descriptions = AnmvFileHandler.DeserializeDescriptionXmlString(UnitTestFileAccessor.GetXmlContent(AmnvFilesUnitTest.XML_AMM_Descriptions_2026_06))!;
        data.MedicinalProducts = [Assert.Single(data.MedicinalProducts, item => item.Num == "0888699")];

        var valid = data.ValidateDataWithRelatedDescription(descriptions, out var errors);

        Assert.True(valid, errors.PrintErrors(Environment.NewLine));
    }
}
