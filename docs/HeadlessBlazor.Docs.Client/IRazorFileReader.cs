namespace HeadlessBlazor.Docs.Client;

public interface IRazorFileReader
{
    Task<string> ReadFileAsync(string fileName, CancellationToken cancellationToken = default);
}
