using Microsoft.AspNetCore.Mvc;
using SAE_APIGestion.Models.EntityFramework;

namespace SAE_APIGestion.Controllers
{

    [Route("api/[controller]")]
    [ApiController]
    public class MurController : ControllerBase
    {
        private readonly IDataRepository<Mur> dataRepository;

        public MurController(IDataRepository<Mur> dataRepo)
        {
            dataRepository = dataRepo;
        }


        // GET: api/Murs
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Mur>>> GetMurs()
        {
            return await dataRepository.GetAllAsync();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Mur>> GetMur(int id)
        {
            var mur = await dataRepository.GetByIdAsync(id);

            if (mur == null)
            {
                return NotFound();
            }

            return mur;
        }

        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> PutMur(int id, Mur mur)
        {
            if (id != mur.MurId)
            {
                return BadRequest();
            }


            var murToUpdate = await dataRepository.GetByIdAsync(id);

            if (murToUpdate == null)
            {
                return NotFound();
            }
            else
            {
                await dataRepository.UpdateAsync(murToUpdate.Value, mur);
                return NoContent();
            }
        }


        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Mur>> PostMur(Mur mur)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            await dataRepository.AddAsync(mur);

            return CreatedAtAction("GetMur", new { id = mur.MurId }, mur);
        }

        // DELETE: api/typeMurs/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteMur(int id)
        {
            var mur = await dataRepository.GetByIdAsync(id);
            if (mur == null)
            {
                return NotFound();
            }

            await dataRepository.DeleteAsync(mur.Value);

            return NoContent();
        }



    }
}
