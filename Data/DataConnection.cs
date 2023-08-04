using System.Data.SqlClient;

namespace LucasVaz.Data
{
    public class DataConnection
    {
        private readonly string _connectionString;

        public DataConnection(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("PortfolioDatabase");
        }

        public SqlConnection CreateConnection()
        {
            return new SqlConnection(_connectionString);
        }
    }
}
