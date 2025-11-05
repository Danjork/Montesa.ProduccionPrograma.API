using Montesa.ProduccionPrograma.API.Models;

namespace Montesa.ProduccionPrograma.API.Services
{
    public interface IProdMaquinaRepository
    {
        Task<IEnumerable<Prod_Maquina>> GetAllAsync();
        Task<Prod_Maquina?> GetByIdAsync(int recursoId);
    }
}
