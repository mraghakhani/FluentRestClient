using Ali.RestService.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;

namespace Ali.RestService;

/// <summary>
/// Provides extension methods for registering REST client services in the dependency injection container.
/// </summary>
public static class DependencyResolver
{
    /// <summary>
    /// Adds the REST client services to the <see cref="IServiceCollection"/> container.
    /// This includes registering the <see cref="IRestClientService"/> and configuring
    /// necessary HTTP services like <see cref="HttpClient"/> and <see cref="IHttpContextAccessor"/>.
    /// </summary>
    /// <param name="services">The <see cref="IServiceCollection"/> to which the services will be added.</param>
    /// <returns>The updated <see cref="IServiceCollection"/> instance.</returns>
    public static IServiceCollection AddRestServices(this IServiceCollection services)
    {
        services.AddScoped<IRestClientService, RestClientService>();
        services.AddHttpContextAccessor();
        return services;
    }
}