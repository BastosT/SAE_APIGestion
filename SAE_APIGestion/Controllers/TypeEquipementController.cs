using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using SAE_APIGestion.Models.DTO;
using SAE_APIGestion.Models.EntityFramework;

namespace SAE_APIGestion.Controllers
{

    [Route("api/[controller]")]
    [ApiController]
    public class TypeEquipementController : ControllerBase
    {
        private readonly IDataRepository<TypeEquipement> dataRepository;
        private readonly IMapper _mapper;

        public TypeEquipementController(IDataRepository<TypeEquipement> dataRepo, IMapper mapper = null)
        {
            dataRepository = dataRepo;
            _mapper = mapper;
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

        // DELETE: api/TypeEquipements/5
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


        // =====================================================================================
        //DTO
        // =====================================================================================


        // GET: api/TypeEquipement/dto
        [HttpGet("dto")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<IEnumerable<TypeEquipementDTO>>> GetTypeEquipementsDTO()
        {
            var result = await dataRepository.GetAllAsync();
            var types = result.Value;
            if (types == null || !types.Any())
            {
                return NotFound();
            }
            var typesDto = _mapper.Map<IEnumerable<TypeEquipementDTO>>(types);
            return Ok(typesDto);
        }


        // GET: api/TypeEquipement/dto/5
        [HttpGet("dto/{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<TypeEquipementDTO>> GetTypeEquipementDTO(int id)
        {
            var type = await dataRepository.GetByIdAsync(id);
            if (type == null)
            {
                return NotFound();
            }
            var typeEquipementDto = _mapper.Map<TypeEquipementDTO>(type.Value);
            return Ok(typeEquipementDto);
        }


        [HttpPut("dto/{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> PutTypeEquipementDTO(int id, TypeEquipementDTO typeEquipementDto)
        {
            if (id != typeEquipementDto.TypeEquipementId)
            {
                return BadRequest();
            }

            var typeToUpdate = await dataRepository.GetByIdAsync(id);
            if (typeToUpdate == null)
            {
                return NotFound();
            }

            var type = _mapper.Map<TypeEquipement>(typeEquipementDto);
            await dataRepository.UpdateAsync(typeToUpdate.Value, type);
            return NoContent();
        }

        [HttpPost("dto")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<TypeEquipementDTO>> PostTypeEquipementDTO(TypeEquipementDTO typeEquipementDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            // Mapper le DTO vers l'entité
            var typeEquipement = _mapper.Map<TypeEquipement>(typeEquipementDto);

            // Ajouter l'entité
            await dataRepository.AddAsync(typeEquipement);

            // Mapper l'entité retournée vers un DTO pour la réponse
            var resultDto = _mapper.Map<TypeEquipementDTO>(typeEquipement);

            return CreatedAtAction("GetTypeEquipement", new { id = resultDto.TypeEquipementId }, resultDto);
        }

        // DELETE: api/typeEquipement/5
        [HttpDelete("dto/{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> DeleteTypeEquipementDTO(int id)
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
