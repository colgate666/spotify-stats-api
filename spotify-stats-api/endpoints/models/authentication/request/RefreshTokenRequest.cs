namespace spotify_stats_api.endpoints.models.authentication.request;

public class RefreshTokenRequest
{
    public string AccessToken { get; set; }
    public string RefreshToken { get; set; }
}