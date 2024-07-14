using Dapper;
using spotify_stats_api.database.models;
using spotify_stats_api.database.repositories.models;

namespace spotify_stats_api.database.repositories;

public class UserRepository(DbContext context) : IUserRepository
{
    public async Task AddAsync(string name, string email, string accessToken, string refreshToken)
    {
        const string query = "INSERT INTO users(name, email, access_token, refresh_token, last_update) VALUES(@Name, @Email, @AccessToken, @RefreshToken, @LastUpdate)";
        using var connection = context.CreateConnection();
        await connection.QuerySingleAsync(query, new { name, email, accessToken, refreshToken, lastUpdate = DateTime.UtcNow });
    }

    public async Task UpdateTokenAsync(long userId, string accessToken, string refreshToken)
    {
        const string query = "UPDATE users SET refresh_token = @RefreshToken, access_token = @AccessToken WHERE id = @UserId";
        using var connection = context.CreateConnection();
        await connection.ExecuteAsync(query, new { refreshToken, accessToken, userId });
    }

    public async Task<User> GetByIdAsync(long userId)
    {
        const string query = "SELECT id, name, email, access_token AS AccessToken, refresh_token AS RefreshToken, last_update AS LastUpdated FROM users WHERE id = @UserId";
        using var connection = context.CreateConnection();
        return await connection.QuerySingleAsync<User>(query, new { userId });
    }
}