using HeadlessBlazor.Docs.Client;

namespace HeadlessBlazor.Docs;

public class DirectoryFileReader : IRazorFileReader
{
    public async Task<string> ReadFileAsync(string fileName, CancellationToken cancellationToken)
    {
        var match = Directory.EnumerateFiles(AppDomain.CurrentDomain.BaseDirectory, fileName, SearchOption.AllDirectories).SingleOrDefault();

        if (match is not null)
            return await File.ReadAllTextAsync(match, cancellationToken);

        return string.Empty;
    }
}
