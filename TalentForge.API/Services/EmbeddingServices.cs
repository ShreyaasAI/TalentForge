using System.Text.Json;


public class EmbeddingServices
{
    private readonly HttpClient _http;
    private readonly string _baseUrl;

    public EmbeddingServices(HttpClient http, IConfiguration config)
    {
        _http = http;
        _baseUrl = config["Ollama:BaseUrl"] ?? "http://localhost:11434";
    }

    public async Task<float[]> GetEmbeddingAsync(string text)
    {
       var response = await _http.PostAsJsonAsync($"{_baseUrl}/api/embeddings",
            new { model = "nomic-embed-text", prompt = text });
        var json = await response.Content.ReadFromJsonAsync<JsonElement>();
        return json.GetProperty("embedding").EnumerateArray().Select(x => x.GetSingle()).ToArray();
    }
}