using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using SAE_APIGestion.Models.DTO;
using SAE_APIGestion.Models.EntityFramework;

namespace SAE_APIGestion.Controllers
{
    /// <summary>
    /// Contrôleur API pour gérer les entités <see cref="Salle"/>.
    /// Fournit des points de terminaison pour effectuer des opérations CRUD et manipuler les données via des DTO.
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class SalleController : ControllerBase
    {

        private readonly IDataRepository<Salle> dataRepository;
        private readonly IMapper _mapper;


        /// <summary>
        /// Constructeur permettant d'initialiser le contrôleur avec un repository et le service AutoMapper.
        /// </summary>
        /// <param name="dataRepo">Le repository permettant d'accéder aux données des <see cref="Salle"/>.</param>
        /// <param name="mapper">Le service AutoMapper pour la conversion entités/DTOs.</param>
        public SalleController(IDataRepository<Salle> dataRepo, IMapper mapper = null)
        {
            dataRepository = dataRepo;
            _mapper = mapper;
        }


        // GET: api/Salles
        /// <summary>
        /// Récupère toutes les salles dans la base de données.
        /// </summary>
        /// <returns>Une liste de salles avec un statut HTTP approprié.</returns>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<IEnumerable<Salle>>> GetSalles()
        {
            return await dataRepository.GetAllAsync();
        }

        /// <summary>
        /// Récupère une salle spécifique en fonction de son identifiant.
        /// </summary>
        /// <param name="id">L'identifiant de la salle à récupérer.</param>
        /// <returns>La salle correspondante avec un statut HTTP approprié.</returns>
        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<Salle>> GetSalle(int id)
        {
            var salle = await dataRepository.GetByIdAsync(id);

            if (salle == null)
            {
                return NotFound();
            }

            return salle;
        }

        /// <summary>
        /// Met à jour les informations d'une salle spécifique.
        /// </summary>
        /// <param name="id">L'identifiant de la salle à mettre à jour.</param>
        /// <param name="salle">Les nouvelles données de la salle.</param>
        /// <returns>Un statut HTTP approprié après l'opération.</returns>
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
        /// <summary>
        /// Crée une nouvelle salle dans la base de données.
        /// </summary>
        /// <param name="salle">Les données de la salle à ajouter.</param>
        /// <returns>La salle créée avec un statut HTTP approprié.</returns>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
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
        /// <summary>
        /// Supprime une salle spécifique de la base de données.
        /// </summary>
        /// <param name="id">L'identifiant de la salle à supprimer.</param>
        /// <returns>Un statut HTTP approprié après l'opération.</returns>
        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
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

        /// <summary>
        /// Récupère toutes les salles sous forme de DTOs (Data Transfer Objects).
        /// </summary>
        /// <returns>Une liste de salles au format DTO avec un statut HTTP approprié.</returns>
        [HttpGet("dto")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
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

        /// <summary>
        /// Récupère une salle spécifique sous forme de DTO en fonction de son identifiant.
        /// </summary>
        /// <param name="id">L'identifiant de la salle à récupérer sous forme de DTO.</param>
        /// <returns>La salle correspondante au format DTO avec un statut HTTP approprié.</returns>
        [HttpGet("dto/{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
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

        /// <summary>
        /// Met à jour une salle spécifique à partir de son DTO.
        /// </summary>
        /// <param name="id">L'identifiant de la salle à mettre à jour.</param>
        /// <param name="salleDto">Le DTO contenant les nouvelles données de la salle.</param>
        /// <returns>Un statut HTTP approprié après l'opération.</returns>
        [HttpPut("dto/{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
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

        /// <summary>
        /// Crée une nouvelle salle à partir de son DTO.
        /// </summary>
        /// <param name="salleDto">Le DTO contenant les données de la salle à ajouter.</param>
        /// <returns>La salle créée au format DTO avec un statut HTTP approprié.</returns>
        [HttpPost("dto")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
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
        /// <summary>
        /// Supprime une salle spécifique à partir de son DTO.
        /// </summary>
        /// <param name="id">L'identifiant de la salle à supprimer.</param>
        /// <returns>Un statut HTTP approprié après l'opération.</returns>
        [HttpDelete("dto/{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
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
