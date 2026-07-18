using VetCore.Anmv.Tests.utils;
using VetCore.Anmv.Utils;
using VetCore.Anmv.Xml.Data;

namespace VetCore.Anmv.Tests._2026_06.Data;

public class JsonValidationTests
{
    [Fact]
    public void Deserialize_data_then_convert_toJson_and_count_values()
    {
        // Arrange
        var xmlContent = UnitTestFileAccessor.GetXmlContent(AmnvFilesUnitTest.XML_AMM_Data_2026_06);

        // Act
        var result = AnmvFileHandler.DeserializeDataXmlString(xmlContent).ToJsonDto();

        // Assert
        Assert.NotNull(result);
        Assert.Equal(3212, result.MedicinalProducts.Length);
        Assert.Equal(DateTime.Parse("2026-06-04T09:31:00.0000000"), result.Informations.DateJeuDeDonnees);

        var aggregated = result.MedicinalProducts.Aggregate(
            new
            {
                CompositionCount = 0,
                AtcvetCodeCount = 0,
                ParagraphesRcpCount = 0,
                VoiesAdminCount = 0,
                MdvCodesGtinCount = 0,
                ModeleDestineVenteCount = 0,
            },
            (acc, product) => new
            {
                CompositionCount = acc.CompositionCount + product.Compositions.Length,
                AtcvetCodeCount = acc.AtcvetCodeCount + product.AtcvetCodes.Length,
                ParagraphesRcpCount = acc.ParagraphesRcpCount + product.ParagraphesRcp.Length,
                VoiesAdminCount = acc.VoiesAdminCount + product.VoiesAdmin.Length,
                MdvCodesGtinCount = acc.MdvCodesGtinCount + product.MdvCodesGtin.Length,
                ModeleDestineVenteCount = acc.ModeleDestineVenteCount + product.ModeleDestineVente.Length,
            });

        Assert.Equal(4840, aggregated.CompositionCount);
        Assert.Equal(3241, aggregated.AtcvetCodeCount);
        Assert.Equal(94362, aggregated.ParagraphesRcpCount);
        Assert.Equal(9717, aggregated.VoiesAdminCount);
        Assert.Equal(15628, aggregated.MdvCodesGtinCount);
        Assert.Equal(15628, aggregated.ModeleDestineVenteCount);
    }

    [Fact]
    public void Deserialize_data_then_convert_toJson_and_validate_every_nested_dto()
    {
        // Arrange
        var xmlContent = UnitTestFileAccessor.GetXmlContent(AmnvFilesUnitTest.XML_AMM_Data_2026_06);

        // Act
        var result = AnmvFileHandler.DeserializeDataXmlString(xmlContent).ToJsonDto();

        // Assert
        foreach (var medicinalProduct in result.MedicinalProducts)
        {
            foreach (var composition in medicinalProduct.Compositions)
            {
                Assert.True(composition.IsDtoValidAccordingToAttributes(),
                    $"INVALID JSON medicinal product Num={medicinalProduct.Num} => Compositions : {composition.Sa.TermSa}");
            }

            foreach (var voie in medicinalProduct.VoiesAdmin)
            {
                Assert.True(voie.IsDtoValidAccordingToAttributes(),
                    $"INVALID JSON medicinal product Num={medicinalProduct.Num} => VoiesAdmin : {voie.TermDenr}");
            }

            foreach (var code in medicinalProduct.AtcvetCodes)
            {
                Assert.True(code.IsDtoValidAccordingToAttributes(),
                    $"INVALID JSON medicinal product Num={medicinalProduct.Num} => AtcvetCodes : {code}");
            }

            foreach (var mdvGtin in medicinalProduct.MdvCodesGtin)
            {
                Assert.True(mdvGtin.IsDtoValidAccordingToAttributes(),
                    $"INVALID JSON medicinal product Num={medicinalProduct.Num} => MdvCodesGtin : {mdvGtin.CodeGtin}");
            }

            foreach (var modeleDestineVente in medicinalProduct.ModeleDestineVente)
            {
                Assert.True(modeleDestineVente.IsDtoValidAccordingToAttributes(),
                    $"INVALID JSON medicinal product Num={medicinalProduct.Num} => ModeleDestineVente : {modeleDestineVente.LibCondp}");
            }

            foreach (var paragraph in medicinalProduct.ParagraphesRcp)
            {
                Assert.True(paragraph.IsDtoValidAccordingToAttributes(),
                    $"INVALID JSON medicinal product Num={medicinalProduct.Num} => ParagraphesRcp : {paragraph.TermTitre}");
            }

            Assert.True(medicinalProduct.IsDtoValidAccordingToAttributes(),
                $"INVALID JSON medicinal product Num={medicinalProduct.Num}");
        }
    }

    [Fact]
    public void Deserialize_data_then_convert_toJson_and_validate()
    {
        // Arrange
        var xmlContent = UnitTestFileAccessor.GetXmlContent(AmnvFilesUnitTest.XML_AMM_Data_2026_06);

        // Act
        var result = AnmvFileHandler.DeserializeDataXmlString(xmlContent).ToJsonDto();

        // Assert
        Assert.True(result.IsDtoValidAccordingToAttributes());
    }

    [Fact]
    public void GetMax_VoieAdministration_QteTa_max_length()
    {
        // Arrange
        var xmlContent = UnitTestFileAccessor.GetXmlContent(AmnvFilesUnitTest.XML_AMM_Data_2026_06);

        // Act
        var result = AnmvFileHandler.DeserializeDataXmlString(xmlContent).ToJsonDto();
        var allVoiesAdmin = result.MedicinalProducts.SelectMany(product => product.VoiesAdmin).ToArray();

        // Assert
        Assert.Equal(3212, result.MedicinalProducts.Length);
        Assert.Equal(9717, allVoiesAdmin.Length);
        Assert.Equal(5, allVoiesAdmin.Select(voie => voie.QteTa?.Length ?? 0).Max());
    }

    [Fact]
    public void GetMax_VoieAdministration_Commentaire_max_length()
    {
        // Arrange
        var xmlContent = UnitTestFileAccessor.GetXmlContent(AmnvFilesUnitTest.XML_AMM_Data_2026_06);

        // Act
        var result = AnmvFileHandler.DeserializeDataXmlString(xmlContent).ToJsonDto();
        var allVoiesAdmin = result.MedicinalProducts.SelectMany(product => product.VoiesAdmin).ToArray();

        // Assert
        Assert.Equal(3212, result.MedicinalProducts.Length);
        Assert.Equal(9717, allVoiesAdmin.Length);
        Assert.Equal(444, allVoiesAdmin.Select(voie => voie.Commentaire?.Length ?? 0).Max());
    }
}
