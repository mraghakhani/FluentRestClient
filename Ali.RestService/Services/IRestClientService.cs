namespace Ali.RestService.Services;

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
    /// Sends an HTTP request and returns the response as raw bytes.
    /// </summary>
    /// <param name="method">The HTTP method (GET, POST, etc.) for the request.</param>
    /// <param name="url">The URL for the request.</param>
    /// <param name="options">The options that define the request, including headers, body, and serialization options.</param>
    /// <param name="cancellationToken">A token to cancel the request operation, if needed.</param>
    /// <returns>A task that represents the asynchronous operation, with a result of the response as raw bytes.</returns>
    Task<ReadOnlyMemory<byte>> RequestAsync(HttpMethod method, string url, RequestOptions options,
        CancellationToken cancellationToken = default);
}