using System.IO.Compression;
using PRF.Utils.CoreComponents.Extensions;
using PRF.Utils.CoreComponents.IO;

namespace VetCore.Anmv.Tests.utils;

// ReSharper disable InconsistentNaming
public enum AmnvFilesUnitTest
{
    // 2025_01_14
    XML_AMM_Descriptions_2025_01_14,
    XML_AMM_Data_2025_01_14,
    // 2025_02_17
    XML_AMM_Descriptions_2025_02_17,
    XML_AMM_Data_2025_02_17,
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
        var testFolder = new DirectoryInfoWrapper(Path.Combine(AppDomain.CurrentDomain.BaseDirectory));

        // 2025_01_14
        var folder_xml_2025_01_14 = testFolder.GetDirectory("2025_01_14").GetDirectory("xml");
        _keyToUnitTestFile.Add(AmnvFilesUnitTest.XML_AMM_Descriptions_2025_01_14, folder_xml_2025_01_14.GetFile("amm-vet-fr-v2-d.zip").EnsureExists());
        _keyToUnitTestFile.Add(AmnvFilesUnitTest.XML_AMM_Data_2025_01_14, folder_xml_2025_01_14.GetFile("amm-vet-fr-v2-v.zip").EnsureExists());

        // 2025_02_17
        var folder_xml_2025_02_17 = testFolder.GetDirectory("2025_02_17").GetDirectory("xml");
        _keyToUnitTestFile.Add(AmnvFilesUnitTest.XML_AMM_Descriptions_2025_02_17, folder_xml_2025_02_17.GetFile("amm-vet-fr-v2-d.zip").EnsureExists());
        _keyToUnitTestFile.Add(AmnvFilesUnitTest.XML_AMM_Data_2025_02_17, folder_xml_2025_02_17.GetFile("amm-vet-fr-v2-v.zip").EnsureExists());
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