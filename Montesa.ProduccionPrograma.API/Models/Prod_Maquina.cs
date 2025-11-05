using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Montesa.ProduccionPrograma.API.Models
{
    [Table("Produccion_Maquina", Schema ="dbo")]
    public class Prod_Maquina
    {
        [Key]
        public int RecursoId { get; set; }

        public string Nombre { get; set; }
        public string parametro1Nombre { get; set; }
        public string parametro1 { get; set; }
        public string Proceso { get; set; }
        public int Recibir { get; set; }
        public string Area { get; set; }
        public string Alias { get; set; }


    }
}
