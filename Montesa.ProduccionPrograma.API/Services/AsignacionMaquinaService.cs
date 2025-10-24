using Dapper;
using Montesa.ProduccionPrograma.API.Data;
using Montesa.ProduccionPrograma.API.Models;
using System.Data;

namespace Montesa.ProduccionPrograma.API.Services
{
    public sealed class AsignacionMaquinaService : IAsignacionMaquinaService
    {
        private readonly ISqlConnectionFactory _factory;
        private readonly IProgramaCargaRepository _cargaRepo;

        public AsignacionMaquinaService(ISqlConnectionFactory factory, IProgramaCargaRepository cargaRepo)
        {
            _factory = factory;
            _cargaRepo = cargaRepo;
        }

        public async Task<(int affected, string message)> AsignarAsync(
      string ordNo, int linea, string maquina, short? prioridad, string? usuario, string? equipo, CancellationToken ct = default)

        {
            // 1) Traer base desde TU TABLA (última fila por OP+Línea)
            const string sql = @"
SELECT TOP(1) *
FROM dbo.Produccion_Programa
WHERE OP = @op AND Linea = @linea
ORDER BY id DESC;";

            using var conn = _factory.CreateConnection();
            var cmd = new CommandDefinition(
    sql,
    new { op = ordNo.PadLeft(8, '0'), linea },
    commandType: CommandType.Text,
    cancellationToken: ct
);

            var baseRow = await conn.QuerySingleOrDefaultAsync<ProdPrograma>(cmd);


            if (baseRow is null)
                return (0, $"No existe OP {ordNo.PadLeft(8, '0')} en línea {linea}.");
            // o lanzar NotFound


            // 2) Preparar valores para el SP (el SP pide INTs)
            int ToInt(decimal? d) => (int)Math.Round(d ?? 0m);
            var nowTxt = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");

            // 3) Ejecutar el SP de carga/asignación
            var affected = await _cargaRepo.EjecutarCargaAsync(
                maquina: maquina,
                notaventa: baseRow.Notaventa ?? string.Empty,
                linea: baseRow.Linea ?? 0,
                op: (baseRow.OP ?? ordNo).PadLeft(8, '0'),
                vencimiento: baseRow.Vencimiento ?? string.Empty,
                cliente: (baseRow.Cliente ?? string.Empty).PadRight(40)[..40],
                codigo: baseRow.Codigo ?? string.Empty,
                descripcion: baseRow.Descripcion ?? string.Empty,
                solicitado: ToInt(baseRow.Solicitado),
                programado: ToInt(baseRow.Programado),
                metros: ToInt(baseRow.Metros),
                reportado: ToInt(baseRow.Reportado),
                faltante: ToInt(baseRow.Faltante),
                prioridad: prioridad ?? baseRow.Prioridad,
                fechaCarga: nowTxt,
                usuarioCarga: usuario,
                equipoCarga: equipo,
                status: baseRow.Status ?? "A",
                ct: ct
            );

            // devolver la TUPLA que exige la interfaz
            return (affected, affected > 0 ? "Asignado" : "Sin cambios");
        }
        }
    }
