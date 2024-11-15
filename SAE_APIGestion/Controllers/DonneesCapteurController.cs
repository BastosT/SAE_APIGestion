using Microsoft.AspNetCore.Mvc;
using SAE_APIGestion.Models.EntityFramework;

namespace SAE_APIGestion.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DonneesCapteurController : ControllerBase
    {

        private readonly IDataRepository<DonneesCapteur> dataRepository;

        public DonneesCapteurController(IDataRepository<DonneesCapteur> dataRepo)
        {
            dataRepository = dataRepo;
        }


        [HttpGet]
        public async Task<ActionResult<IEnumerable<DonneesCapteur>>> GetDonneesCapteurs()
        {
            return await dataRepository.GetAllAsync();
        }


        [HttpGet("{id}")]
        public async Task<ActionResult<DonneesCapteur>> GetDonneesCapteur(int id)
        {
            var donneesCapteur = await dataRepository.GetByIdAsync(id);

            if (donneesCapteur == null)
            {
                return NotFound();
            }

            return donneesCapteur;
        }


        [HttpPut("{id}")]
        public async Task<IActionResult> PutDonneesCapteur(int id, DonneesCapteur donneesCapteur)
        {
            if (id != donneesCapteur.DonneesCapteurId)
            {
                return BadRequest();
            }



            var donneesCapteurToUpdate = await dataRepository.GetByIdAsync(id);

            if (donneesCapteurToUpdate == null)
            {
                return NotFound();
            }
            else
            {
                await dataRepository.UpdateAsync(donneesCapteurToUpdate.Value, donneesCapteur);
                return NoContent();
            }
        }


        [HttpPost]
        public async Task<ActionResult<DonneesCapteur>> PostDonneesCapteur(DonneesCapteur donneesCapteur)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            await dataRepository.AddAsync(donneesCapteur);

            return CreatedAtAction("GetDonneesCapteur", new { id = donneesCapteur.DonneesCapteurId }, donneesCapteur);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteDonneesCapteur(int id)
        {
            var donneesCapteur = await dataRepository.GetByIdAsync(id);
            if (donneesCapteur == null)
            {
                return NotFound();
            }

            await dataRepository.DeleteAsync(donneesCapteur.Value);


            return NoContent();
        }


    }
}
