using Microsoft.AspNetCore.Mvc;
using SAE_APIGestion.Models.EntityFramework;

namespace SAE_APIGestion.Controllers
{

    [Route("api/[controller]")]
    [ApiController]
    public class TypeEquipementController : ControllerBase
    {
        private readonly IDataRepository<TypeEquipement> dataRepository;

        public TypeEquipementController(IDataRepository<TypeEquipement> dataRepo)
        {
            dataRepository = dataRepo;
        }


        // GET: api/TypeEquipements
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<IEnumerable<TypeEquipement>>> GetTypeEquipements()
        {
            return await dataRepository.GetAllAsync();
        }

        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<TypeEquipement>> GetTypeEquipement(int id)
        {
            var typeEquipement = await dataRepository.GetByIdAsync(id);

            if (typeEquipement == null)
            {
                return NotFound();
            }

            return typeEquipement;
        }

        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> PutTypeEquipement(int id, TypeEquipement typeEquipement)
        {
            if (id != typeEquipement.TypeEquipementId)
            {
                return BadRequest();
            }



            var typeEquipementToUpdate = await dataRepository.GetByIdAsync(id);

            if (typeEquipementToUpdate == null)
            {
                return NotFound();
            }
            else
            {
                await dataRepository.UpdateAsync(typeEquipementToUpdate.Value, typeEquipement);
                return NoContent();
            }
        }


        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<TypeEquipement>> PostTypeEquipement(TypeEquipement typeEquipement)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            await dataRepository.AddAsync(typeEquipement);

            return CreatedAtAction("GetTypeEquipement", new { id = typeEquipement.TypeEquipementId }, typeEquipement);
        }

        // DELETE: api/typeSalles/5
        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> DeleteTypeEquipement(int id)
        {
            var typeEquipement = await dataRepository.GetByIdAsync(id);
            if (typeEquipement == null)
            {
                return NotFound();
            }

            await dataRepository.DeleteAsync(typeEquipement.Value);
            
            return NoContent();
        }



    }
}
