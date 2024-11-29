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
        public async Task<ActionResult<IEnumerable<TypeSalle>>> GetTypeSalles()
        {
            return await dataRepository.GetAllAsync();
        }


        // GET: api/Typesalles/automapper
        [HttpGet("automapper")]
        public async Task<ActionResult<IEnumerable<TypeSalleDTO>>> GetTypeSallesAutomapper()
        {
            // Récupération des produits
            var result = await dataRepository.GetAllAsync();
            var types = result.Value;

            // Vérifie si la liste est vide
            if (types == null || !types.Any())
            {
                return NotFound();
            }

            // Mappage de la liste des produits vers ProduitDto
            var typesDto = _mapper.Map<IEnumerable<TypeSalleDTO>>(types);

            // Retourne la liste des DTO
            return Ok(typesDto);
        }

        // GET: api/Typesalles/5
        [HttpGet("{id}")]
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


    }
}
