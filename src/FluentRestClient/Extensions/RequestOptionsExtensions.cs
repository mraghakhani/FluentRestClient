using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using MessagePack;

namespace FluentRestClient.Extensions;

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

    /// <summary>
    /// Enables multipart/form-data content type for the request.
    /// </summary>
    public static RequestOptions WithMultipartFormData(this RequestOptions options)
    {
        options.UseMultipartFormData = true;
        options.UseMessagePack = false;
        options.MessagePackSerializerOptions = null;
        options.MultipartContent ??= new MultipartFormDataContent();
        return options;
    }

    /// <summary>
    /// Adds a file to the multipart/form-data request.
    /// </summary>
    /// <param name="options">The request options.</param>
    /// <param name="name">The name of the form field.</param>
    /// <param name="fileContent">The file content as a byte array.</param>
    /// <param name="fileName">The file name.</param>
    /// <param name="contentType">Optional content type (defaults to application/octet-stream).</param>
    public static RequestOptions AddFile(this RequestOptions options, string name, byte[] fileContent, string fileName, string? contentType = null)
    {
        options.WithMultipartFormData();
        
        var byteArrayContent = new ByteArrayContent(fileContent);
        byteArrayContent.Headers.ContentType = new MediaTypeHeaderValue(contentType ?? "application/octet-stream");
        
        options.MultipartContent!.Add(byteArrayContent, name, fileName);
        return options;
    }

    /// <summary>
    /// Adds a file stream to the multipart/form-data request.
    /// </summary>
    /// <param name="options">The request options.</param>
    /// <param name="name">The name of the form field.</param>
    /// <param name="fileStream">The file stream.</param>
    /// <param name="fileName">The file name.</param>
    /// <param name="contentType">Optional content type (defaults to application/octet-stream).</param>
    public static RequestOptions AddFileStream(this RequestOptions options, string name, Stream fileStream, string fileName, string? contentType = null)
    {
        options.WithMultipartFormData();
        
        var streamContent = new StreamContent(fileStream);
        streamContent.Headers.ContentType = new MediaTypeHeaderValue(contentType ?? "application/octet-stream");
        
        options.MultipartContent!.Add(streamContent, name, fileName);
        return options;
    }

    /// <summary>
    /// Adds a form field to the multipart/form-data request.
    /// </summary>
    /// <param name="options">The request options.</param>
    /// <param name="name">The name of the form field.</param>
    /// <param name="value">The value of the form field.</param>
    public static RequestOptions AddFormField(this RequestOptions options, string name, string value)
    {
        options.WithMultipartFormData();
        options.MultipartContent!.Add(new StringContent(value), name);
        return options;
    }
}