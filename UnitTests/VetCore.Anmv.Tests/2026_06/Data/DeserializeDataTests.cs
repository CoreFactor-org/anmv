using VetCore.Anmv.Tests.utils;
using VetCore.Anmv.Utils;

namespace VetCore.Anmv.Tests._2026_06.Data;

public sealed class DeserializeDataTests
{
    [Fact]
    public void Deserialize_data_into_DTO()
    {
        // Arrange
        var xmlContent = UnitTestFileAccessor.GetXmlContent(AmnvFilesUnitTest.XML_AMM_Data_2026_06);

        // Act
        var result = AnmvFileHandler.DeserializeDataXmlString(xmlContent);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(3212, result.MedicinalProducts.Count);
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
                CompositionCount = acc.CompositionCount + product.Compositions.Count,
                AtcvetCodeCount = acc.AtcvetCodeCount + product.AtcvetCodes.Count,
                ParagraphesRcpCount = acc.ParagraphesRcpCount + product.ParagraphesRcp.Count,
                VoiesAdminCount = acc.VoiesAdminCount + product.VoiesAdmin.Count,
                MdvCodesGtinCount = acc.MdvCodesGtinCount + product.MdvCodesGtin.Count,
                ModeleDestineVenteCount = acc.ModeleDestineVenteCount + product.ModeleDestineVente.Count,
            });

        Assert.Equal(4840, aggregated.CompositionCount);
        Assert.Equal(3241, aggregated.AtcvetCodeCount);
        Assert.Equal(94362, aggregated.ParagraphesRcpCount);
        Assert.Equal(9711, aggregated.VoiesAdminCount);
        Assert.Equal(15628, aggregated.MdvCodesGtinCount);
        Assert.Equal(15627, aggregated.ModeleDestineVenteCount);
    }
}
