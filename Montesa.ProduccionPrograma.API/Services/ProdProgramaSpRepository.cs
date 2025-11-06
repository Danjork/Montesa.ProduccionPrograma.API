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
            // Valor default para parámetros
            string ordNo = null;
            string nv1 = null;
            string nv2 = null;

            const string proc = "dbo.SP_ObtenerProduccionPorOP_NV";
            
            // Lógica según el filtro seleccionado
            if (filtro.Tipo == "OP")
            {
                ordNo = (filtro.OrdNo ?? string.Empty).PadLeft(8, '0');
            }
            else if (filtro.Tipo == "NV")
            {
                nv1 = nv2 = (filtro.OrdNo ?? string.Empty).PadLeft(8, '0'); // Asumiendo que OrdNo aquí es el número de NV
                ordNo = "00000000";
            }

            var p = new DynamicParameters();
            p.Add("@ord_no", ordNo, DbType.String, size: 8);
            p.Add("@nv1", nv1, DbType.String, size: 8);
            p.Add("@nv2", nv2, DbType.String, size: 8);

            using var conn = _factory.CreateConnection();
            var cmd = new CommandDefinition(proc, p, commandType: CommandType.StoredProcedure, cancellationToken: ct);
            return await conn.QueryAsync<ProdProgramaSpRow>(cmd);
        }
    }
}
