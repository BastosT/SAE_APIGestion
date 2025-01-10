using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using SAE_APIGestion.Models.DTO;
using SAE_APIGestion.Models.EntityFramework;

namespace SAE_APIGestion.Controllers
{

    [Route("api/[controller]")]
    [ApiController]
    public class MurController : ControllerBase
    {
        private readonly IDataRepository<Mur> dataRepository;
        private readonly IMapper _mapper;

        public MurController(IDataRepository<Mur> dataRepo, IMapper mapper = null)
        {
            dataRepository = dataRepo;
            _mapper = mapper;
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

        // =====================================================================================
        //DTO
        // =====================================================================================

        // GET: api/Mur/dto
        [HttpGet("dto")]
        public async Task<ActionResult<IEnumerable<MurDTO>>> GetAllMurDTO()
        {
            var result = await dataRepository.GetAllAsync();
            var murs = result.Value;
            if (murs == null || !murs.Any())
            {
                return NotFound();
            }
            var murDto = _mapper.Map<IEnumerable<MurDTO>>(murs);
            return Ok(murDto);
        }


        // GET: api/Mur/dto/5
        [HttpGet("dto/{id}")]
        public async Task<ActionResult<MurDTO>> GetMurDTO(int id)
        {
            var mur = await dataRepository.GetByIdAsync(id);
            if (mur == null)
            {
                return NotFound();
            }
            var murDto = _mapper.Map<MurDTO>(mur.Value);
            return Ok(murDto);
        }


        [HttpPut("dto/{id}")]
        public async Task<IActionResult> PutMurDTO(int id, MurDTO murDto)
        {

            if (id != murDto.MurId)
            {
                return BadRequest("ID mismatch");
            }

            // Vérification si le capteur existe
            var murToUpdate = await dataRepository.GetByIdAsync(id);
            if (murToUpdate == null)
            {
                return NotFound("Mur not found");
            }

            // Mapping du DTO vers l'entité Capteur
            var mur = _mapper.Map<Mur>(murDto);


            // Mise à jour
            await dataRepository.UpdateAsync(murToUpdate.Value, mur);

            return NoContent();
        }


        [HttpPost("dto")]
        public async Task<ActionResult<CapteurDTO>> PostMurDTO(MurDTO murDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            // Mapper le DTO vers l'entité
            var mur = _mapper.Map<Mur>(murDto);

            // Ajouter l'entité
            await dataRepository.AddAsync(mur);

            // Mapper l'entité retournée vers un DTO pour la réponse
            var resultDto = _mapper.Map<MurDTO>(mur);

            return CreatedAtAction("GetMur", new { id = resultDto.MurId }, resultDto);
        }

        // DELETE: api/Mur/dto/5
        [HttpDelete("dto/{id}")]
        public async Task<IActionResult> DeleteMurDTO(int id)
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
