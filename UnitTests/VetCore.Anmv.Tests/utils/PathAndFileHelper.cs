using PRF.Utils.CoreComponents.IO;

namespace VetCore.Anmv.Tests.utils;

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

    /// <summary>
    /// Convert a PRF.IFileInfo to microsoft .net FileInfo
    /// </summary>
    public static FileInfo ToFileInfo(this IFileInfo file)
    {
        return new FileInfo(file.FullName);
    }

    public static ITempDirectory CreateTempUnitTestDirectory()
    {
        var appDataPath = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
        return new TempDirectory(new DirectoryInfoWrapper(Path.Combine(appDataPath, $"temp_unit_test_{Guid.NewGuid().ToString()[..8]}")));
    }

    private class TempDirectory : ITempDirectory
    {
        public TempDirectory(IDirectoryInfo tempDirectory)
        {
            tempDirectory.CreateIfNotExists();
            Current = tempDirectory;
        }

        public IDirectoryInfo Current { get; }

        public void Dispose()
        {
            Current.DeleteIfExistAndWaitDeletion(TimeSpan.FromSeconds(5));
        }
    }
}

public interface ITempDirectory : IDisposable
{
    IDirectoryInfo Current { get; }
}