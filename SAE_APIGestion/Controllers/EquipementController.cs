using Microsoft.AspNetCore.Mvc;
using SAE_APIGestion.Models.EntityFramework;

namespace SAE_APIGestion.Controllers
{

    [Route("api/[controller]")]
    [ApiController]
    public class EquipementController : ControllerBase
    {

        private readonly IDataRepository<Equipement> dataRepository;

        public EquipementController(IDataRepository<Equipement> dataRepo)
        {
            dataRepository = dataRepo;
        }


        // GET: api/Equipement
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Equipement>>> GetEquipements()
        {
            return await dataRepository.GetAllAsync();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Equipement>> GetEquipement(int id)
        {
            var equipement = await dataRepository.GetByIdAsync(id);

            if (equipement == null)
            {
                return NotFound();
            }

            return equipement;
        }

        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> PutEquipement(int id, Equipement equipement)
        {
            if (id != equipement.EquipementId)
            {
                return BadRequest();
            }


            var equipementToUpdate = await dataRepository.GetByIdAsync(id);

            if (equipementToUpdate == null)
            {
                return NotFound();
            }
            else
            {
                await dataRepository.UpdateAsync(equipementToUpdate.Value, equipement);
                return NoContent();
            }
        }


        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Equipement>> PostEquipement(Equipement equipement)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            await dataRepository.AddAsync(equipement);

            return CreatedAtAction("GetEquipement", new { id = equipement.EquipementId }, equipement);
        }

        // DELETE: api/typeEquipement/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteEquipement(int id)
        {
            var equipement = await dataRepository.GetByIdAsync(id);
            if (equipement == null)
            {
                return NotFound();
            }

            await dataRepository.DeleteAsync(equipement.Value);

            return NoContent();
        }



    }
}
