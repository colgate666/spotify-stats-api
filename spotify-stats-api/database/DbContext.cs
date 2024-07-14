using System.Data;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;

namespace spotify_stats_api.database;

public class DbContext
{
    private readonly IConfiguration _configuration;
    private readonly string _connectionString;

    public DbContext(IConfiguration configuration)
    {
        _configuration = configuration;
        _connectionString = _configuration.GetConnectionString("database");
    }

    public IDbConnection CreateConnection()
        => new SqlConnection(_connectionString);
}