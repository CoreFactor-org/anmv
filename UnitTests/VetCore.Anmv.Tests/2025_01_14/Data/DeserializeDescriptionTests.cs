using VetCore.Anmv.Utils;
using VetCore.Anmv.Utils.Helpers;
using VetCore.Anmv.Xml.Data;

namespace VetCore.Anmv.Tests._2025_01_14.Data;

public sealed class DeserializeDataTests
{
    [Fact]
    public void Deserialize_data_short()
    {
        //Arrange
        var file = UnitTestFileAccessor.GetFile(AmnvFilesUnitTest.XML_AMM_Data_short_2025_01_14);

        //Act
        var res = AnmvFileHandler.DeserializeDataFile(file.ToFileInfo());

        //Assert
        Assert.NotNull(res);
        Assert.Equal(101, res.MedicinalProducts.Count);
    }
}