using System.Data;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;

namespace Montesa.ProduccionPrograma.API.Data
{
    // Renombrada para evitar conflicto con EF
    public interface ISqlConnectionFactory
    {
        IDbConnection CreateConnection();
    }

    public sealed class DapperContext : ISqlConnectionFactory
    {
        private readonly string _cs;

        public DapperContext(IConfiguration cfg)
        {
            _cs = cfg.GetConnectionString("DBConexion")
                 ?? throw new InvalidOperationException("Falta la connection string 'DBConexion'.");
        }

        public IDbConnection CreateConnection() => new SqlConnection(_cs);
    }
}
