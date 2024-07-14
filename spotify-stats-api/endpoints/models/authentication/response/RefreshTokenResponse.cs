namespace spotify_stats_api.endpoints.models.authentication.response;

public class RefreshTokenResponse
{
    public string AccessToken { get; set; }
    public string RefreshToken { get; set; }
}