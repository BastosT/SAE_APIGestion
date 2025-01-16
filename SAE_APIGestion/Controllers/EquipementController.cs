using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using SAE_APIGestion.Models.DTO;
using SAE_APIGestion.Models.EntityFramework;

namespace SAE_APIGestion.Controllers
{

    /// <summary>
    /// Contrôleur API pour gérer les entités <see cref="Equipement"/>.
    /// Expose des points de terminaison pour effectuer des opérations CRUD sur les équipements.
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class EquipementController : ControllerBase
    {

        private readonly IDataRepository<Equipement> dataRepository;
        private readonly IMapper _mapper;

        /// <summary>
        /// Constructeur pour initialiser le contrôleur avec un repository et un service de mappage AutoMapper.
        /// </summary>
        /// <param name="dataRepo">Le repository des équipements pour interagir avec la base de données.</param>
        /// <param name="mapper">Le service AutoMapper pour la conversion des entités en DTOs.</param>
        public EquipementController(IDataRepository<Equipement> dataRepo, IMapper mapper = null)
        {
            dataRepository = dataRepo;
            _mapper = mapper;
        }


        // GET: api/Equipement
        /// <summary>
        /// Récupère tous les équipements de la base de données.
        /// </summary>
        /// <returns>Une liste d'équipements avec un statut HTTP approprié.</returns>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<IEnumerable<Equipement>>> GetEquipements()
        {
            return await dataRepository.GetAllAsync();
        }

        /// <summary>
        /// Récupère un équipement spécifique en fonction de son identifiant.
        /// </summary>
        /// <param name="id">L'identifiant de l'équipement à récupérer.</param>
        /// <returns>L'équipement correspondant à l'identifiant avec un statut HTTP approprié.</returns>
        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<Equipement>> GetEquipement(int id)
        {
            var equipement = await dataRepository.GetByIdAsync(id);

            if (equipement == null)
            {
                return NotFound();
            }

            return equipement;
        }

        /// <summary>
        /// Met à jour les informations d'un équipement spécifique.
        /// </summary>
        /// <param name="id">L'identifiant de l'équipement à mettre à jour.</param>
        /// <param name="equipement">Les nouvelles données de l'équipement à mettre à jour.</param>
        /// <returns>Le statut HTTP approprié après l'opération.</returns>
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
        /// <summary>
        /// Crée un nouvel équipement dans la base de données.
        /// </summary>
        /// <param name="equipement">Les données de l'équipement à ajouter.</param>
        /// <returns>L'équipement ajouté avec un statut HTTP approprié.</returns>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
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
        /// <summary>
        /// Supprime un équipement de la base de données en fonction de son identifiant.
        /// </summary>
        /// <param name="id">L'identifiant de l'équipement à supprimer.</param>
        /// <returns>Le statut HTTP approprié après l'opération.</returns>
        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
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
        /// <summary>
        /// Récupère tous les équipements sous forme de DTOs (Data Transfer Objects).
        /// </summary>
        /// <returns>Une liste d'équipements au format DTO avec un statut HTTP approprié.</returns>
        [HttpGet("dto")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
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
        /// <summary>
        /// Récupère un équipement spécifique sous forme de DTO en fonction de son identifiant.
        /// </summary>
        /// <param name="id">L'identifiant de l'équipement à récupérer sous forme de DTO.</param>
        /// <returns>L'équipement correspondant à l'identifiant au format DTO avec un statut HTTP approprié.</returns>
        [HttpGet("dto/{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
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


        /// <summary>
        /// Met à jour un équipement spécifique à partir de son DTO.
        /// </summary>
        /// <param name="id">L'identifiant de l'équipement à mettre à jour.</param>
        /// <param name="equipementDto">Le DTO contenant les nouvelles données de l'équipement.</param>
        /// <returns>Le statut HTTP approprié après l'opération.</returns>
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

        /// <summary>
        /// Crée un nouvel équipement à partir de son DTO.
        /// </summary>
        /// <param name="equipementDto">Le DTO contenant les données de l'équipement à ajouter.</param>
        /// <returns>L'équipement créé sous forme de DTO avec un statut HTTP approprié.</returns>
        [HttpPost("dto")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
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
        /// <summary>
        /// Supprime un équipement à partir de son DTO.
        /// </summary>
        /// <param name="id">L'identifiant de l'équipement à supprimer.</param>
        /// <returns>Le statut HTTP approprié après l'opération.</returns>
        [HttpDelete("dto/{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
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
