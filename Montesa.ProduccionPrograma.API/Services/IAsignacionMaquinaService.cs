namespace Montesa.ProduccionPrograma.API.Services
{
    public interface IAsignacionMaquinaService
    {
        Task<(int affected, string message)> AsignarAsync(
            string ordNo,
            int linea,
            string maquina,
            short? prioridad,
            string? usuario,
            string? equipo,
            CancellationToken ct = default);
    }
}
