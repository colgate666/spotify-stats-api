namespace spotify_stats_api.lib;

public interface IJwtService
{
    Task<KeyValuePair<string, string>> GenerateTokens(long userId);
    Task<KeyValuePair<string, string>> GenerateTokens(string token);
    string GenerateJwt(long userId);
    Task<string?> IsTokenValid(string token);
    Task RemoveToken(string token);
}