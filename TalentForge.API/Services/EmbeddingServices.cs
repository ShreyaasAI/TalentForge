using System.Text.Json;


public class EmbeddingServices
{
    private readonly HttpClient _http;
    private readonly string _baseUrl;
private readonly bool _enabled;

public EmbeddingServices(HttpClient http, IConfiguration config)
{
    _http = http;
    _baseUrl = config["Ollama:BaseUrl"] ?? "http://localhost:11434";
    _enabled = config.GetValue<bool>("Ollama:Enabled", true);
}

public async Task<float[]> GetEmbeddingAsync(string text)
{
    if (!_enabled) return Array.Empty<float>();

    var response = await _http.PostAsJsonAsync($"{_baseUrl}/api/embeddings",
        new { model = "nomic-embed-text", prompt = text });
    var json = await response.Content.ReadFromJsonAsync<JsonElement>();
    return json.GetProperty("embedding").EnumerateArray().Select(x => x.GetSingle()).ToArray();
}
}