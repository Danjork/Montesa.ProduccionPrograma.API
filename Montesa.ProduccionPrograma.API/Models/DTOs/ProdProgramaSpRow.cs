using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Montesa.ProduccionPrograma.API.Models.DTOs
{
    public sealed class ProdProgramaSpRow
    {
        public string? notaventa { get; set; }
        public int? linea { get; set; }
        public string? OP { get; set; }
        public string? Vencimiento { get; set; }
        public string? cliente { get; set; }
        public string? codigo { get; set; }
        public string? descripcion { get; set; }
        public decimal? Solicitado { get; set; }
        public decimal? Programado { get; set; }
        public string? metros { get; set; }      // ← viene como texto (a veces vacío o “-”)
        public decimal? Reportado { get; set; }  // puede venir -1 (ok)
        public decimal? faltante { get; set; }
        public string? prioridad { get; set; }
        public string? maquina { get; set; }
    }
    
}
