using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using SAE_APIGestion.Models.DTO;
using SAE_APIGestion.Models.EntityFramework;
using System.Text.Json;

namespace SAE_APIGestion.Controllers
{
    /// <summary>
    /// Contrôleur API pour gérer les entités <see cref="Capteur"/>.
    /// Expose des points de terminaison pour effectuer des opérations CRUD sur les capteurs.
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class CapteurController : ControllerBase
    {

        private readonly IDataRepository<Capteur> dataRepository;
        private readonly IMapper _mapper;

        /// <summary>
        /// Constructeur pour initialiser le contrôleur avec un repository et un mapper.
        /// </summary>
        /// <param name="dataRepo">Le repository des capteurs pour interagir avec la base de données.</param>
        /// <param name="mapper">Le mapper pour la conversion entre entités et DTOs.</param>
        public CapteurController(IDataRepository<Capteur> dataRepo, IMapper mapper = null)
        {
            dataRepository = dataRepo;
            _mapper = mapper;
        }


        /// <summary>
        /// Récupère tous les capteurs de la base de données.
        /// </summary>
        /// <returns>Une liste de capteurs avec un statut HTTP approprié.</returns>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<IEnumerable<Capteur>>> GetCapteurs()
        {
            return await dataRepository.GetAllAsync();
        }


        /// <summary>
        /// Récupère un capteur spécifique en fonction de son identifiant.
        /// </summary>
        /// <param name="id">L'identifiant du capteur à récupérer.</param>
        /// <returns>Un capteur avec un statut HTTP approprié.</returns>
        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<Capteur>> GetCapteur(int id)
        {
            var capteur = await dataRepository.GetByIdAsync(id);

            if (capteur == null)
            {
                return NotFound();
            }

            return capteur;
        }


        /// <summary>
        /// Met à jour un capteur existant avec de nouvelles informations.
        /// </summary>
        /// <param name="id">L'identifiant du capteur à mettre à jour.</param>
        /// <param name="capteur">Les nouvelles données du capteur.</param>
        /// <returns>Le statut HTTP approprié après l'opération.</returns>
        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> PutCapteur(int id, Capteur capteur)
        {
            if (id != capteur.CapteurId)
            {
                return BadRequest();
            }



            var capteurToUpdate = await dataRepository.GetByIdAsync(id);

            if (capteurToUpdate == null)
            {
                return NotFound();
            }
            else
            {
                await dataRepository.UpdateAsync(capteurToUpdate.Value, capteur);
                return NoContent();
            }
        }


        /// <summary>
        /// Crée un nouveau capteur dans la base de données.
        /// </summary>
        /// <param name="capteur">Le capteur à ajouter.</param>
        /// <returns>Le capteur créé avec un statut HTTP approprié.</returns>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<Capteur>> PostCapteur(Capteur capteur)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            await dataRepository.AddAsync(capteur);

            return CreatedAtAction("GetCapteur", new { id = capteur.CapteurId }, capteur);
        }

        /// <summary>
        /// Supprime un capteur de la base de données en fonction de son identifiant.
        /// </summary>
        /// <param name="id">L'identifiant du capteur à supprimer.</param>
        /// <returns>Le statut HTTP approprié après l'opération.</returns>
        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> DeleteCapteur(int id)
        {
            var capteur = await dataRepository.GetByIdAsync(id);
            if (capteur == null)
            {
                return NotFound();
            }

            await dataRepository.DeleteAsync(capteur.Value);


            return NoContent();
        }

        // =====================================================================================
        //DTO
        // =====================================================================================

        // GET: api/Capteur/dto
        /// <summary>
        /// Récupère tous les capteurs sous forme de DTO (Data Transfer Object).
        /// </summary>
        /// <returns>Une liste de capteurs sous forme de DTO avec un statut HTTP approprié.</returns>
        [HttpGet("dto")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<IEnumerable<CapteurDTO>>> GetAllCapteurDTO()
        {
            var result = await dataRepository.GetAllAsync();
            var capteurs = result.Value;
            if (capteurs == null || !capteurs.Any())
            {
                return NotFound();
            }
            var typesDto = _mapper.Map<IEnumerable<CapteurDTO>>(capteurs);
            return Ok(typesDto);
        }


        // GET: api/Capteur/dto/5
        /// <summary>
        /// Récupère un capteur spécifique sous forme de DTO en fonction de son identifiant.
        /// </summary>
        /// <param name="id">L'identifiant du capteur à récupérer.</param>
        /// <returns>Un capteur sous forme de DTO avec un statut HTTP approprié.</returns>
        [HttpGet("dto/{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<CapteurDTO>> GetCapteurDTO(int id)
        {
            var capteur = await dataRepository.GetByIdAsync(id);
            if (capteur == null)
            {
                return NotFound();
            }
            var capteurDto = _mapper.Map<CapteurDTO>(capteur.Value);
            return Ok(capteurDto);
        }


        /// <summary>
        /// Met à jour un capteur existant en utilisant un DTO.
        /// </summary>
        /// <param name="id">L'identifiant du capteur à mettre à jour.</param>
        /// <param name="capteurDto">Le DTO contenant les nouvelles données.</param>
        /// <returns>Le statut HTTP approprié après l'opération.</returns>
        [HttpPut("dto/{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> PutCapteurDTO(int id, CapteurDTO capteurDto)
        {

            if (id != capteurDto.CapteurId)
            {
                return BadRequest("ID mismatch");
            }

            // Vérification si le capteur existe
            var capteurToUpdate = await dataRepository.GetByIdAsync(id);
            if (capteurToUpdate == null)
            {
                return NotFound("Capteur not found");
            }

            // Mapping du DTO vers l'entité Capteur
            var capteur = _mapper.Map<Capteur>(capteurDto);

            // Vérification des valeurs après mapping

            // Mise à jour
            await dataRepository.UpdateAsync(capteurToUpdate.Value, capteur);

            return NoContent();
        }


        /// <summary>
        /// Crée un nouveau capteur à partir d'un DTO.
        /// </summary>
        /// <param name="capteurDto">Le DTO du capteur à ajouter.</param>
        /// <returns>Le capteur créé sous forme de DTO avec un statut HTTP approprié.</returns>
        [HttpPost("dto")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<CapteurDTO>> PostCapteurDTO(CapteurDTO capteurDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            // Mapper le DTO vers l'entité
            var capteur = _mapper.Map<Capteur>(capteurDto);

            // Ajouter l'entité
            await dataRepository.AddAsync(capteur);

            // Mapper l'entité retournée vers un DTO pour la réponse
            var resultDto = _mapper.Map<CapteurDTO>(capteur);

            return CreatedAtAction("GetCapteur", new { id = resultDto.CapteurId }, resultDto);
        }

        // DELETE: api/Capteur/dto/5
        /// <summary>
        /// Supprime un capteur en utilisant un DTO, basé sur son identifiant.
        /// </summary>
        /// <param name="id">L'identifiant du capteur à supprimer.</param>
        /// <returns>Le statut HTTP approprié après l'opération.</returns>
        [HttpDelete("dto/{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> DeleteCapteurDTO(int id)
        {
            var capteur = await dataRepository.GetByIdAsync(id);
            if (capteur == null)
            {
                return NotFound();
            }

            await dataRepository.DeleteAsync(capteur.Value);


            return NoContent();
        }

    }
}
