using Microsoft.AspNetCore.Mvc;
using SAE_APIGestion.Models.EntityFramework;

namespace SAE_APIGestion.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SalleController : ControllerBase
    {

        private readonly IDataRepository<Salle> dataRepository;

        public SalleController(IDataRepository<Salle> dataRepo)
        {
            dataRepository = dataRepo;
        }


        // GET: api/Salles
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Salle>>> GetSalles()
        {
            return await dataRepository.GetAllAsync();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Salle>> GetSalle(int id)
        {
            var salle = await dataRepository.GetByIdAsync(id);

            if (salle == null)
            {
                return NotFound();
            }

            return salle;
        }

        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> PutSalle(int id, Salle salle)
        {
            if (id != salle.SalleId)
            {
                return BadRequest();
            }


            var salleToUpdate = await dataRepository.GetByIdAsync(id);

            if (salleToUpdate == null)
            {
                return NotFound();
            }
            else
            {
                await dataRepository.UpdateAsync(salleToUpdate.Value, salle);
                return NoContent();
            }
        }


        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Salle>> PostSalle(Salle salle)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            await dataRepository.AddAsync(salle);

            return CreatedAtAction("GetSalle", new { id = salle.SalleId }, salle);
        }

        // DELETE: api/typeSalles/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteSalle(int id)
        {
            var salle = await dataRepository.GetByIdAsync(id);
            if (salle == null)
            {
                return NotFound();
            }

            await dataRepository.DeleteAsync(salle.Value);

            return NoContent();
        }

    }
}
