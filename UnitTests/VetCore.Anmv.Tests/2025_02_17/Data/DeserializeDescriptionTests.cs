using VetCore.Anmv.Tests.utils;
using VetCore.Anmv.Utils;

namespace VetCore.Anmv.Tests._2025_02_17.Data;

public sealed class DeserializeDataTests
{
    [Fact]
    public void Deserialize_data_into_DTO()
    {
        //Arrange
        var file = UnitTestFileAccessor
            .GetFile(AmnvFilesUnitTest.XML_AMM_Data_2025_02_17)
            .ToFileInfo();

        //Act
        var res = AnmvFileHandler.DeserializeDataFile(file);

        //Assert
        Assert.NotNull(res);
        Assert.Equal(3098, res.MedicinalProducts.Count);
        Assert.Equal(DateTime.Parse("2025-02-11T09:30:38.0000000"), res.Informations.DateJeuDeDonnees);
        var aggregated = res.MedicinalProducts.Aggregate(
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
                AtcvetCodeCount = acc.AtcvetCodeCount + product.AtcvetCode.Count,
                ParagraphesRcpCount = acc.ParagraphesRcpCount + product.ParagraphesRcp.Count,
                VoiesAdminCount = acc.VoiesAdminCount + product.VoiesAdmin.Count,
                MdvCodesGtinCount = acc.MdvCodesGtinCount + product.MdvCodesGtin.Count,
                ModeleDestineVenteCount = acc.ModeleDestineVenteCount + product.ModeleDestineVente.Count,
            });

        // count aggregated total
        Assert.Equal(4579, aggregated.CompositionCount);
        Assert.Equal(3138, aggregated.AtcvetCodeCount);
        Assert.Equal(87492, aggregated.ParagraphesRcpCount);
        Assert.Equal(9499, aggregated.VoiesAdminCount);
        Assert.Equal(14922, aggregated.MdvCodesGtinCount);
        Assert.Equal(14922, aggregated.ModeleDestineVenteCount);
    }
}