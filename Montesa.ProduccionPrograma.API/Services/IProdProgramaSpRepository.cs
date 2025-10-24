using Montesa.ProduccionPrograma.API.Models.DTOs;
using Montesa.ProduccionPrograma.API.Models.Requests;



namespace Montesa.ProduccionPrograma.API.Services
{
    public interface IProdProgramaSpRepository
    {
        Task<IEnumerable<ProdProgramaSpRow>> LeerDesdeSpAsync(ConsultaFiltro filtro, CancellationToken ct = default);
    }

}
