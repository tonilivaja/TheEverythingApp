using Microsoft.Extensions.DependencyInjection;
using TheEverythingApp.Application.Features.Auth;
using TheEverythingApp.Infrastructure.Features.Auth;

namespace TheEverythingApp.Infrastructure;

public static class ServiceCollectionExtension
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services)
    {
        services.AddScoped<IAuthService, AuthService>();
        services.AddScoped<IJwtService, JwtService>();

        return services;
    }
}
