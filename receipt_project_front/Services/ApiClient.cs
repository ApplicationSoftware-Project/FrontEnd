using System.Net.Http.Headers;

namespace receipt_project_front.Services;

internal static class ApiConfig
{
    // Backend (AppDevelop) defaults to https://localhost:65289 / http://localhost:65290.
    // Target HTTPS so UseHttpsRedirection() doesn't 307 us and strip the
    // Authorization header on the follow-up request.
    public static string BaseUrl { get; set; } = "https://localhost:65289";
}

internal static class ApiClient
{
    private static readonly HttpClient _client = CreateClient();

    public static HttpClient ForCurrentUser()
    {
        var token = AppState.Current.AccessToken;
        _client.DefaultRequestHeaders.Authorization = string.IsNullOrWhiteSpace(token)
            ? null
            : new AuthenticationHeaderValue("Bearer", token);
        return _client;
    }

    private static HttpClient CreateClient()
    {
        var handler = new HttpClientHandler
        {
            // DEV ONLY: trust the ASP.NET Core self-signed dev cert on localhost.
            // Remove (or gate behind a build flag) before shipping to production.
            ServerCertificateCustomValidationCallback = (_, _, _, _) => true
        };

        var http = new HttpClient(handler)
        {
            BaseAddress = new Uri(ApiConfig.BaseUrl),
            Timeout = TimeSpan.FromSeconds(60)
        };
        http.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        return http;
    }
}

internal sealed class ApiException : Exception
{
    public int StatusCode { get; }
    public ApiException(int statusCode, string message) : base(message)
    {
        StatusCode = statusCode;
    }
}
