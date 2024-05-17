using HeadlessBlazor.Docs.Client;

namespace HeadlessBlazor.Docs;

public class DirectoryFileProvider : IFileProvider
{
    public async Task<string> GetFilesAsync(string fileName)
    {
        var match = Directory.EnumerateFiles(AppDomain.CurrentDomain.BaseDirectory, fileName, SearchOption.AllDirectories).SingleOrDefault();

        if (match is not null)
            return await File.ReadAllTextAsync(match);

        return string.Empty;
    }
}
