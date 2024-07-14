using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text.Encodings.Web;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Options;
using spotify_stats_api.lib;

namespace spotify_stats_api.extensions;

public class JwtAuthenticationHandler(
    IOptionsMonitor<JwtBearerOptions> options,
    ILoggerFactory logger,
    UrlEncoder encoder,
    IJwtService jwtService)
    : JwtBearerHandler(options, logger, encoder)
{
    protected override async Task<AuthenticateResult> HandleAuthenticateAsync()
    {
        // Get the token from the Authorization header
        if (!Context.Request.Headers.TryGetValue("Authorization", out var authorizationHeaderValues))
        {
            return AuthenticateResult.Fail("Authorization header not found.");
        }

        var authorizationHeader = authorizationHeaderValues.FirstOrDefault();
        if (string.IsNullOrEmpty(authorizationHeader) || !authorizationHeader.StartsWith("Bearer "))
        {
            return AuthenticateResult.Fail("Bearer token not found in Authorization header.");
        }

        var token = authorizationHeader["Bearer ".Length..].Trim();
        var authenticationResult = await base.HandleAuthenticateAsync();
        var refreshToken = await jwtService.IsTokenValid(token);

        if (!authenticationResult.Succeeded || refreshToken is null)
        {
            return AuthenticateResult.Fail("Invalid token.");
        }

        // Set the authentication result with the claims from the API response          
        var principal = GetClaims(token);

        return AuthenticateResult.Success(new AuthenticationTicket(principal, "CustomJwtBearer"));
    }

    private ClaimsPrincipal GetClaims(string Token)
    {
        var handler = new JwtSecurityTokenHandler();
        var token = handler.ReadToken(Token) as JwtSecurityToken;

        var claimsIdentity = new ClaimsIdentity(token.Claims, "Token");
        var claimsPrincipal = new ClaimsPrincipal(claimsIdentity);

        return claimsPrincipal;
    }
}