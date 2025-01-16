using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using SAE_APIGestion.Models.DTO;
using SAE_APIGestion.Models.EntityFramework;

namespace SAE_APIGestion.Controllers
{

    /// <summary>
    /// Contrôleur API pour gérer les entités <see cref="TypeEquipement"/>.
    /// Fournit des points de terminaison pour effectuer des opérations CRUD sur les types d'équipements.
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class TypeEquipementController : ControllerBase
    {
        private readonly IDataRepository<TypeEquipement> dataRepository;
        private readonly IMapper _mapper;

        /// <summary>
        /// Constructeur permettant d'initialiser le contrôleur avec un repository et un mappage AutoMapper.
        /// </summary>
        /// <param name="dataRepo">Le repository permettant d'accéder aux données des <see cref="TypeEquipement"/>.</param>
        /// <param name="mapper">Le service AutoMapper utilisé pour la conversion entre entités et DTO.</param>
        public TypeEquipementController(IDataRepository<TypeEquipement> dataRepo, IMapper mapper = null)
        {
            dataRepository = dataRepo;
            _mapper = mapper;
        }


        // GET: api/TypeEquipements
        /// <summary>
        /// Récupère tous les types d'équipements dans la base de données.
        /// </summary>
        /// <returns>Une liste des types d'équipements avec un statut HTTP approprié.</returns>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<IEnumerable<TypeEquipement>>> GetTypeEquipements()
        {
            return await dataRepository.GetAllAsync();
        }

        /// <summary>
        /// Récupère un type d'équipement spécifique en fonction de son identifiant.
        /// </summary>
        /// <param name="id">L'identifiant du type d'équipement à récupérer.</param>
        /// <returns>Le type d'équipement correspondant avec un statut HTTP approprié.</returns>
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

        /// <summary>
        /// Met à jour les informations d'un type d'équipement spécifique.
        /// </summary>
        /// <param name="id">L'identifiant du type d'équipement à mettre à jour.</param>
        /// <param name="typeEquipement">Les nouvelles données du type d'équipement.</param>
        /// <returns>Un statut HTTP approprié après l'opération.</returns>
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
        /// <summary>
        /// Crée un nouveau type d'équipement dans la base de données.
        /// </summary>
        /// <param name="typeEquipement">Les données du type d'équipement à ajouter.</param>
        /// <returns>Le type d'équipement créé avec un statut HTTP approprié.</returns>
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
        /// <summary>
        /// Supprime un type d'équipement spécifique de la base de données.
        /// </summary>
        /// <param name="id">L'identifiant du type d'équipement à supprimer.</param>
        /// <returns>Un statut HTTP approprié après l'opération.</returns>
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
        /// <summary>
        /// Récupère tous les types d'équipements sous forme de DTO (Data Transfer Object).
        /// </summary>
        /// <returns>Une liste des types d'équipements en format DTO avec un statut HTTP approprié.</returns>
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
        /// <summary>
        /// Récupère un type d'équipement spécifique en format DTO.
        /// </summary>
        /// <param name="id">L'identifiant du type d'équipement à récupérer en DTO.</param>
        /// <returns>Le type d'équipement en format DTO avec un statut HTTP approprié.</returns>
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


        /// <summary>
        /// Met à jour un type d'équipement spécifique en utilisant un DTO.
        /// </summary>
        /// <param name="id">L'identifiant du type d'équipement à mettre à jour.</param>
        /// <param name="typeEquipementDto">Les nouvelles données sous forme de DTO du type d'équipement.</param>
        /// <returns>Un statut HTTP approprié après l'opération.</returns>
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

        /// <summary>
        /// Crée un nouveau type d'équipement en utilisant un DTO.
        /// </summary>
        /// <param name="typeEquipementDto">Le DTO contenant les données du type d'équipement à créer.</param>
        /// <returns>Le type d'équipement créé sous forme de DTO avec un statut HTTP approprié.</returns>
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
        /// <summary>
        /// Supprime un type d'équipement spécifique en utilisant son DTO.
        /// </summary>
        /// <param name="id">L'identifiant du type d'équipement à supprimer.</param>
        /// <returns>Un statut HTTP approprié après l'opération.</returns>
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
