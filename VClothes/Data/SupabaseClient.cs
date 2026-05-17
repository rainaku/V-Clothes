using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace VClothes.Data;

/// <summary>
/// HTTP client cho Supabase PostgREST API.
/// Xử lý tất cả thao tác CRUD qua REST.
/// </summary>
public class SupabaseClient
{
    private static readonly HttpClient _httpClient = new();
    private static readonly JsonSerializerOptions _tuyChonJson = new()
    {
        PropertyNamingPolicy = JsonNamingPolicy.SnakeCaseLower,
        DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull,
        PropertyNameCaseInsensitive = true
    };

    static SupabaseClient()
    {
        _httpClient.BaseAddress = new Uri(CauHinhSupabase.Url + "/rest/v1/");
        _httpClient.DefaultRequestHeaders.Add("apikey", CauHinhSupabase.ApiKey);
        _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", CauHinhSupabase.ApiKey);
        _httpClient.DefaultRequestHeaders.Add("Prefer", "return=representation");
        _httpClient.Timeout = TimeSpan.FromSeconds(10);
    }

    /// <summary>
    /// Lấy tất cả dòng từ bảng với tham số truy vấn tùy chọn.
    /// </summary>
    public static async Task<List<T>> LayDanhSachAsync<T>(string bang, string truyVan = "")
    {
        var url = string.IsNullOrEmpty(truyVan) ? bang : $"{bang}?{truyVan}";
        var response = await _httpClient.GetAsync(url);
        response.EnsureSuccessStatusCode();
        var json = await response.Content.ReadAsStringAsync();
        return JsonSerializer.Deserialize<List<T>>(json, _tuyChonJson) ?? new List<T>();
    }

    /// <summary>
    /// Lấy dòng đồng bộ (dùng trong ViewModel không async).
    /// </summary>
    public static List<T> LayDanhSach<T>(string bang, string truyVan = "")
    {
        return Task.Run(() => LayDanhSachAsync<T>(bang, truyVan)).GetAwaiter().GetResult();
    }

    /// <summary>
    /// Thêm mới một dòng và trả về dòng đã thêm.
    /// </summary>
    public static async Task<T?> ThemMoiAsync<T>(string bang, object duLieu)
    {
        var json = JsonSerializer.Serialize(duLieu, _tuyChonJson);
        var content = new StringContent(json, Encoding.UTF8, "application/json");
        var response = await _httpClient.PostAsync(bang, content);
        response.EnsureSuccessStatusCode();
        var responseJson = await response.Content.ReadAsStringAsync();
        var ketQua = JsonSerializer.Deserialize<List<T>>(responseJson, _tuyChonJson);
        return ketQua != null && ketQua.Count > 0 ? ketQua[0] : default;
    }

    /// <summary>
    /// Thêm mới đồng bộ.
    /// </summary>
    public static T? ThemMoi<T>(string bang, object duLieu)
    {
        return Task.Run(() => ThemMoiAsync<T>(bang, duLieu)).GetAwaiter().GetResult();
    }

    /// <summary>
    /// Cập nhật các dòng khớp bộ lọc.
    /// </summary>
    public static async Task CapNhatAsync(string bang, string boLoc, object duLieu)
    {
        var json = JsonSerializer.Serialize(duLieu, _tuyChonJson);
        var content = new StringContent(json, Encoding.UTF8, "application/json");
        var request = new HttpRequestMessage(HttpMethod.Patch, $"{bang}?{boLoc}")
        {
            Content = content
        };
        var response = await _httpClient.SendAsync(request);
        response.EnsureSuccessStatusCode();
    }

    /// <summary>
    /// Cập nhật đồng bộ.
    /// </summary>
    public static void CapNhat(string bang, string boLoc, object duLieu)
    {
        Task.Run(() => CapNhatAsync(bang, boLoc, duLieu)).GetAwaiter().GetResult();
    }

    /// <summary>
    /// Xóa các dòng khớp bộ lọc.
    /// </summary>
    public static async Task XoaAsync(string bang, string boLoc)
    {
        var response = await _httpClient.DeleteAsync($"{bang}?{boLoc}");
        response.EnsureSuccessStatusCode();
    }

    /// <summary>
    /// Xóa đồng bộ.
    /// </summary>
    public static void Xoa(string bang, string boLoc)
    {
        Task.Run(() => XoaAsync(bang, boLoc)).GetAwaiter().GetResult();
    }

    /// <summary>
    /// Gọi RPC (stored procedures/functions).
    /// </summary>
    public static async Task<string> GoiRpcAsync(string tenHam, object? thamSo = null)
    {
        var url = CauHinhSupabase.Url + $"/rest/v1/rpc/{tenHam}";
        var json = thamSo != null ? JsonSerializer.Serialize(thamSo, _tuyChonJson) : "{}";
        var content = new StringContent(json, Encoding.UTF8, "application/json");

        var request = new HttpRequestMessage(HttpMethod.Post, url) { Content = content };
        request.Headers.Add("apikey", CauHinhSupabase.ApiKey);
        request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", CauHinhSupabase.ApiKey);

        using var client = new HttpClient();
        var response = await client.SendAsync(request);
        response.EnsureSuccessStatusCode();
        return await response.Content.ReadAsStringAsync();
    }

    /// <summary>
    /// Kiểm tra kết nối Supabase.
    /// </summary>
    public static bool KiemTraKetNoi()
    {
        try
        {
            var ketQua = Task.Run(async () =>
            {
                var response = await _httpClient.GetAsync("roles?select=id&limit=1");
                return response.IsSuccessStatusCode;
            }).GetAwaiter().GetResult();
            return ketQua;
        }
        catch
        {
            return false;
        }
    }
}
