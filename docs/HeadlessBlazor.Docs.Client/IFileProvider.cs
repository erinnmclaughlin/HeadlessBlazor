using System.Net.Http.Json;

namespace HeadlessBlazor.Docs.Client;

public interface IFileProvider
{
    Task<string> GetFilesAsync(string fileName);
}

public class HttpFileProvider(HttpClient httpClient) : IFileProvider
{
    private readonly HttpClient _httpClient = httpClient;

    public async Task<string> GetFilesAsync(string fileName)
    {
        var result = await _httpClient.GetAsync($"api/files/{fileName}");

        if (result.IsSuccessStatusCode)
        {
            return await result.Content.ReadFromJsonAsync<string>() ?? "";
        }

        return string.Empty;
    }
}