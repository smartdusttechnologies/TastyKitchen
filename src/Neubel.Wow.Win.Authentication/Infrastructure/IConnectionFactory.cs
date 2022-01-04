using System.Data;

namespace Neubel.Wow.Win.Authentication.Infrastructure
{
    public interface IConnectionFactory
    {
        IDbConnection GetConnection{ get; }
    }
}
