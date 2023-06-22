using Microsoft.Data.SqlClient;

namespace MotoShop.Data
{
    public class DbContext
    {
        private readonly string _connectionString;

        public DbContext(IConfiguration config)
        {
            _connectionString = config.GetConnectionString("Default") ?? throw new Exception("Failed to connect to Database");
        }

        public SqlConnection GetConnection() => new(_connectionString);
    }
}
