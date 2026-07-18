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
    XML_AMM_Descriptions_2025_03,
    XML_AMM_Data_2025_03,
    // 2026_06
    XML_AMM_Descriptions_2026_06,
    XML_AMM_Data_2026_06,
}

public static class UnitTestFileAccessor
{
    /// <summary>
    /// The dictionary that map each enum with a file for unit testing. When adding a file,
    /// you should add it in this dictionary (and set the content action to 'copy if newer')
    /// </summary>
    private static readonly Dictionary<AmnvFilesUnitTest, UnitTestXmlFile> _keyToUnitTestFile = [];

    static UnitTestFileAccessor()
    {
        var testFolder = new DirectoryInfoWrapper(Path.Combine(AppDomain.CurrentDomain.BaseDirectory));

        // 2025_01_14
        var folder_xml_2025_01_14 = testFolder.GetDirectory("2025_01_14").GetDirectory("xml");
        Add(AmnvFilesUnitTest.XML_AMM_Descriptions_2025_01_14, folder_xml_2025_01_14, "amm-vet-fr-v2-d");
        Add(AmnvFilesUnitTest.XML_AMM_Data_2025_01_14, folder_xml_2025_01_14, "amm-vet-fr-v2-v");

        // 2025_03
        var folder_xml_2025_03 = testFolder.GetDirectory("2025_03").GetDirectory("xml");
        Add(AmnvFilesUnitTest.XML_AMM_Descriptions_2025_03, folder_xml_2025_03, "amm-vet-fr-v2-d");
        Add(AmnvFilesUnitTest.XML_AMM_Data_2025_03, folder_xml_2025_03, "amm-vet-fr-v2-v");

        // 2026_06
        var folderXml2026_06 = testFolder.GetDirectory("2026_06").GetDirectory("xml");
        Add(AmnvFilesUnitTest.XML_AMM_Descriptions_2026_06, folderXml2026_06, "amm-vet-fr-v2-d");
        Add(AmnvFilesUnitTest.XML_AMM_Data_2026_06, folderXml2026_06, "amm-vet-fr-v2-v");
    }

    /// <summary>
    /// Retrieve the matching unit test file or throw if not found
    /// </summary>
    public static IFileInfo GetFile(AmnvFilesUnitTest key)
    {
        if (_keyToUnitTestFile.TryGetValue(key, out var file))
        {
            return file.ZipFile;
        }
        throw new FileNotFoundException($"The file that should map key ${key} was not found. When adding a file, you should add it in the _keyToUnitTestFile dictionary and set the content action to 'copy if newer'");
    }

    /// <summary>
    /// Read the XML entry contained in the zip associated with a unit-test key.
    /// </summary>
    public static string GetXmlContent(AmnvFilesUnitTest key)
    {
        if (!_keyToUnitTestFile.TryGetValue(key, out var file))
        {
            throw new FileNotFoundException($"The file that should map key ${key} was not found. When adding a file, you should add it in the _keyToUnitTestFile dictionary and set the content action to 'copy if newer'");
        }

        using var zipStream = file.ZipFile.OpenRead();
        using var archive = new ZipArchive(zipStream, ZipArchiveMode.Read);
        return archive.ReadEntryAsString(file.XmlEntryName);
    }

    private static void Add(AmnvFilesUnitTest key, IDirectoryInfo folder, string xmlFileNameWithoutExtension)
    {
        _keyToUnitTestFile.Add(
            key,
            new UnitTestXmlFile(
                folder.GetFile($"{xmlFileNameWithoutExtension}.zip").EnsureExists(),
                $"{xmlFileNameWithoutExtension}.xml"));
    }

    private sealed record UnitTestXmlFile(IFileInfo ZipFile, string XmlEntryName);
}

// ReSharper restore InconsistentNaming
