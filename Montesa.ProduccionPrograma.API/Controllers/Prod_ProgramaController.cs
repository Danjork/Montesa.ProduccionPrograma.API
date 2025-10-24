using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Montesa.ProduccionPrograma.API.Data;
using Montesa.ProduccionPrograma.API.Models;
using Montesa.ProduccionPrograma.API.Models.Requests;
using Montesa.ProduccionPrograma.API.Services;


namespace Montesa.ProduccionPrograma.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class Prod_ProgramaController : ControllerBase
    {
        private readonly IProdProgramaSpRepository _spRepo;
        private readonly ICargarProgramaService _cargaService;

        public Prod_ProgramaController(IProdProgramaSpRepository spRepo, ICargarProgramaService cargaService, IAsignacionMaquinaService asignacion)
        {
            _spRepo = spRepo;
            _cargaService = cargaService;
            _asignacion = asignacion;
        }

        [HttpPost("buscar-sp")]
        public async Task<IActionResult> BuscarSp([FromBody] ConsultaFiltro filtro, CancellationToken ct)
            => Ok(await _spRepo.LeerDesdeSpAsync(filtro, ct));

        public sealed class CargarInput
        {
            public string OrdNo { get; set; } = string.Empty;
            public string? Usuario { get; set; }
            public string? Equipo { get; set; }
        }

        [HttpPost("cargar-desde-sp")]
        public async Task<IActionResult> CargarDesdeSp([FromBody] CargarInput body, CancellationToken ct)
            => Ok(new { inserted = await _cargaService.CargarPorOpAsync(body.OrdNo, body.Usuario, body.Equipo, ct) });

        public sealed class AsignarInput
        {
            public string OrdNo { get; set; } = string.Empty;
            public int Linea { get; set; }
            public string Maquina { get; set; } = string.Empty;
            public short? Prioridad { get; set; }
            public string? Usuario { get; set; }
            public string? Equipo { get; set; }
        }

        private readonly IAsignacionMaquinaService _asignacion;

        //public Prod_ProgramaController(
        //    IProdProgramaSpRepository spRepo,
        //    ICargarProgramaService cargaService,
        //    IAsignacionMaquinaService asignacion)
        //{
        //    _spRepo = spRepo;
        //    _cargaService = cargaService;
        //    _asignacion = asignacion;
        //}

        [HttpPost("asignar-maquina")]
        public async Task<IActionResult> AsignarMaquina([FromBody] AsignarInput body, CancellationToken ct)
        {
            var (affected, message) = await _asignacion.AsignarAsync(
                body.OrdNo, body.Linea, body.Maquina, body.Prioridad, body.Usuario, body.Equipo, ct);

            return Ok(new { affected, message });
        }



    }
}
