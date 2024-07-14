namespace spotify_stats_api.lib;

public interface IJwtService
{
    Task<KeyValuePair<string, string>> GenerateTokens(long userId);
    string GenerateJwt(long userId);
}