using System.Text;
using System.Text.Json;
using MessagePack;

namespace FluentRestClient;

/// <summary>
/// Represents configurable options for HTTP requests, including serialization settings, headers, authorization, and more.
/// </summary>
public class RequestOptions
{
    private static readonly JsonSerializerOptions CamelCaseJsonOptions = new()
    {
        PropertyNamingPolicy = JsonNamingPolicy.CamelCase
    };

    /// <summary>
    /// Gets or sets the named client key for the <see cref="IHttpClientFactory"/> to use.
    /// </summary>
    public string? BaseClientKey { get; set; }

    /// <summary>
    /// Gets or sets the request body data to be serialized.
    /// </summary>
    public object? RequestBody { get; set; }

    /// <summary>
    /// Gets or sets the bearer token for Authorization header.
    /// </summary>
    public string? BearerToken { get; set; }

    /// <summary>
    /// Gets or sets any additional headers for the request.
    /// </summary>
    public Dictionary<string, string>? Headers { get; set; }

    /// <summary>
    /// Gets or sets the JSON serializer options. Defaults to camelCase.
    /// </summary>
    public JsonSerializerOptions JsonOptions { get; set; } = CamelCaseJsonOptions;

    /// <summary>
    /// Gets or sets whether to use MessagePack instead of JSON.
    /// </summary>
    public bool UseMessagePack { get; set; } = false;

    /// <summary>
    /// Gets or sets the MessagePack serializer options (if <see cref="UseMessagePack"/> is true).
    /// </summary>
    public MessagePackSerializerOptions? MessagePackSerializerOptions { get; set; }

    /// <summary>
    /// Gets or sets the character encoding for the request content. Defaults to UTF-8.
    /// </summary>
    public Encoding Encoding { get; set; } = Encoding.UTF8;

    /// <summary>
    /// Gets a reusable default instance of <see cref="RequestOptions"/>.
    /// </summary>
    public static readonly RequestOptions Default = new();

    /// <summary>
    /// Creates a new <see cref="RequestOptions"/> instance with the specified base client key.
    /// </summary>
    public static RequestOptions WithKey(string baseClientKey)
        => new()
        {
            BaseClientKey = baseClientKey,
        };

    /// <summary>
    /// Creates a new <see cref="RequestOptions"/> instance with the specified data payload.
    /// </summary>
    public static RequestOptions WithData(object data)
        => new()
        {
            RequestBody = data,
        };

    /// <summary>
    /// Creates a new <see cref="RequestOptions"/> instance with the specified bearer token.
    /// </summary>
    public static RequestOptions WithBearerToken(string bearerToken)
        => new()
        {
            BearerToken = bearerToken,
        };

    /// <summary>
    /// Creates a new <see cref="RequestOptions"/> instance with the specified headers.
    /// </summary>
    public static RequestOptions WithHeaders(Dictionary<string, string> headers)
        => new()
        {
            Headers = headers,
        };


    /// <summary>
    /// Creates a new <see cref="RequestOptions"/> instance with the specified JSON serializer options.
    /// </summary>
    public static RequestOptions WithOptions(JsonSerializerOptions options)
        => new()
        {
            JsonOptions = options
        };

    /// <summary>
    /// Creates a new <see cref="RequestOptions"/> instance configured to use MessagePack with specified options.
    /// </summary>
    public static RequestOptions WithOptions(MessagePackSerializerOptions options)
        => new()
        {
            UseMessagePack = true,
            MessagePackSerializerOptions = options
        };
}