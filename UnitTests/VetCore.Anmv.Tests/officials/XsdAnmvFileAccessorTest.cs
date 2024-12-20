using VetCore.Anmv.Utils.Xsd;

namespace VetCore.Anmv.Tests.officials;

public sealed class XsdAnmvFileAccessorTest
{
    [Fact]
    public void GetFile_Test_that_all_Enum_Values_has_been_defined()
    {
        //Arrange

        //Act
        foreach (var enumValue in Enum.GetValues<AmnvFilesKey>())
        {
            var res = enumValue.GetFile();
            Assert.NotNull(res);
        }

        //Assert
    }

    [Fact]
    public void GetFile_Test_that_undefined_Enum_Values_throw()
    {
        //Arrange
        var undefinedEnumMember = Enum.GetValues<AmnvFilesKey>().Min() - 1;

        //Act
        Assert.Throws<FileNotFoundException>(() => undefinedEnumMember.GetFile());

        //Assert
    }

}