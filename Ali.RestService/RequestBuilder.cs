using System.Text.Json;
using Ali.RestService.Extensions;
using Ali.RestService.Services;
using MessagePack;
using Microsoft.AspNetCore.WebUtilities;

namespace Ali.RestService;

/// <summary>
/// Fluent builder for constructing and sending HTTP requests using configurable options.
/// </summary>
public sealed class RequestBuilder
{
    private readonly HttpMethod _method;
    private string _url;
    private readonly RequestOptions _options = new();

    /// <summary>
    /// Private constructor to initialize the builder with HTTP method and URL.
    /// </summary>
    private RequestBuilder(HttpMethod method, string url)
    {
        _method = method;
        _url = url;
    }

    /// <summary>
    /// Initializes a new instance of <see cref="RequestBuilder"/> with the specified HTTP method and URL.
    /// </summary>
    /// <param name="method">HTTP method (GET, POST, etc.).</param>
    /// <param name="url">Target request URL.</param>
    /// <returns>An initialized <see cref="RequestBuilder"/> instance.</returns>
    public static RequestBuilder Create(HttpMethod method, string url) => new(method, url);

    /// <summary>
    /// Sets the named HttpClient key for dependency injection resolution.
    /// </summary>
    /// <param name="clientKey">The key name for the HttpClient.</param>
    /// <returns>The current <see cref="RequestBuilder"/> instance.</returns>
    public RequestBuilder WithClientKey(string clientKey)
    {
        _options.WithKey(clientKey);
        return this;
    }

    /// <summary>
    /// Adds a bearer token for Authorization.
    /// </summary>
    /// <param name="bearerToken">JWT or other bearer token.</param>
    /// <returns>The current <see cref="RequestBuilder"/> instance.</returns>
    public RequestBuilder WithBearerToken(string bearerToken)
    {
        _options.WithBearerToken(bearerToken);
        return this;
    }

    /// <summary>
    /// Adds custom HTTP headers to the request.
    /// </summary>
    /// <param name="headers">Key-value pairs of header names and values.</param>
    /// <returns>The current <see cref="RequestBuilder"/> instance.</returns>
    public RequestBuilder WithHeaders(Dictionary<string, string> headers)
    {
        _options.WithHeaders(headers);
        return this;
    }

    /// <summary>
    /// Sets the request body to be serialized as JSON.
    /// </summary>
    /// <param name="body">The request payload object.</param>
    /// <param name="jsonOptions">Optional custom JSON serializer options.</param>
    /// <returns>The current <see cref="RequestBuilder"/> instance.</returns>
    public RequestBuilder WithJsonBody(object body, JsonSerializerOptions? jsonOptions = null)
    {
        _options.WithData(body)
            .WithJsonOptions(jsonOptions ?? _options.JsonOptions);
        return this;
    }

    /// <summary>
    /// Sets the request body to be serialized as MessagePack.
    /// </summary>
    /// <param name="body">The request payload object.</param>
    /// <param name="mpOptions">Optional custom MessagePack serializer options.</param>
    /// <returns>The current <see cref="RequestBuilder"/> instance.</returns>
    public RequestBuilder WithMessagePackBody(object body, MessagePackSerializerOptions? mpOptions = null)
    {
        _options.WithData(body)
            .WithMessagePackOptions(mpOptions);
        return this;
    }

    /// <summary>
    /// Sets the request body and response body to be serialized/deserialized as MessagePack.
    /// </summary>
    /// <param name="mpOptions">Optional custom MessagePack serializer options.</param>
    /// <returns>The current <see cref="RequestBuilder"/> instance.</returns>
    public RequestBuilder WithMessagePackEnabled(MessagePackSerializerOptions? mpOptions = null)
    {
        _options.WithMessagePackOptions(mpOptions);
        return this;
    }

    /// <summary>
    /// Appends query string parameters to the request URL.
    /// </summary>
    /// <param name="queryParams">Key-value pairs of query parameters.</param>
    /// <returns>The current <see cref="RequestBuilder"/> instance.</returns>
    public RequestBuilder WithQueryParams(Dictionary<string, object?> queryParams)
    {
        var filteredParams = queryParams
            .Where(kv => kv.Value is not null) // Remove null values
            .ToDictionary(
                kv => kv.Key,
                kv => kv.Value switch
                {
                    DateOnly date => date.ToString("yyyy-MM-dd"),
                    bool boolean => boolean.ToString().ToLower(), // Convert bool to lowercase ("true"/"false")
                    _ => kv.Value?.ToString() // Default string conversion for other types
                });

        _url = filteredParams.Count > 0 ? QueryHelpers.AddQueryString(_url, filteredParams) : _url;

        return this;
    }

    /// <summary>
    /// Sends the HTTP request and deserializes the response as the specified type.
    /// </summary>
    /// <typeparam name="TResponse">The expected response type.</typeparam>
    /// <param name="client">A service for sending HTTP requests.</param>
    /// <param name="cancellationToken">Optional cancellation token.</param>
    /// <returns>The deserialized response or null if no content was returned.</returns>
    public Task<TResponse?> SendAsync<TResponse>(IRestClientService client,
        CancellationToken cancellationToken = default)
        => client.RequestAsync<TResponse>(_method, _url, _options, cancellationToken);

    /// <summary>
    /// Sends the HTTP request and returns the raw byte response (useful for file downloads or binary data).
    /// </summary>
    /// <param name="client">A service for sending HTTP requests.</param>
    /// <param name="cancellationToken">Optional cancellation token.</param>
    /// <returns>Byte array of the response content.</returns>
    public async Task<ReadOnlyMemory<byte>> SendAsBytesAsync(IRestClientService client,
        CancellationToken cancellationToken = default)
        => await client.RequestAsync(_method, _url, _options, cancellationToken);

    /// <summary>
    /// Applies a transformation to the <see cref="RequestBuilder"/> only if a specified condition is true.
    /// </summary>
    /// <param name="condition">A boolean value that determines whether the transformation should be applied.</param>
    /// <param name="apply">A function that takes a <see cref="RequestBuilder"/> and returns a modified instance.</param>
    /// <returns>The original or modified <see cref="RequestBuilder"/> depending on the condition.</returns>
    public RequestBuilder ApplyIf(bool condition, Func<RequestBuilder, RequestBuilder> apply)
        => condition ? apply(this) : this;
}