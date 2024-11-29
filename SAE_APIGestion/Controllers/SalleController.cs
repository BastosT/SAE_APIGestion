using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using SAE_APIGestion.Models.DTO;
using SAE_APIGestion.Models.EntityFramework;

namespace SAE_APIGestion.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SalleController : ControllerBase
    {

        private readonly IDataRepository<Salle> dataRepository;
        private readonly IMapper _mapper;


        public SalleController(IDataRepository<Salle> dataRepo, IMapper mapper = null)
        {
            dataRepository = dataRepo;
            _mapper = mapper;
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




        // =====================================================================================
        //DTO
        // =====================================================================================

        [HttpGet("dto")]
        public async Task<ActionResult<IEnumerable<SalleDTO>>> GetSallesDTO()
        {
            var result = await dataRepository.GetAllAsync();
            var salles = result.Value;
            if (salles == null || !salles.Any())
            {
                return NotFound();
            }
            var sallesDto = _mapper.Map<IEnumerable<SalleDTO>>(salles);
            return Ok(sallesDto);
        }

        [HttpGet("dto/{id}")]
        public async Task<ActionResult<SalleDTO>> GetSalleDTO(int id)
        {
            var salle = await dataRepository.GetByIdAsync(id);
            if (salle == null)
            {
                return NotFound();
            }
            var salleDto = _mapper.Map<SalleDTO>(salle.Value);
            return Ok(salleDto);
        }

        [HttpPut("dto/{id}")]
        public async Task<IActionResult> PutSalleDTO(int id, SalleDTO salleDto)
        {
            if (id != salleDto.SalleId)
            {
                return BadRequest();
            }

            var salleToUpdate = await dataRepository.GetByIdAsync(id);
            if (salleToUpdate == null)
            {
                return NotFound();
            }

            var salle = _mapper.Map<Salle>(salleDto);
            await dataRepository.UpdateAsync(salleToUpdate.Value, salle);
            return NoContent();
        }

        [HttpPost("dto")]
        public async Task<ActionResult<SalleDTO>> PostSalleDTO(SalleDTO salleDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            // Mapper le DTO vers l'entité Salle
            var salle = _mapper.Map<Salle>(salleDto);

            // Ajouter l'entité à la base de données
            await dataRepository.AddAsync(salle);

            // Mapper l'entité retournée vers un DTO pour la réponse
            var resultDto = _mapper.Map<SalleDTO>(salle);

            return CreatedAtAction("GetSalle", new { id = resultDto.SalleId }, resultDto);
        }

        // DELETE: api/typeSalles/5
        [HttpDelete("dto/{id}")]
        public async Task<IActionResult> DeleteSalleDTO(int id)
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
