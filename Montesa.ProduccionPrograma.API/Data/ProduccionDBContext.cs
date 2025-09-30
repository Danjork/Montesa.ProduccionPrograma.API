using Microsoft.EntityFrameworkCore;
using Montesa.ProduccionPrograma.API.Models;

namespace Montesa.ProduccionPrograma.API.Data
{
    public class ProduccionDBContext: DbContext
    {
        public ProduccionDBContext(DbContextOptions<ProduccionDBContext> options) : base(options)
        { 
        }

        public DbSet<ProdPrograma> prodProgramas { get; set; }
    }
}
