using System.Net;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text.Json;
using receipt_project_front.Models;

namespace receipt_project_front.Services;

internal static class ReceiptApi
{
    private static readonly JsonSerializerOptions JsonOptions = new()
    {
        PropertyNameCaseInsensitive = true
    };

    public static async Task<UploadReceiptResult> UploadAsync(
        string filePath,
        CancellationToken ct = default)
    {
        await using var fileStream = File.OpenRead(filePath);
        using var fileContent = new StreamContent(fileStream);
        fileContent.Headers.ContentType = new MediaTypeHeaderValue(GuessContentType(filePath));

        using var form = new MultipartFormDataContent
        {
            { fileContent, "file", Path.GetFileName(filePath) }
        };

        using var response = await ApiClient.ForCurrentUser()
            .PostAsync("/api/receipts/upload", form, ct);

        var body = await response.Content.ReadAsStringAsync(ct);

        if (response.StatusCode == HttpStatusCode.Created)
        {
            var parsed = JsonSerializer.Deserialize<UploadReceiptResult>(body, JsonOptions)
                         ?? throw new ApiException(500, "서버 응답을 해석할 수 없습니다.");
            return parsed;
        }

        throw new ApiException((int)response.StatusCode, FormatUploadError(response.StatusCode, body));
    }

    public static async Task<ConfirmReceiptCategoryResult> ConfirmCategoryAsync(
        Guid receiptId,
        string finalCategory,
        CancellationToken ct = default)
    {
        var request = new ConfirmReceiptCategoryRequest(finalCategory);

        using var response = await ApiClient.ForCurrentUser()
            .PostAsJsonAsync($"/api/receipts/{receiptId}/confirm", request, JsonOptions, ct);

        var body = await response.Content.ReadAsStringAsync(ct);

        if (response.StatusCode == HttpStatusCode.OK)
        {
            return JsonSerializer.Deserialize<ConfirmReceiptCategoryResult>(body, JsonOptions)
                   ?? throw new ApiException(500, "서버 응답을 해석할 수 없습니다.");
        }

        throw new ApiException((int)response.StatusCode, FormatConfirmError(response.StatusCode, body));
    }

    public static async Task<ReceiptListResult> GetListAsync(
        int page = 1,
        int pageSize = 50,
        CancellationToken ct = default)
    {
        using var response = await ApiClient.ForCurrentUser()
            .GetAsync($"/api/receipts/?page={page}&pageSize={pageSize}", ct);

        var body = await response.Content.ReadAsStringAsync(ct);

        if (response.StatusCode == HttpStatusCode.OK)
        {
            return JsonSerializer.Deserialize<ReceiptListResult>(body, JsonOptions)
                   ?? throw new ApiException(500, "서버 응답을 해석할 수 없습니다.");
        }

        throw new ApiException((int)response.StatusCode, FormatGenericError(response.StatusCode, body));
    }

    public static async Task<Image> GetImageAsync(Guid receiptId, CancellationToken ct = default)
    {
        using var response = await ApiClient.ForCurrentUser()
            .GetAsync($"/api/receipts/{receiptId}/image", HttpCompletionOption.ResponseHeadersRead, ct);

        if (response.StatusCode != HttpStatusCode.OK)
        {
            var errBody = await response.Content.ReadAsStringAsync(ct);
            throw new ApiException((int)response.StatusCode, FormatGenericError(response.StatusCode, errBody));
        }

        var bytes = await response.Content.ReadAsByteArrayAsync(ct);
        // MemoryStream must outlive the Image (Image.FromStream lazily reads
        // pixels), so don't dispose it here — it'll be GC'd with the Image.
        var ms = new MemoryStream(bytes, writable: false);
        return Image.FromStream(ms);
    }

    private static string FormatGenericError(HttpStatusCode status, string body) => status switch
    {
        HttpStatusCode.Unauthorized =>
            "인증이 필요합니다. Settings에서 Dev Access Token을 입력해 주세요.",
        HttpStatusCode.NotFound =>
            ExtractValidationMessage(body) ?? "찾을 수 없습니다.",
        HttpStatusCode.BadRequest =>
            ExtractValidationMessage(body) ?? "잘못된 요청입니다.",
        _ => $"오류 ({(int)status}): {Truncate(body, 200)}"
    };

    private static string FormatUploadError(HttpStatusCode status, string body) => status switch
    {
        HttpStatusCode.Unauthorized =>
            "인증이 필요합니다. Settings에서 Dev Access Token을 입력해 주세요.",
        HttpStatusCode.UnprocessableEntity =>
            "영수증으로 인식되지 않은 이미지입니다. 다른 이미지를 시도해 주세요.",
        HttpStatusCode.BadRequest =>
            ExtractValidationMessage(body) ?? "잘못된 요청입니다.",
        _ => $"업로드 실패 ({(int)status}): {Truncate(body, 200)}"
    };

    private static string FormatConfirmError(HttpStatusCode status, string body) => status switch
    {
        HttpStatusCode.Unauthorized =>
            "인증이 필요합니다. Settings에서 Dev Access Token을 입력해 주세요.",
        HttpStatusCode.NotFound =>
            ExtractValidationMessage(body) ?? "영수증을 찾을 수 없습니다.",
        _ => $"저장 실패 ({(int)status}): {Truncate(body, 200)}"
    };

    private static string? ExtractValidationMessage(string body)
    {
        try
        {
            using var doc = JsonDocument.Parse(body);
            if (doc.RootElement.TryGetProperty("errors", out var errors) &&
                errors.ValueKind == JsonValueKind.Object)
            {
                var first = errors.EnumerateObject().FirstOrDefault();
                if (first.Value.ValueKind == JsonValueKind.Array && first.Value.GetArrayLength() > 0)
                    return first.Value[0].GetString();
            }
            if (doc.RootElement.TryGetProperty("detail", out var detail))
                return detail.GetString();
        }
        catch (JsonException) { }
        return null;
    }

    private static string GuessContentType(string path) => Path.GetExtension(path).ToLowerInvariant() switch
    {
        ".jpg" or ".jpeg" => "image/jpeg",
        ".png" => "image/png",
        ".webp" => "image/webp",
        _ => "application/octet-stream"
    };

    private static string Truncate(string s, int max) =>
        string.IsNullOrEmpty(s) || s.Length <= max ? s : s.Substring(0, max) + "…";
}
