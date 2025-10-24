namespace Montesa.ProduccionPrograma.API.Services
{
    public interface IProgramaCargaRepository
    {
        Task<int> EjecutarCargaAsync(
            string maquina,
            string notaventa,
            int linea,
            string op,
            string vencimiento,
            string cliente,
            string codigo,
            string descripcion,
            int solicitado,
            int programado,
            int metros,
            int reportado,
            int faltante,
            short? prioridad,
            string fechaCarga,
            string? usuarioCarga,
            string? equipoCarga,
            string status,
            CancellationToken ct = default);
    }
}
