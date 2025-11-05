using Dapper;
using Montesa.ProduccionPrograma.API.Data;
using Montesa.ProduccionPrograma.API.Models;
using System.Data;

namespace Montesa.ProduccionPrograma.API.Services
{
   
    public class ProdMaquinaRepository: IProdMaquinaRepository
    {
        private readonly ISqlConnectionFactory _conectarfactory;

        public ProdMaquinaRepository(ISqlConnectionFactory connectionFactory)
        {
            _conectarfactory = connectionFactory;
        }

        public async Task<IEnumerable<Prod_Maquina>> GetAllAsync()
        {
            using var connection = _conectarfactory.CreateConnection();
            var sql = "SELECT * FROM Produccion_Maquina";
            return await connection.QueryAsync<Prod_Maquina>(sql);
        }

        public async Task<Prod_Maquina?> GetByIdAsync(int recursoId)
        {
            using var connection = _conectarfactory.CreateConnection();
            var sql = "SELECT * FROM Produccion_Maquina WHERE RecursoId = @RecursoId";
            return await connection.QueryFirstOrDefaultAsync<Prod_Maquina>(sql, new { recursoId = recursoId});
        }
    }
}
