using Microsoft.Extensions.DependencyInjection;
using Persistence.Repositories.Concrete;
using Security.EmailAuthenticator;
using Security.Jwt;
using Security.OtpAuthenticator;
using Security.OtpAuthenticator.OtpNet;

namespace Security;

public static class SecurityServiceRegistration
{
    public static IServiceCollection AddSecurityServices<TId>(this IServiceCollection services)
    {
        
        services.AddScoped<ITokenHelper<TId>, JwtHelper<TId>>();
        services.AddScoped<IEmailAuthenticatorHelper, EmailAuthenticatorHelper>();
        services.AddScoped<IOtpAuthenticationHelper, OtpNetAuthenticationHelper>();

        return services;
    }
}
