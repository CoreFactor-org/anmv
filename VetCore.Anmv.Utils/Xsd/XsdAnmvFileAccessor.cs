using System.Reflection;

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
    private static readonly Dictionary<AmnvFilesKey, string> _keyToResourceName = new()
    {
        { AmnvFilesKey.Descriptions_XSD_AMM, "VetCore.Anmv.Utils.Xsd.amm-vet-fr-v2-d.xsd" },
        { AmnvFilesKey.Data_XSD_AMM, "VetCore.Anmv.Utils.Xsd.amm-vet-fr-v2-v.xsd" },
    };

    /// <summary>
    /// Retrieve the matching embedded resource as a stream
    /// </summary>
    public static string GetXsdContent(this AmnvFilesKey key)
    {
        if (_keyToResourceName.TryGetValue(key, out var resourceName))
        {
            var assembly = Assembly.GetExecutingAssembly();
            using var stream = assembly.GetManifestResourceStream(resourceName);
            if (stream != null)
            {
                using var reader = new StreamReader(stream);
                return reader.ReadToEnd();
            }
        }

        throw new FileNotFoundException($"Embedded resource '{resourceName}' not found in the assembly. It means there is an error in Package configuration. Please report this to maintainers");
    }
}

// ReSharper restore InconsistentNaming