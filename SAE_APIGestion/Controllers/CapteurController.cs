using Microsoft.AspNetCore.Mvc;
using SAE_APIGestion.Models.EntityFramework;

namespace SAE_APIGestion.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CapteurController : ControllerBase
    {

        private readonly IDataRepository<Capteur> dataRepository;

        public CapteurController(IDataRepository<Capteur> dataRepo)
        {
            dataRepository = dataRepo;
        }


        [HttpGet]
        public async Task<ActionResult<IEnumerable<Capteur>>> GetCapteurs()
        {
            return await dataRepository.GetAllAsync();
        }


        [HttpGet("{id}")]
        public async Task<ActionResult<Capteur>> GetCapteur(int id)
        {
            var capteur = await dataRepository.GetByIdAsync(id);

            if (capteur == null)
            {
                return NotFound();
            }

            return capteur;
        }


        [HttpPut("{id}")]
        public async Task<IActionResult> PutCapteur(int id, Capteur capteur)
        {
            if (id != capteur.CapteurId)
            {
                return BadRequest();
            }



            var capteurToUpdate = await dataRepository.GetByIdAsync(id);

            if (capteurToUpdate == null)
            {
                return NotFound();
            }
            else
            {
                await dataRepository.UpdateAsync(capteurToUpdate.Value, capteur);
                return NoContent();
            }
        }


        [HttpPost]
        public async Task<ActionResult<Capteur>> PostCapteur(Capteur capteur)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            await dataRepository.AddAsync(capteur);

            return CreatedAtAction("GetCapteur", new { id = capteur.CapteurId }, capteur);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCapteur(int id)
        {
            var capteur = await dataRepository.GetByIdAsync(id);
            if (capteur == null)
            {
                return NotFound();
            }

            await dataRepository.DeleteAsync(capteur.Value);


            return NoContent();
        }


    }
}
