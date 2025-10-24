namespace Montesa.ProduccionPrograma.API.Services
{
    public interface ICargarProgramaService
    {
        Task<int> CargarPorOpAsync(string ordNo, string? usuario = null, string? equipo = null, CancellationToken ct = default);
    }
}
