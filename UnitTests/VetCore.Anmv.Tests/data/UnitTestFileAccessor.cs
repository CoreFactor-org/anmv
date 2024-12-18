using PRF.Utils.CoreComponents.IO;

namespace VetCore.Anmv.Tests.data;

// ReSharper disable InconsistentNaming
public enum AmnvFilesUnitTest
{
    XML_AMM_Data_2024_12_10,
    XML_AMM_Descriptions_2024_12_10,
    XSD_AMM_Data_2024_12_10,
    XSD_AMM_Descriptions_2024_12_10,
}

public static class UnitTestFileAccessor
{
    /// <summary>
    /// The dictionary that map each enum with a file for unit testing. When adding a file,
    /// you should add it in this dictionary (and set the content action to 'copy if newer')
    /// </summary>
    private static readonly Dictionary<AmnvFilesUnitTest, IFileInfo> _keyToUnitTestFile = [];

    static UnitTestFileAccessor()
    {
        var dataTestFolder = new DirectoryInfoWrapper(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "data"));
        var folder_2024_12_10 = dataTestFolder.GetDirectory("2024_12_10");

        _keyToUnitTestFile.Add(AmnvFilesUnitTest.XML_AMM_Data_2024_12_10, folder_2024_12_10.GetFile("amm-vet-fr-v2-v.xml").EnsureExists());
        _keyToUnitTestFile.Add(AmnvFilesUnitTest.XML_AMM_Descriptions_2024_12_10, folder_2024_12_10.GetFile("amm-vet-fr-v2-d.xml").EnsureExists());
        _keyToUnitTestFile.Add(AmnvFilesUnitTest.XSD_AMM_Data_2024_12_10, folder_2024_12_10.GetFile("amm-vet-fr-v2-v.xsd").EnsureExists());
        _keyToUnitTestFile.Add(AmnvFilesUnitTest.XSD_AMM_Descriptions_2024_12_10, folder_2024_12_10.GetFile("amm-vet-fr-v2-d.xsd").EnsureExists());
    }

    /// <summary>
    /// Retrieve the matching unit test file or throw if not found
    /// </summary>
    public static IFileInfo GetFile(AmnvFilesUnitTest key)
    {
        if (_keyToUnitTestFile.TryGetValue(key, out var file))
        {
            return file;
        }
        throw new FileNotFoundException($"The file that should map key ${key} was not found. When adding a file, you should add it in the _keyToUnitTestFile dictionary and set the content action to 'copy if newer'");
    }
}

// ReSharper restore InconsistentNaming