namespace spotify_stats_api.database.models;

public class User
{
    public long Id { get; set; }
    public string Name { get; set; }
    public string Email { get; set; }
    public string? AccessToken { get; set; }
    public string RefreshToken { get; set; }
    public DateTime LastUpdated { get; set; }
}