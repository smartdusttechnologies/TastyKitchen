using System.Data;
using System.Data.Common;
using Microsoft.Extensions.Configuration;

namespace Neubel.Wow.Win.Authentication.Infrastructure
{
    public class ConnectionFactory : IConnectionFactory
    {
        private static IConfiguration _configuration;

        public ConnectionFactory(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public IDbConnection GetConnection
        {
            get
            {
                DbProviderFactories.RegisterFactory("System.Data.SqlClient", System.Data.SqlClient.SqlClientFactory.Instance);
                var factory = DbProviderFactories.GetFactory("System.Data.SqlClient");
                var conn = factory.CreateConnection();
                if (conn == null) return null;
                conn.ConnectionString = _configuration["ConnectionStrings:IdentityDBAws"];
                conn.Open();
                return conn;
            }
        }
    }
}
