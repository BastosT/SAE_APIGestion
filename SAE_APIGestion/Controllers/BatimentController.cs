using Microsoft.AspNetCore.Mvc;
using SAE_APIGestion.Models.EntityFramework;

namespace SAE_APIGestion.Controllers
{
    public class BatimentController : ControllerBase
    {

        private readonly IDataRepository<Batiment> dataRepository;

        public BatimentController(IDataRepository<Batiment> dataRepo)
        {
            dataRepository = dataRepo;
        }


        [HttpGet]
        public async Task<ActionResult<IEnumerable<Batiment>>> GetBatiments()
        {
            return await dataRepository.GetAllAsync();
        }


        [HttpGet("{id}")]
        public async Task<ActionResult<Batiment>> GetBatiment(int id)
        {
            var batiment = await dataRepository.GetByIdAsync(id);

            if (batiment == null)
            {
                return NotFound();
            }

            return batiment;
        }


        [HttpPut("{id}")]
        public async Task<IActionResult> PutBatiment(int id, Batiment batiment)
        {
            if (id != batiment.BatimentId)
            {
                return BadRequest();
            }



            var batimentToUpdate = await dataRepository.GetByIdAsync(id);

            if (batimentToUpdate == null)
            {
                return NotFound();
            }
            else
            {
                await dataRepository.UpdateAsync(batimentToUpdate.Value, batiment);
                return NoContent();
            }
        }


        [HttpPost]
        public async Task<ActionResult<Batiment>> PostBatiment(Batiment batiment)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            await dataRepository.AddAsync(batiment);

            return CreatedAtAction("GetBatiment", new { id = batiment.BatimentId }, batiment);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteBatiment(int id)
        {
            var batiment = await dataRepository.GetByIdAsync(id);
            if (batiment == null)
            {
                return NotFound();
            }

            await dataRepository.DeleteAsync(batiment.Value);


            return NoContent();
        }


    }
}
