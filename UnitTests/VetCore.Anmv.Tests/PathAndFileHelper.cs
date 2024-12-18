using PRF.Utils.CoreComponents.IO;

namespace VetCore.Anmv.Tests;

public static class PathAndFileHelper
{
    /// <summary>
    /// Ensure the file really exists on disk or throw
    /// </summary>
    public static IFileInfo EnsureExists(this IFileInfo file)
    {
        if (!file.ExistsExplicit)
        {
            throw new ArgumentException($"EnsureExists ERROR: the target file : {file.FullName} does not exist");
        }

        return file;
    }
    
}