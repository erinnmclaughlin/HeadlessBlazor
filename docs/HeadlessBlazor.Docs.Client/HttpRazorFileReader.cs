using System.Net.Http.Json;

namespace HeadlessBlazor.Docs.Client;

public class HttpRazorFileReader(HttpClient httpClient) : IRazorFileReader
{
    private readonly HttpClient _httpClient = httpClient;

    public async Task<string> ReadFileAsync(string fileName, CancellationToken cancellationToken)
    {
        var result = await _httpClient.GetAsync($"api/files/{fileName}", cancellationToken);

        if (result.IsSuccessStatusCode)
        {
            return await result.Content.ReadFromJsonAsync<string>(cancellationToken) ?? "";
        }

        return string.Empty;
    }
}