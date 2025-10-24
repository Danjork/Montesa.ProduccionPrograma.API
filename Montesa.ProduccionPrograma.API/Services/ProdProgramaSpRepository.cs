using System.Data;
using Dapper;
using Montesa.ProduccionPrograma.API.Data;
using Montesa.ProduccionPrograma.API.Models.DTOs;
using Montesa.ProduccionPrograma.API.Models.Requests;

namespace Montesa.ProduccionPrograma.API.Services
{
    public sealed class ProdProgramaSpRepository: IProdProgramaSpRepository
    {
        private readonly ISqlConnectionFactory _factory;

        public ProdProgramaSpRepository(ISqlConnectionFactory factory)
        {
            _factory = factory;
        }
        public async Task<IEnumerable<ProdProgramaSpRow>> LeerDesdeSpAsync(
         ConsultaFiltro filtro, CancellationToken ct = default)
        {
            // 🔁 Cambia por el nombre REAL de tu SP
            const string proc = "dbo.SP_ObtenerProduccionPorOP_NV";

            var ordNo = (filtro.OrdNo ?? string.Empty).PadLeft(8, '0');

            var p = new DynamicParameters();
            p.Add("@ord_no", ordNo, DbType.String, size: 8);
          
            using var conn = _factory.CreateConnection();
            var cmd = new CommandDefinition(proc, p, commandType: CommandType.StoredProcedure, cancellationToken: ct);
            return await conn.QueryAsync<ProdProgramaSpRow>(cmd);
        }
    }
}
