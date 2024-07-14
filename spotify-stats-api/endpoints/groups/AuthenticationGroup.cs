using Microsoft.AspNetCore.Http.HttpResults;
using spotify_stats_api.endpoints.models.authentication.request;
using spotify_stats_api.endpoints.models.authentication.response;
using spotify_stats_api.lib;

namespace spotify_stats_api.endpoints.groups;

public static class AuthenticationGroup
{
    public static void RegisterAuthenticationEndpoints(this WebApplication app)
    {
        app.MapPost("/RefreshToken", RefreshToken);
    }

    static async Task<Results<Ok<RefreshTokenResponse>, BadRequest<string>>> RefreshToken(
        RefreshTokenRequest request, 
        IJwtService jwtService)
    {
        var refreshToken = await jwtService.IsTokenValid(request.AccessToken);

        if (refreshToken is null || refreshToken != request.RefreshToken)
        {
            return TypedResults.BadRequest("Invalid Refresh token");
        }

        await jwtService.RemoveToken(request.AccessToken);
        var tokens = await jwtService.GenerateTokens(request.AccessToken);

        return TypedResults.Ok(new RefreshTokenResponse
        {
            AccessToken = tokens.Key,
            RefreshToken = tokens.Value
        });
    }
}