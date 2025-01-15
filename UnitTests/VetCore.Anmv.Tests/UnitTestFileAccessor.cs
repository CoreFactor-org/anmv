using PRF.Utils.CoreComponents.IO;

namespace VetCore.Anmv.Tests.data;

// ReSharper disable InconsistentNaming
public enum AmnvFilesUnitTest
{
    // 2024_12_10
    XML_AMM_Data_2024_12_10,
    XML_AMM_Data_short_2024_12_10,
    XML_AMM_Data_header_2024_12_10,
    XML_AMM_Descriptions_2024_12_10,
    XSD_AMM_Data_2024_12_10,
    XSD_AMM_Descriptions_2024_12_10,

    // 2025_01_14
    XML_AMM_Descriptions_2025_01_14,
    XML_AMM_Data_short_2025_01_14,
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

        // 2024_12_10
        var folder_xml_2024_12_10 = testFolder.GetDirectory("2024_12_10").GetDirectory("xml");
        var folder_xsd_2024_12_10 = testFolder.GetDirectory("2024_12_10").GetDirectory("xsd");
        _keyToUnitTestFile.Add(AmnvFilesUnitTest.XML_AMM_Data_2024_12_10, folder_xml_2024_12_10.GetFile("amm-vet-fr-v2-v.xml").EnsureExists());
        _keyToUnitTestFile.Add(AmnvFilesUnitTest.XML_AMM_Data_short_2024_12_10, folder_xml_2024_12_10.GetFile("amm-vet-fr-v2-v_short.xml").EnsureExists());
        _keyToUnitTestFile.Add(AmnvFilesUnitTest.XML_AMM_Data_header_2024_12_10, folder_xml_2024_12_10.GetFile("amm-vet-fr-v2-v_headers.xml").EnsureExists());
        _keyToUnitTestFile.Add(AmnvFilesUnitTest.XML_AMM_Descriptions_2024_12_10, folder_xml_2024_12_10.GetFile("amm-vet-fr-v2-d.xml").EnsureExists());
        _keyToUnitTestFile.Add(AmnvFilesUnitTest.XSD_AMM_Data_2024_12_10, folder_xsd_2024_12_10.GetFile("amm-vet-fr-v2-v.xsd").EnsureExists());
        _keyToUnitTestFile.Add(AmnvFilesUnitTest.XSD_AMM_Descriptions_2024_12_10, folder_xsd_2024_12_10.GetFile("amm-vet-fr-v2-d.xsd").EnsureExists());

        // 2025_01_14
        var folder_xml_2025_01_14 = testFolder.GetDirectory("2025_01_14").GetDirectory("xml");
        _keyToUnitTestFile.Add(AmnvFilesUnitTest.XML_AMM_Descriptions_2025_01_14, folder_xml_2025_01_14.GetFile("amm-vet-fr-v2-d.xml").EnsureExists());
        _keyToUnitTestFile.Add(AmnvFilesUnitTest.XML_AMM_Data_short_2025_01_14, folder_xml_2025_01_14.GetFile("amm-vet-fr-v2-v_extract.xml").EnsureExists());
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