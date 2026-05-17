using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace VClothes.Data;

/// <summary>
/// HTTP client for Supabase PostgREST API.
/// Handles all CRUD operations via REST.
/// </summary>
public class SupabaseClient
{
    private static readonly HttpClient _httpClient = new();
    private static readonly JsonSerializerOptions _jsonOptions = new()
    {
        PropertyNamingPolicy = JsonNamingPolicy.SnakeCaseLower,
        DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull,
        PropertyNameCaseInsensitive = true
    };

    static SupabaseClient()
    {
        _httpClient.BaseAddress = new Uri(SupabaseConfig.Url + "/rest/v1/");
        _httpClient.DefaultRequestHeaders.Add("apikey", SupabaseConfig.ApiKey);
        _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", SupabaseConfig.ApiKey);
        _httpClient.DefaultRequestHeaders.Add("Prefer", "return=representation");
        _httpClient.Timeout = TimeSpan.FromSeconds(10);
    }

    /// <summary>
    /// GET all rows from a table with optional query parameters.
    /// </summary>
    public static async Task<List<T>> GetAsync<T>(string table, string query = "")
    {
        var url = string.IsNullOrEmpty(query) ? table : $"{table}?{query}";
        var response = await _httpClient.GetAsync(url);
        response.EnsureSuccessStatusCode();
        var json = await response.Content.ReadAsStringAsync();
        return JsonSerializer.Deserialize<List<T>>(json, _jsonOptions) ?? new List<T>();
    }

    /// <summary>
    /// GET rows synchronously (for use in ViewModels without async).
    /// </summary>
    public static List<T> Get<T>(string table, string query = "")
    {
        return Task.Run(() => GetAsync<T>(table, query)).GetAwaiter().GetResult();
    }

    /// <summary>
    /// INSERT a new row and return the inserted row.
    /// </summary>
    public static async Task<T?> InsertAsync<T>(string table, object data)
    {
        var json = JsonSerializer.Serialize(data, _jsonOptions);
        var content = new StringContent(json, Encoding.UTF8, "application/json");
        var response = await _httpClient.PostAsync(table, content);
        response.EnsureSuccessStatusCode();
        var responseJson = await response.Content.ReadAsStringAsync();
        var results = JsonSerializer.Deserialize<List<T>>(responseJson, _jsonOptions);
        return results != null && results.Count > 0 ? results[0] : default;
    }

    /// <summary>
    /// INSERT synchronously.
    /// </summary>
    public static T? Insert<T>(string table, object data)
    {
        return Task.Run(() => InsertAsync<T>(table, data)).GetAwaiter().GetResult();
    }

    /// <summary>
    /// UPDATE rows matching a filter.
    /// </summary>
    public static async Task UpdateAsync(string table, string filter, object data)
    {
        var json = JsonSerializer.Serialize(data, _jsonOptions);
        var content = new StringContent(json, Encoding.UTF8, "application/json");
        var request = new HttpRequestMessage(HttpMethod.Patch, $"{table}?{filter}")
        {
            Content = content
        };
        var response = await _httpClient.SendAsync(request);
        response.EnsureSuccessStatusCode();
    }

    /// <summary>
    /// UPDATE synchronously.
    /// </summary>
    public static void Update(string table, string filter, object data)
    {
        Task.Run(() => UpdateAsync(table, filter, data)).GetAwaiter().GetResult();
    }

    /// <summary>
    /// DELETE rows matching a filter.
    /// </summary>
    public static async Task DeleteAsync(string table, string filter)
    {
        var response = await _httpClient.DeleteAsync($"{table}?{filter}");
        response.EnsureSuccessStatusCode();
    }

    /// <summary>
    /// DELETE synchronously.
    /// </summary>
    public static void Delete(string table, string filter)
    {
        Task.Run(() => DeleteAsync(table, filter)).GetAwaiter().GetResult();
    }

    /// <summary>
    /// RPC call (for stored procedures/functions).
    /// </summary>
    public static async Task<string> RpcAsync(string functionName, object? parameters = null)
    {
        var url = SupabaseConfig.Url + $"/rest/v1/rpc/{functionName}";
        var json = parameters != null ? JsonSerializer.Serialize(parameters, _jsonOptions) : "{}";
        var content = new StringContent(json, Encoding.UTF8, "application/json");

        var request = new HttpRequestMessage(HttpMethod.Post, url) { Content = content };
        request.Headers.Add("apikey", SupabaseConfig.ApiKey);
        request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", SupabaseConfig.ApiKey);

        using var client = new HttpClient();
        var response = await client.SendAsync(request);
        response.EnsureSuccessStatusCode();
        return await response.Content.ReadAsStringAsync();
    }

    /// <summary>
    /// Check if connection to Supabase is working.
    /// </summary>
    public static bool TestConnection()
    {
        try
        {
            var result = Task.Run(async () =>
            {
                var response = await _httpClient.GetAsync("roles?select=id&limit=1");
                return response.IsSuccessStatusCode;
            }).GetAwaiter().GetResult();
            return result;
        }
        catch
        {
            return false;
        }
    }
}
