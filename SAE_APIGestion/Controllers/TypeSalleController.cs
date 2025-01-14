using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using SAE_APIGestion.Models.DTO;
using SAE_APIGestion.Models.EntityFramework;

namespace SAE_APIGestion.Controllers
{

    [Route("api/[controller]")]
    [ApiController]
    public class TypeSalleController : ControllerBase
    {

        private readonly IDataRepository<TypeSalle> dataRepository;
        private readonly IMapper _mapper;

        public TypeSalleController(IDataRepository<TypeSalle> dataRepo, IMapper mapper = null)
        {
            dataRepository = dataRepo;
            _mapper = mapper;

        }


        // GET: api/Typesalles
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<IEnumerable<TypeSalle>>> GetTypeSalles()
        {
            return await dataRepository.GetAllAsync();
        }


        // GET: api/Typesalles/5
        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<TypeSalle>> GetTypeSalle(int id)
        {
            var typeSalle = await dataRepository.GetByIdAsync(id);

            if (typeSalle == null)
            {
                return NotFound();
            }

            return typeSalle;
        }


        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> PutTypeSalle(int id, TypeSalle typeSalle)
        {
            if (id != typeSalle.TypeSalleId)
            {
                return BadRequest();
            }



            var typeSalleToUpdate = await dataRepository.GetByIdAsync(id);

            if (typeSalleToUpdate == null)
            {
                return NotFound();
            }
            else
            {
                await dataRepository.UpdateAsync(typeSalleToUpdate.Value, typeSalle);
                return NoContent();
            }
        }


        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<TypeSalle>> PostTypeSalle(TypeSalle typeSalle)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            await dataRepository.AddAsync(typeSalle);

            return CreatedAtAction("GetTypeSalle", new { id = typeSalle.TypeSalleId }, typeSalle);
        }

        // DELETE: api/typeSalles/5
        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> DeleteTypeSalle(int id)
        {
            var typeSalle = await dataRepository.GetByIdAsync(id);
            if (typeSalle == null)
            {
                return NotFound();
            }

            await dataRepository.DeleteAsync(typeSalle.Value);


            return NoContent();
        }


        // =====================================================================================
        //DTO
        // =====================================================================================


        // GET: api/Typesalles
        [HttpGet("dto")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<IEnumerable<TypeSalleDTO>>> GetTypeSallesDTO()
        {
            var result = await dataRepository.GetAllAsync();
            var types = result.Value;
            if (types == null || !types.Any())
            {
                return NotFound();
            }
            var typesDto = _mapper.Map<IEnumerable<TypeSalleDTO>>(types);
            return Ok(typesDto);
        }


        // GET: api/Typesalles/5
        [HttpGet("dto/{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<TypeSalleDTO>> GetTypeSalleDTO(int id)
        {
            var type = await dataRepository.GetByIdAsync(id);
            if (type == null)
            {
                return NotFound();
            }
            var salleDto = _mapper.Map<TypeSalleDTO>(type.Value);
            return Ok(salleDto);
        }


        [HttpPut("dto/{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> PutTypeSalleDTO(int id, TypeSalleDTO typeSalleDto)
        {
            if (id != typeSalleDto.TypeSalleId)
            {
                return BadRequest();
            }

            var typeToUpdate = await dataRepository.GetByIdAsync(id);
            if (typeToUpdate == null)
            {
                return NotFound();
            }

            var type = _mapper.Map<TypeSalle>(typeSalleDto);
            await dataRepository.UpdateAsync(typeToUpdate.Value, type);
            return NoContent();
        }

        [HttpPost("dto")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<TypeSalleDTO>> PostTypeSalleDTO(TypeSalleDTO typeSalleDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            // Mapper le DTO vers l'entité
            var typeSalle = _mapper.Map<TypeSalle>(typeSalleDto);

            // Ajouter l'entité
            await dataRepository.AddAsync(typeSalle);

            // Mapper l'entité retournée vers un DTO pour la réponse
            var resultDto = _mapper.Map<TypeSalleDTO>(typeSalle);

            return CreatedAtAction("GetTypeSalle", new { id = resultDto.TypeSalleId }, resultDto);
        }

        // DELETE: api/typeSalles/5
        [HttpDelete("dto/{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> DeleteTypeSalleDTO(int id)
        {
            var typeSalle = await dataRepository.GetByIdAsync(id);
            if (typeSalle == null)
            {
                return NotFound();
            }

            await dataRepository.DeleteAsync(typeSalle.Value);


            return NoContent();
        }

    }
}
