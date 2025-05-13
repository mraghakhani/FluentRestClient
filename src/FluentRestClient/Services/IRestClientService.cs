namespace FluentRestClient.Services;

/// <summary>
/// Defines the contract for making HTTP requests with various options for handling responses.
/// </summary>
public interface IRestClientService
{
    /// <summary>
    /// Sends an HTTP request and returns the response deserialized to the specified type.
    /// </summary>
    /// <typeparam name="TResponse">The type to which the response should be deserialized.</typeparam>
    /// <param name="method">The HTTP method (GET, POST, etc.) for the request.</param>
    /// <param name="url">The URL for the request.</param>
    /// <param name="options">The options that define the request, including headers, body, and serialization options.</param>
    /// <param name="cancellationToken">A token to cancel the request operation, if needed.</param>
    /// <returns>A task that represents the asynchronous operation, with a result of the deserialized response.</returns>
    Task<TResponse?> RequestAsync<TResponse>(HttpMethod method, string url, RequestOptions options,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Sends an HTTP request using the specified method, URL, and options, and returns the response content as raw bytes along with the HTTP status code.
    /// Suitable for retrieving binary data such as files, images, or other non-text responses.
    /// </summary>
    /// <param name="method">The HTTP method (e.g., GET, POST, PUT, DELETE) to be used for the request.</param>
    /// <param name="url">The endpoint URL to which the request is sent.</param>
    /// <param name="options">Request configuration including headers, body content, and serialization settings.</param>
    /// <param name="cancellationToken">A cancellation token that can be used to cancel the request.</param>
    /// <returns>A task representing the asynchronous operation. The result contains a tuple with the response body as <see cref="ReadOnlyMemory{byte}"/> and the HTTP status code.</returns>
    Task<(ReadOnlyMemory<byte> responseBody, int statusCode)> RequestAsync(HttpMethod method, string url, RequestOptions options,
        CancellationToken cancellationToken = default);
}