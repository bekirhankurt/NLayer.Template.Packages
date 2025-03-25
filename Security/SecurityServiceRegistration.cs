using Microsoft.Extensions.DependencyInjection;
using Security.EmailAuthenticator;
using Security.Jwt;
using Security.OtpAuthenticator;
using Security.OtpAuthenticator.OtpNet;

namespace Security;

public static class SecurityServiceRegistration
{
    public static IServiceCollection AddSecurityServices(this IServiceCollection services)
    {
        services.AddScoped<ITokenHelper, JwtHelper>();
        services.AddScoped<IEmailAuthenticatorHelper, EmailAuthenticatorHelper>();
        services.AddScoped<IOtpAuthenticationHelper, OtpNetAuthenticationHelper>();

        return services;
    }
}