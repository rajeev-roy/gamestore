using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;

namespace GameStore.API.Authorization;

public static class AuthorizationExtensions
{
    public static IServiceCollection AddGameStoreAuthorization(this IServiceCollection services)
    {
        services.AddScoped<IClaimsTransformation,ScopeTransformation>()
                .AddAuthorization(options => {
    options.AddPolicy(Policies.Read_Access, builder => {
        builder.RequireClaim("scope", "games:read")
               .AddAuthenticationSchemes(JwtBearerDefaults.AuthenticationScheme,"Auth0");
    });
    options.AddPolicy(Policies.Write_Access, builder => {
        builder.RequireClaim("scope", "games:write")
        .RequireRole("Admin")
        .AddAuthenticationSchemes(JwtBearerDefaults.AuthenticationScheme,"Auth0");});
        });
        return services;
    }
}