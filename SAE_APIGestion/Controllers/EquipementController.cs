using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using SAE_APIGestion.Models.DTO;
using SAE_APIGestion.Models.EntityFramework;

namespace SAE_APIGestion.Controllers
{

    [Route("api/[controller]")]
    [ApiController]
    public class EquipementController : ControllerBase
    {

        private readonly IDataRepository<Equipement> dataRepository;
        private readonly IMapper _mapper;

        public EquipementController(IDataRepository<Equipement> dataRepo, IMapper mapper = null)
        {
            dataRepository = dataRepo;
            _mapper = mapper;
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

        // DELETE: api/Equipement/5
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

        // =====================================================================================
        //DTO
        // =====================================================================================

        // GET: api/Equipement/dto
        [HttpGet("dto")]
        public async Task<ActionResult<IEnumerable<EquipementDTO>>> GetEquipementDTO()
        {
            var result = await dataRepository.GetAllAsync();
            var equipements = result.Value;
            if (equipements == null || !equipements.Any())
            {
                return NotFound();
            }
            var typesDto = _mapper.Map<IEnumerable<EquipementDTO>>(equipements);
            return Ok(typesDto);
        }


        // GET: api/Equipement/dto/5
        [HttpGet("dto/{id}")]
        public async Task<ActionResult<EquipementDTO>> GetEquipementDTO(int id)
        {
            var equipement = await dataRepository.GetByIdAsync(id);
            if (equipement == null)
            {
                return NotFound();
            }
            var salleDto = _mapper.Map<EquipementDTO>(equipement.Value);
            return Ok(salleDto);
        }


        [HttpPut("dto/{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> PutEquipementDTO(int id, EquipementDTO equipementDto)
        {
            if (id != equipementDto.EquipementId)
            {
                return BadRequest();
            }

            var equipementToUpdate = await dataRepository.GetByIdAsync(id);
            if (equipementToUpdate == null)
            {
                return NotFound();
            }

            var equipement = _mapper.Map<Equipement>(equipementDto);
            await dataRepository.UpdateAsync(equipementToUpdate.Value, equipement);
            return NoContent();
        }

        [HttpPost("dto")]
        public async Task<ActionResult<EquipementDTO>> PostEquipementDTO(EquipementDTO equipementDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            // Mapper le DTO vers l'entité
            var equipement = _mapper.Map<Equipement>(equipementDto);

            // Ajouter l'entité
            await dataRepository.AddAsync(equipement);

            // Mapper l'entité retournée vers un DTO pour la réponse
            var resultDto = _mapper.Map<EquipementDTO>(equipement);

            return CreatedAtAction("GetEquipement", new { id = resultDto.EquipementId }, resultDto);
        }

        // DELETE: api/Equipement/dto/5
        [HttpDelete("dto/{id}")]
        public async Task<IActionResult> DeleteEquipementDTO(int id)
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
