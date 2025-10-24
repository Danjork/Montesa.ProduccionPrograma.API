using Dapper;
using Montesa.ProduccionPrograma.API.Data;
using System.Data;

namespace Montesa.ProduccionPrograma.API.Services
{
    public sealed class ProgramaCargaRepository : IProgramaCargaRepository
    {
        private readonly ISqlConnectionFactory _factory;
        public ProgramaCargaRepository(ISqlConnectionFactory factory) => _factory = factory;

        public async Task<int> EjecutarCargaAsync(
            string maquina, string notaventa, int linea, string op, string vencimiento,
            string cliente, string codigo, string descripcion,
            int solicitado, int programado, int metros, int reportado, int faltante,
            short? prioridad, string fechaCarga, string? usuarioCarga, string? equipoCarga,
            string status, CancellationToken ct = default)
        {
            const string proc = "dbo.PP_CargaPrograma";

            var p = new DynamicParameters();
            p.Add("@Maquina", maquina, DbType.String, size: 50);
            p.Add("@Notaventa", notaventa, DbType.StringFixedLength, size: 8);
            p.Add("@Linea", linea, DbType.Int32);
            p.Add("@OP", op, DbType.StringFixedLength, size: 8);
            p.Add("@Vencimiento", vencimiento, DbType.StringFixedLength, size: 10);
            p.Add("@Cliente", cliente, DbType.StringFixedLength, size: 40);
            p.Add("@Codigo", codigo, DbType.String, size: 15);
            p.Add("@Descripcion", descripcion, DbType.String, size: 40);
            p.Add("@Solicitado", solicitado, DbType.Int32);
            p.Add("@Programado", programado, DbType.Int32);
            p.Add("@Metros", metros, DbType.Int32);
            p.Add("@Reportado", reportado, DbType.Int32);
            p.Add("@faltante", faltante, DbType.Int32);
            p.Add("@Prioridad", prioridad, DbType.Int16);
            p.Add("@fechaCarga", fechaCarga, DbType.String, size: 50);
            p.Add("@usuarioCarga", usuarioCarga, DbType.String, size: 20);
            p.Add("@equipoCarga", equipoCarga, DbType.String, size: 50);
            p.Add("@status", status, DbType.StringFixedLength, size: 1);

            using var conn = _factory.CreateConnection();
            var cmd = new CommandDefinition(proc, p, commandType: CommandType.StoredProcedure, cancellationToken: ct);
            // Devuelve #filas afectadas (o 0/1 según diseño del SP)
            return await conn.ExecuteAsync(cmd);
        }
    }
}
