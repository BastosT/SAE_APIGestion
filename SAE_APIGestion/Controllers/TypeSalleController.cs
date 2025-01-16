using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using SAE_APIGestion.Models.DTO;
using SAE_APIGestion.Models.EntityFramework;

namespace SAE_APIGestion.Controllers
{

    /// <summary>
    /// Contrôleur API pour gérer les entités <see cref="TypeSalle"/>.
    /// Fournit des points de terminaison pour effectuer des opérations CRUD sur les types de salles.
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class TypeSalleController : ControllerBase
    {

        private readonly IDataRepository<TypeSalle> dataRepository;
        private readonly IMapper _mapper;

        /// <summary>
        /// Constructeur permettant d'initialiser le contrôleur avec un repository et un mappage AutoMapper.
        /// </summary>
        /// <param name="dataRepo">Le repository permettant d'accéder aux données des <see cref="TypeSalle"/>.</param>
        /// <param name="mapper">Le service AutoMapper utilisé pour la conversion entre entités et DTO.</param>
        public TypeSalleController(IDataRepository<TypeSalle> dataRepo, IMapper mapper = null)
        {
            dataRepository = dataRepo;
            _mapper = mapper;

        }


        // GET: api/Typesalles
        /// <summary>
        /// Récupère tous les types de salles dans la base de données.
        /// </summary>
        /// <returns>Une liste des types de salles avec un statut HTTP approprié.</returns>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<IEnumerable<TypeSalle>>> GetTypeSalles()
        {
            return await dataRepository.GetAllAsync();
        }


        // GET: api/Typesalles/5
        /// <summary>
        /// Récupère un type de salle spécifique en fonction de son identifiant.
        /// </summary>
        /// <param name="id">L'identifiant du type de salle à récupérer.</param>
        /// <returns>Le type de salle correspondant avec un statut HTTP approprié.</returns>
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


        /// <summary>
        /// Met à jour les informations d'un type de salle spécifique.
        /// </summary>
        /// <param name="id">L'identifiant du type de salle à mettre à jour.</param>
        /// <param name="typeSalle">Les nouvelles données du type de salle.</param>
        /// <returns>Un statut HTTP approprié après l'opération.</returns>
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
        /// <summary>
        /// Crée un nouveau type de salle dans la base de données.
        /// </summary>
        /// <param name="typeSalle">Les données du type de salle à ajouter.</param>
        /// <returns>Le type de salle créé avec un statut HTTP approprié.</returns>
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
        /// <summary>
        /// Supprime un type de salle spécifique de la base de données.
        /// </summary>
        /// <param name="id">L'identifiant du type de salle à supprimer.</param>
        /// <returns>Un statut HTTP approprié après l'opération.</returns>
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
        /// <summary>
        /// Récupère tous les types de salles sous forme de DTO (Data Transfer Object).
        /// </summary>
        /// <returns>Une liste des types de salles en format DTO avec un statut HTTP approprié.</returns>
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
        /// <summary>
        /// Récupère un type de salle spécifique en format DTO.
        /// </summary>
        /// <param name="id">L'identifiant du type de salle à récupérer en DTO.</param>
        /// <returns>Le type de salle en format DTO avec un statut HTTP approprié.</returns>
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


        /// <summary>
        /// Met à jour un type de salle spécifique en utilisant un DTO.
        /// </summary>
        /// <param name="id">L'identifiant du type de salle à mettre à jour.</param>
        /// <param name="typeSalleDto">Les nouvelles données sous forme de DTO du type de salle.</param>
        /// <returns>Un statut HTTP approprié après l'opération.</returns>
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

        /// <summary>
        /// Crée un nouveau type de salle en utilisant un DTO.
        /// </summary>
        /// <param name="typeSalleDto">Le DTO contenant les données du type de salle à créer.</param>
        /// <returns>Le type de salle créé sous forme de DTO avec un statut HTTP approprié.</returns>
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
        /// <summary>
        /// Supprime un type de salle spécifique en utilisant son DTO.
        /// </summary>
        /// <param name="id">L'identifiant du type de salle à supprimer.</param>
        /// <returns>Un statut HTTP approprié après l'opération.</returns>
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
