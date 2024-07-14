using System.IdentityModel.Tokens.Jwt;
using System.Text;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.IdentityModel.Tokens;

namespace spotify_stats_api.lib;

public class JwtService(
    IConfiguration configuration,
    IDistributedCache cache
    ) : IJwtService
{
    public async Task<KeyValuePair<string, string>> GenerateTokens(long userId)
    {
        var jwt = GenerateJwt(userId);
        var refreshToken = GenerateRefreshToken();
        
        await cache.SetStringAsync(jwt, refreshToken, new DistributedCacheEntryOptions
        {
            AbsoluteExpirationRelativeToNow = TimeSpan.FromDays(14)
        });
        
        return new(jwt, refreshToken);
    }

    public string GenerateJwt(long userId)
    {
        var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["Jwt:Key"]));
        var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

        var sectoken = new JwtSecurityToken(configuration["Jwt:Issuer"],
            configuration["Jwt:Issuer"],
            null,
            expires: DateTime.Now.AddMinutes(120),
            signingCredentials: credentials);

        return new JwtSecurityTokenHandler().WriteToken(sectoken);
    }

    public async Task<string?> IsTokenValid(string token)
    {
        return await cache.GetStringAsync(token);
    }

    public async Task RemoveToken(string token)
    {
        await cache.RemoveAsync(token);
    }

    private static string GenerateRefreshToken()
    {
        return Guid.NewGuid().ToString();
    }
}