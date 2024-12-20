namespace VetCore.Anmv.Utils.Xsd;

// ReSharper disable InconsistentNaming
/// <summary>
/// Available AMNV xsd file
/// </summary>
public enum AmnvFilesKey
{
    /// <summary>
    /// Data file XSD
    /// </summary>
    Data_XSD_AMM,

    /// <summary>
    /// Description (term reference) xsd file
    /// </summary>
    Descriptions_XSD_AMM,
}

/// <summary>
/// Provide access to the provided official xsd files for validation
/// </summary>
public static class XsdAnmvFileAccessor
{
    /// <summary>
    /// The dictionary that map each enum with the file path
    /// </summary>
    private static readonly Dictionary<AmnvFilesKey, string> _keyToUnitTestFile = new()
    {
        { AmnvFilesKey.Descriptions_XSD_AMM, "amm-vet-fr-v3-d.xsd" },
    };

    /// <summary>
    /// Retrieve the matching unit test file or throw if not found
    /// </summary>
    public static FileInfo GetFile(this AmnvFilesKey key)
    {
        if (_keyToUnitTestFile.TryGetValue(key, out var fileName))
        {
            var file = new FileInfo(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Xsd", fileName));
            if (file.Exists)
            {
                return file;
            }
        }

        // else default to an exception :
        throw new FileNotFoundException($"The file that should map key ${key} was not found. It means there is an error in Package configuration. Please report this to maintainers'");
    }
}

// ReSharper restore InconsistentNaming