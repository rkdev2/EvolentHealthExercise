using Microsoft.Extensions.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Threading.Tasks;

namespace EvolentHealthExercise.Core.Repositories
{
    public class ConnectionRepository
    {
        private readonly IConfiguration configuration;

        public ConnectionRepository(IConfiguration configuration)
        {
            this.configuration = configuration;
        }

        public async Task<IDbConnection> OpenConnectionAsync()
        {
            SqlConnection dbConnection = new SqlConnection(configuration["ConnectionStrings:DefaultConnection"]);

            await dbConnection.OpenAsync();

            return dbConnection;
        }
    }
}