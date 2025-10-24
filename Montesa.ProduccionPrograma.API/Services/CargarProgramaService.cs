using Dapper;
using System.Data;
using Montesa.ProduccionPrograma.API.Data;
using Montesa.ProduccionPrograma.API.Models;
using Montesa.ProduccionPrograma.API.Models.DTOs;
using Montesa.ProduccionPrograma.API.Models.Requests;


namespace Montesa.ProduccionPrograma.API.Services
{
    public sealed class CargarProgramaService : ICargarProgramaService
    {
        private readonly ISqlConnectionFactory _factory;
        private readonly IProdProgramaSpRepository _sp;

        public CargarProgramaService(ISqlConnectionFactory factory, IProdProgramaSpRepository sp)
        {
            _factory = factory;
            _sp = sp;
        }

        public async Task<int> CargarPorOpAsync(string ordNo, string? usuario = null, string? equipo = null, CancellationToken ct = default)
        {
            // 1) Obtener filas desde el SP
            var filas = await _sp.LeerDesdeSpAsync(new Models.Requests.ConsultaFiltro { OrdNo = ordNo }, ct);
            var lista = filas.ToList();
            if (lista.Count == 0) return 0;

            // 2) Abrir conexión + transacción
            using var conn = _factory.CreateConnection();
            await (conn as System.Data.Common.DbConnection)!.OpenAsync(ct);
            using var tx = conn.BeginTransaction();

            try
            {
                // 3) Borrar existentes de esa OP (reemplazo simple)
                const string sqlDelete = @"DELETE FROM dbo.Produccion_Programa WHERE OP = @OP;";
                await conn.ExecuteAsync(new CommandDefinition(sqlDelete, new { OP = ordNo.PadLeft(8, '0') }, transaction: tx, cancellationToken: ct));

                // 4) Insertar nuevas filas
                const string sqlInsert = @"
INSERT INTO dbo.Produccion_Programa
( Maquina, Notaventa, Linea, OP, Vencimiento, Cliente, Codigo, Descripcion,
  Recibido, Solicitado, Programado, Unidades, Metros, UnReportados, Reportado, Scrap,
  Faltante, Prioridad, fechaInicio, horaInicio, fechaCarga, usuarioCarga, equipoCarga,
  fechaActualizacion, usuarioActualizacion, equipoActualizacion, Status, LoteNo, VerSufi,
  oper_desc, MaquID, fecha_hora_act, Devueltos, mermas_dif )
VALUES
( @Maquina, @Notaventa, @Linea, @OP, @Vencimiento, @Cliente, @Codigo, @Descripcion,
  @Recibido, @Solicitado, @Programado, @Unidades, @Metros, @UnReportados, @Reportado, @Scrap,
  @Faltante, @Prioridad, @fechaInicio, @horaInicio, @fechaCarga, @usuarioCarga, @equipoCarga,
  @fechaActualizacion, @usuarioActualizacion, @equipoActualizacion, @Status, @LoteNo, @VerSufi,
  @oper_desc, @MaquID, @fecha_hora_act, @Devueltos, @mermas_dif );";

                // defaults para columnas NOT NULL
                var ahoraTxt = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");

                foreach (var r in lista)
                {
                    var e = MapToEntity(r, ordNo, usuario, equipo, ahoraTxt);

                    var cmd = new CommandDefinition(sqlInsert, e, transaction: tx, cancellationToken: ct);
                    await conn.ExecuteAsync(cmd);
                }

                tx.Commit();
                return lista.Count;
            }
            catch
            {
                tx.Rollback();
                throw;
            }
        }

        // ---------- Helpers ----------
        private static ProdPrograma MapToEntity(ProdProgramaSpRow r, string ordNo, string? usuario, string? equipo, string fechaTxt)
        {
            return new ProdPrograma
            {
                Maquina = r.maquina ?? string.Empty,
                Notaventa = r.notaventa ?? string.Empty,
                Linea = r.linea ?? 0,
                OP = (r.OP ?? ordNo).PadLeft(8, '0'),
                Vencimiento = r.Vencimiento ?? string.Empty,
                Cliente = (r.cliente ?? string.Empty).PadRight(10).Substring(0, Math.Min(10, (r.cliente ?? "").Length)), // tu columna es char(10)
                Codigo = r.codigo ?? string.Empty,
                Descripcion = r.descripcion ?? string.Empty,

                // Decimales NOT NULL → default 0 si vienen null
                Recibido = 0m,
                Solicitado = r.Solicitado ?? 0m,
                Programado = r.Programado ?? 0m,
                Reportado = r.Reportado ?? 0m,
                Scrap = 0m,
                Faltante = r.faltante ?? 0m,

                // Otros
                Unidades = null,
                Metros = ParseDecimalSafe(r.metros),   // convierte "", "-" → null
                UnReportados = null,
                Prioridad = ParseShortSafe(r.prioridad) ?? 0,
                fechaInicio = null,
                horaInicio = null,

                fechaCarga = fechaTxt,
                usuarioCarga = usuario,
                equipoCarga = equipo,
                fechaActualizacion = fechaTxt,
                usuarioActualizacion = usuario,
                equipoActualizacion = equipo,

                Status = null,
                LoteNo = null,
                VerSufi = 0,           // NOT NULL → 0
                oper_desc = null,
                MaquID = null,
                fecha_hora_act = DateTime.Now,
                Devueltos = null,
                mermas_dif = null
            };
        }

        private static decimal? ParseDecimalSafe(string? s)
        {
            if (string.IsNullOrWhiteSpace(s)) return null;
            s = s.Trim();
            if (s == "-") return null;
            if (decimal.TryParse(s, out var d)) return d;
            return null;
        }

        private static short? ParseShortSafe(string? s)
        {
            if (string.IsNullOrWhiteSpace(s)) return null;
            s = s.Trim();
            if (s == "-") return null;
            return short.TryParse(s, out var v) ? v : (short?)null;
        }
    }
}