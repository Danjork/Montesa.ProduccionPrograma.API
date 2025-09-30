using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Montesa.ProduccionPrograma.API.Models
{
    [Table("Produccion_Programa", Schema = "dbo")]
    public class ProdPrograma
    { /// <summary>Identificador único del programa.</summary>
        [Key]
        public long id { get; set; }

        /// <summary>Nombre de la máquina asignada.</summary>
        [Required]
        [StringLength(50)]
        public string Maquina { get; set; } = string.Empty;

        /// <summary>Número de nota de venta asociada.</summary>
        public string? Notaventa { get; set; }

        /// <summary>Número de línea de producción.</summary>
        [Required]
        public int? Linea { get; set; }

        /// <summary>Número de orden de producción.</summary>
        [Required]
        [StringLength(8)]
        public string? OP { get; set; } = string.Empty;

        /// <summary>Fecha de vencimiento del programa.</summary>
        public string? Vencimiento { get; set; } = string.Empty;

        /// <summary>Nombre del cliente.</summary>
        [Required]
        [StringLength(40)]
        public string? Cliente { get; set; } = string.Empty;

        /// <summary>Código del producto.</summary>
        [Required]
        [StringLength(15)]
        public string? Codigo { get; set; } = string.Empty;

        /// <summary>Descripción del producto.</summary>
        [Required]
        [StringLength(40)]
        public string? Descripcion { get; set; } = string.Empty;

        /// <summary>Cantidad recibida.</summary>
        [Required]
        public decimal? Recibido { get; set; }

        /// <summary>Cantidad solicitada.</summary>
        [Required]
        public decimal? Solicitado { get; set; }

        /// <summary>Cantidad programada.</summary>
        [Required]
        public decimal? Programado { get; set; }

        /// <summary>Número de unidades.</summary>
        public int? Unidades { get; set; }

        /// <summary>Metros programados.</summary>
        public decimal? Metros { get; set; }

        /// <summary>Unidades no reportadas.</summary>
        public int? UnReportados { get; set; }

        /// <summary>Cantidad reportada.</summary>
        [Required]
        public decimal? Reportado { get; set; }

        /// <summary>Cantidad de scrap (merma).</summary>
        [Required]
        public decimal? Scrap { get; set; }

        /// <summary>Cantidad faltante.</summary>
        [Required]
        public decimal? Faltante { get; set; }

        /// <summary>Prioridad del programa.</summary>
        public short? Prioridad { get; set; }

        /// <summary>Fecha de inicio de la producción.</summary>
        public string? fechaInicio { get; set; }

        /// <summary>Hora de inicio de la producción.</summary>
        public string? horaInicio { get; set; }

        /// <summary>Fecha de carga de datos.</summary>
        public string? fechaCarga { get; set; }

        /// <summary>Usuario que realizó la carga.</summary>
        public string? usuarioCarga { get; set; }

        /// <summary>Equipo desde el que se realizó la carga.</summary>
        public string? equipoCarga { get; set; }

        /// <summary>Fecha de última actualización.</summary>
        public string? fechaActualizacion { get; set; }

        /// <summary>Usuario que realizó la última actualización.</summary>
        public string? usuarioActualizacion { get; set; }

        /// <summary>Equipo desde el que se realizó la última actualización.</summary>
        public string? equipoActualizacion { get; set; }

        /// <summary>Estado actual del programa.</summary>
        public string? Status { get; set; }

        /// <summary>Número de lote.</summary>
        public int? LoteNo { get; set; }

        /// <summary>Indicador de suficiencia de la orden.</summary>
        [Required]
        public int? VerSufi { get; set; }

        /// <summary>Descripción de la operación.</summary>
        public string? oper_desc { get; set; }

        /// <summary>ID de la máquina.</summary>
        public int? MaquID { get; set; }

        /// <summary>Fecha y hora de la última actualización.</summary>
        public DateTime? fecha_hora_act { get; set; }

        /// <summary>Cantidad devuelta.</summary>
        public decimal? Devueltos { get; set; }

        /// <summary>Diferencia de mermas.</summary>
        public decimal? mermas_dif { get; set; }
    }
}
