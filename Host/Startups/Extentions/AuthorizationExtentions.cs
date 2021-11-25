﻿using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;

namespace Host;

public static class AuthorizationExtentions
{
    public static IServiceCollection AddAuthorizationAndPolicies(this IServiceCollection services, IConfiguration configuration)
    {
        return services.AddAuthorization(options =>
        {
            options.DefaultPolicy = new AuthorizationPolicyBuilder()
                .RequireAuthenticatedUser()
                .RequireClaim(ClaimTypes.Role, configuration.GetValueOrDefault("oidc:roles:user", "user"))
                .Build();

            options.AddPolicy("admin", policy => policy
                .RequireAuthenticatedUser()
                .RequireClaim(ClaimTypes.Role, configuration.GetValueOrDefault("oidc:roles:admin", "admin")));
        });
    }
}
