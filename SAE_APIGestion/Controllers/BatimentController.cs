using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using SAE_APIGestion.Models.DTO;
using SAE_APIGestion.Models.EntityFramework;

namespace SAE_APIGestion.Controllers
{

    /// <summary>
    /// Contrôleur API pour gérer les entités <see cref="Batiment"/>.
    /// Expose des points de terminaison pour effectuer des opérations CRUD sur les bâtiments.
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class BatimentController : ControllerBase
    {

        private readonly IDataRepository<Batiment> dataRepository;
        private readonly IMapper _mapper;

        /// <summary>
        /// Constructeur pour initialiser le contrôleur avec un repository et un mapper.
        /// </summary>
        /// <param name="dataRepo">Le repository des bâtiments pour interagir avec la base de données.</param>
        /// <param name="mapper">Le mapper pour la conversion entre entités et DTOs.</param>
        public BatimentController(IDataRepository<Batiment> dataRepo, IMapper mapper = null)
        {
            dataRepository = dataRepo;
            _mapper = mapper;
        }


        /// <summary>
        /// Récupère tous les bâtiments de la base de données.
        /// </summary>
        /// <returns>Une liste de bâtiments avec un statut HTTP approprié.</returns>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<IEnumerable<Batiment>>> GetBatiments()
        {
            return await dataRepository.GetAllAsync();
        }


        /// <summary>
        /// Récupère un bâtiment spécifique en fonction de son identifiant.
        /// </summary>
        /// <param name="id">L'identifiant du bâtiment à récupérer.</param>
        /// <returns>Un bâtiment avec un statut HTTP approprié.</returns>
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


        /// <summary>
        /// Met à jour un bâtiment existant avec de nouvelles informations.
        /// </summary>
        /// <param name="id">L'identifiant du bâtiment à mettre à jour.</param>
        /// <param name="batiment">Les nouvelles données du bâtiment.</param>
        /// <returns>Le statut HTTP approprié après l'opération.</returns>
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


        /// <summary>
        /// Crée un nouveau bâtiment dans la base de données.
        /// </summary>
        /// <param name="batiment">Le bâtiment à ajouter.</param>
        /// <returns>Le bâtiment créé avec un statut HTTP approprié.</returns>
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

        /// <summary>
        /// Supprime un bâtiment de la base de données en fonction de son identifiant.
        /// </summary>
        /// <param name="id">L'identifiant du bâtiment à supprimer.</param>
        /// <returns>Le statut HTTP approprié après l'opération.</returns>
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


        /// <summary>
        /// Récupère tous les bâtiments en utilisant un DTO (Data Transfer Object).
        /// </summary>
        /// <returns>Une liste de bâtiments sous forme de DTO avec un statut HTTP approprié.</returns>
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

        /// <summary>
        /// Récupère un bâtiment spécifique sous forme de DTO en fonction de son identifiant.
        /// </summary>
        /// <param name="id">L'identifiant du bâtiment à récupérer.</param>
        /// <returns>Un bâtiment sous forme de DTO avec un statut HTTP approprié.</returns>
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


        /// <summary>
        /// Met à jour un bâtiment existant en utilisant un DTO.
        /// </summary>
        /// <param name="id">L'identifiant du bâtiment à mettre à jour.</param>
        /// <param name="batimentDto">Le DTO contenant les nouvelles données.</param>
        /// <returns>Le statut HTTP approprié après l'opération.</returns>
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


        /// <summary>
        /// Crée un nouveau bâtiment à partir d'un DTO.
        /// </summary>
        /// <param name="batimentDto">Le DTO du bâtiment à ajouter.</param>
        /// <returns>Le bâtiment créé sous forme de DTO avec un statut HTTP approprié.</returns>
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

        /// <summary>
        /// Supprime un bâtiment en utilisant un DTO, basé sur son identifiant.
        /// </summary>
        /// <param name="id">L'identifiant du bâtiment à supprimer.</param>
        /// <returns>Le statut HTTP approprié après l'opération.</returns>
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
