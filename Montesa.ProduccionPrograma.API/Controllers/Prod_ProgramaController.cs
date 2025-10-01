using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Montesa.ProduccionPrograma.API.Data;
using Montesa.ProduccionPrograma.API.Models;

namespace Montesa.ProduccionPrograma.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]

    public class Prod_ProgramaController : Controller
    {
        private readonly ProduccionDBContext _context;

        public Prod_ProgramaController(ProduccionDBContext context)
        {
            _context = context;
        }

        [HttpGet]
        public ActionResult<List<ProdPrograma> >Index()
        {
            try
            {
                return Ok(_context.prodProgramas.ToList());
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("{id}", Name = "GetProduccion")]
        public ActionResult Get(int id)
        {
            try
            {
                var programa = _context.prodProgramas.FirstOrDefault(p => p.id == id);
                return Ok(programa);
            }
            catch (Exception ex)
            {

                return BadRequest(ex.Message);
            }
        }

        [HttpPost]
        public ActionResult Post([FromBody] ProdPrograma programa)
        {
            try
            {
                if (programa == null)
                {
                    return BadRequest("Invalid data.");
                }
                _context.prodProgramas.Add(programa);
                _context.SaveChanges();
                return CreatedAtAction("GetProduccion", new { id = programa.id }, programa);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut("{id}")]
        public ActionResult Put(int id, [FromBody] ProdPrograma programa)
        {

            if (programa is null)
                return BadRequest("Body vacío.");

            if (id != programa.id)
                return BadRequest("El id de la URL no coincide con el del cuerpo.");

            try
            {
                _context.Entry(programa).State = EntityState.Modified;
                _context.SaveChanges();
                return NoContent(); // 204 para PUT
                                    // o: return Ok(programa);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);

            }
        }

        [HttpDelete("{id}")]
        public ActionResult Delete(int id)
        {
            try
            {
                var programa = _context.prodProgramas.FirstOrDefault(p => p.id == id);
                if (programa != null)
                {
                    _context.prodProgramas.Remove(programa);
                    _context.SaveChanges();
                    return Ok(id);
                }
                else
                {
                    return BadRequest();
                }


            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

        }

    }
}
