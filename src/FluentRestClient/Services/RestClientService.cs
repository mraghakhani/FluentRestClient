using System.Net.Http.Headers;
using System.Net.Mime;
using System.Text.Json;
using MessagePack;

namespace FluentRestClient.Services;

/// <inheritdoc />
internal sealed class RestClientService(IHttpClientFactory httpClientFactory) : IRestClientService
{
    private const string BearerAuthenticationScheme = "Bearer";
    private const string MessagePackMediaType = "application/x-msgpack";
    private const string JsonMediaType = MediaTypeNames.Application.Json;

    /// <summary>
    /// Creates an <see cref="HttpClient"/> instance from the factory with an optional named client key.
    /// </summary>
    private HttpClient CreateHttpClient(string? baseClientKey = null) =>
        string.IsNullOrWhiteSpace(baseClientKey)
            ? httpClientFactory.CreateClient()
            : httpClientFactory.CreateClient(baseClientKey);

    /// <summary>
    /// Serializes the request body using either JSON or MessagePack based on the provided options.
    /// </summary>
    private static HttpContent SerializeBody(RequestOptions options)
        => options.UseMessagePack
            ? new ByteArrayContent(
                MessagePackSerializer.Serialize(options.RequestBody, options.MessagePackSerializerOptions))
            : new StringContent(JsonSerializer.Serialize(options.RequestBody, options.JsonOptions), options.Encoding,
                MediaTypeNames.Application.Json);

    private static void ConfigureContentHeaders(HttpRequestMessage request, RequestOptions options)
    {
        var mediaType = options.UseMessagePack ? MessagePackMediaType : JsonMediaType;

        request.Headers.Accept.Clear();
        request.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue(mediaType));

        // If the request has content, set its Content-Type as well.
        if (request.Content is not null)
            request.Content.Headers.ContentType = new MediaTypeHeaderValue(mediaType);
    }

    /// <summary>
    /// Adds custom headers to the HTTP request.
    /// </summary>
    private static void AddHeaders(HttpRequestMessage request, Dictionary<string, string>? headers)
    {
        if (headers is not { Count: > 0 }) return;

        foreach (var keyValuePair in headers)
            request.Headers.TryAddWithoutValidation(keyValuePair.Key, keyValuePair.Value);
    }

    /// <summary>
    /// Handles and deserializes the response based on the serialization format.
    /// </summary>
    private static async Task<TResponse?> HandleResponseAsync<TResponse>(HttpResponseMessage response,
        RequestOptions options, CancellationToken cancellationToken)
    {
        var responseStream = await response.Content.ReadAsStreamAsync(cancellationToken);

        if (options.UseMessagePack)
            return await MessagePackSerializer.DeserializeAsync<TResponse>(responseStream,
                options.MessagePackSerializerOptions, cancellationToken);

        return await JsonSerializer.DeserializeAsync<TResponse>(responseStream, options.JsonOptions,
            cancellationToken);
    }

    /// <inheritdoc />
    public async Task<TResponse?> RequestAsync<TResponse>(
        HttpMethod method, string url, RequestOptions options, CancellationToken cancellationToken = default)
    {
        using var client = CreateHttpClient(options.BaseClientKey);

        if (!string.IsNullOrWhiteSpace(options.BearerToken))
            client.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue(BearerAuthenticationScheme, options.BearerToken);


        var request = new HttpRequestMessage(method, url)
        {
            Content = options.RequestBody != null ? SerializeBody(options) : null
        };

        ConfigureContentHeaders(request, options);

        AddHeaders(request, options.Headers);

        var response = await client.SendAsync(request, cancellationToken);
        return await HandleResponseAsync<TResponse>(response, options, cancellationToken);
    }

    /// <inheritdoc />
    public async Task<(ReadOnlyMemory<byte> responseBody, int statusCode)> RequestAsync(
        HttpMethod method, string url, RequestOptions options, CancellationToken cancellationToken = default)
    {
        using var client = CreateHttpClient(options.BaseClientKey);

        if (!string.IsNullOrWhiteSpace(options.BearerToken))
            client.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue(BearerAuthenticationScheme, options.BearerToken);

        var request = new HttpRequestMessage(method, url)
        {
            Content = options.RequestBody != null ? SerializeBody(options) : null
        };

        ConfigureContentHeaders(request, options);

        AddHeaders(request, options.Headers);

        var response = await client.SendAsync(request, cancellationToken);
        return (await response.Content.ReadAsByteArrayAsync(cancellationToken), (int)response.StatusCode);
    }
}