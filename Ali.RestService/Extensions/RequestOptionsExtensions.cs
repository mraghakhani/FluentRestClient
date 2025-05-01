using System.Text;
using System.Text.Json;
using MessagePack;

namespace Ali.RestService.Extensions;

/// <summary>
/// Provides extension methods to fluently configure <see cref="RequestOptions"/>.
/// </summary>
public static class RequestOptionsExtensions
{
    /// <summary>
    /// Sets the base client key used by the HTTP client factory.
    /// </summary>
    public static RequestOptions WithKey(this RequestOptions options, string baseClientKey)
    {
        options.BaseClientKey = baseClientKey;
        return options;
    }

    /// <summary>
    /// Sets the request body data.
    /// </summary>
    public static RequestOptions WithData(this RequestOptions options, object data)
    {
        options.RequestBody = data;
        return options;
    }

    /// <summary>
    /// Sets the bearer token for authorization.
    /// </summary>
    public static RequestOptions WithBearerToken(this RequestOptions options, string bearerToken)
    {
        options.BearerToken = bearerToken;
        return options;
    }

    /// <summary>
    /// Sets custom headers for the request.
    /// </summary>
    public static RequestOptions WithHeaders(this RequestOptions options, Dictionary<string, string> headers)
    {
        options.Headers = headers;
        return options;
    }

    /// <summary>
    /// Adds a single custom header to the request.
    /// </summary>
    /// <param name="options"></param>
    /// <param name="key">Header name.</param>
    /// <param name="value">Header value.</param>
    public static RequestOptions AddHeader(this RequestOptions options, string key, string value)
    {
        options.Headers ??= new Dictionary<string, string>();
        options.Headers[key] = value;
        return options;
    }

    /// <summary>
    /// Sets the JSON serialization options.
    /// This also disables MessagePack.
    /// </summary>
    public static RequestOptions WithJsonOptions(this RequestOptions options, JsonSerializerOptions jsonOptions)
    {
        options.JsonOptions = jsonOptions;
        options.UseMessagePack = false;
        options.MessagePackSerializerOptions = null;
        return options;
    }

    /// <summary>
    /// Enables MessagePack and sets the MessagePack serialization options.
    /// </summary>
    public static RequestOptions WithMessagePackOptions(this RequestOptions options,
        MessagePackSerializerOptions? messagePackOptions)
    {
        options.UseMessagePack = true;
        options.MessagePackSerializerOptions = messagePackOptions;
        return options;
    }

    /// <summary>
    /// Sets the character encoding for the request content.
    /// </summary>
    public static RequestOptions WithEncoding(this RequestOptions options, Encoding encoding)
    {
        options.Encoding = encoding;
        return options;
    }
}