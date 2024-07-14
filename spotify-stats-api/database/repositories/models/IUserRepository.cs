using spotify_stats_api.database.models;

namespace spotify_stats_api.database.repositories.models;

public interface IUserRepository
{
    Task AddAsync(string name, string email, string accessToken, string refreshToken);
    Task UpdateTokenAsync(long userId, string accessToken, string refreshToken);
    Task<User> GetByIdAsync(long userId);
}