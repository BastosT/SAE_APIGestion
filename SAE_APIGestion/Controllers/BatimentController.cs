using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using SAE_APIGestion.Models.DTO;
using SAE_APIGestion.Models.EntityFramework;

namespace SAE_APIGestion.Controllers
{

    [Route("api/[controller]")]
    [ApiController]
    public class BatimentController : ControllerBase
    {

        private readonly IDataRepository<Batiment> dataRepository;
        private readonly IMapper _mapper;

        public BatimentController(IDataRepository<Batiment> dataRepo, IMapper mapper = null)
        {
            dataRepository = dataRepo;
            _mapper = mapper;
        }


        [HttpGet]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<IEnumerable<Batiment>>> GetBatiments()
        {
            return await dataRepository.GetAllAsync();
        }


        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
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
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
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
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
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
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
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


        // =====================================================================================
        //DTO
        // =====================================================================================


        [HttpGet("dto")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<IEnumerable<BatimentDTO>>> GetBatimentsDTO()
        {
            var result = await dataRepository.GetAllAsync();
            var batiments = result.Value;
            if (batiments == null || !batiments.Any())
            {
                return NotFound();
            }
            var batimentsDTO = _mapper.Map<IEnumerable<BatimentDTO>>(batiments);
            return Ok(batimentsDTO);
        }

        [HttpGet("dto/{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<Batiment>> GetBatimentDTO(int id)
        {
            var batiment = await dataRepository.GetByIdAsync(id);
            if (batiment == null)
            {
                return NotFound();
            }
            var batimentDTO = _mapper.Map<BatimentDTO>(batiment.Value);
            return Ok(batimentDTO);
        }


        [HttpPut("dto/{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> PutBatimentDTO(int id, BatimentDTO batimentDto)
        {
            if (id != batimentDto.BatimentId)
            {
                return BadRequest();
            }

            var batimentToUpdate = await dataRepository.GetByIdAsync(id);
            if (batimentToUpdate == null)
            {
                return NotFound();
            }

            var batiment = _mapper.Map<Batiment>(batimentDto);
            await dataRepository.UpdateAsync(batimentToUpdate.Value, batiment);
            return NoContent();
        }


        [HttpPost("dto")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<Batiment>> PostBatimentDTO(BatimentDTO batimentDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            // Mapper le DTO vers l'entité
            var batiment = _mapper.Map<Batiment>(batimentDto);

            // Ajouter l'entité
            await dataRepository.AddAsync(batiment);

            // Mapper l'entité retournée vers un DTO pour la réponse
            var resultDto = _mapper.Map<BatimentDTO>(batiment);

            return CreatedAtAction("GetBatiment", new { id = resultDto.BatimentId }, resultDto);
        }

        [HttpDelete("dto/{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> DeleteBatimentDTO(int id)
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
