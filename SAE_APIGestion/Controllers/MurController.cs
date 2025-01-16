using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using SAE_APIGestion.Models.DTO;
using SAE_APIGestion.Models.EntityFramework;

namespace SAE_APIGestion.Controllers
{

    /// <summary>
    /// Contrôleur API pour gérer les entités <see cref="Mur"/>.
    /// Fournit des points de terminaison pour effectuer des opérations CRUD et manipuler les données via des DTO.
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class MurController : ControllerBase
    {
        private readonly IDataRepository<Mur> dataRepository;
        private readonly IMapper _mapper;

        /// <summary>
        /// Constructeur permettant d'initialiser le contrôleur avec un repository et le service AutoMapper.
        /// </summary>
        /// <param name="dataRepo">Le repository permettant d'accéder aux données des <see cref="Mur"/>.</param>
        /// <param name="mapper">Le service AutoMapper pour la conversion entités/DTOs.</param>
        public MurController(IDataRepository<Mur> dataRepo, IMapper mapper = null)
        {
            dataRepository = dataRepo;
            _mapper = mapper;
        }


        // GET: api/Murs
        /// <summary>
        /// Récupère tous les murs dans la base de données.
        /// </summary>
        /// <returns>Une liste de murs avec un statut HTTP approprié.</returns>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<IEnumerable<Mur>>> GetMurs()
        {
            return await dataRepository.GetAllAsync();
        }

        /// <summary>
        /// Récupère un mur spécifique en fonction de son identifiant.
        /// </summary>
        /// <param name="id">L'identifiant du mur à récupérer.</param>
        /// <returns>Le mur correspondant à l'identifiant avec un statut HTTP approprié.</returns>
        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<Mur>> GetMur(int id)
        {
            var mur = await dataRepository.GetByIdAsync(id);

            if (mur == null)
            {
                return NotFound();
            }

            return mur;
        }

        /// <summary>
        /// Met à jour les informations d'un mur spécifique.
        /// </summary>
        /// <param name="id">L'identifiant du mur à mettre à jour.</param>
        /// <param name="mur">Les nouvelles données du mur.</param>
        /// <returns>Un statut HTTP approprié après l'opération.</returns>
        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> PutMur(int id, Mur mur)
        {
            if (id != mur.MurId)
            {
                return BadRequest();
            }


            var murToUpdate = await dataRepository.GetByIdAsync(id);

            if (murToUpdate == null)
            {
                return NotFound();
            }
            else
            {
                await dataRepository.UpdateAsync(murToUpdate.Value, mur);
                return NoContent();
            }
        }


        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        /// <summary>
        /// Crée un nouveau mur dans la base de données.
        /// </summary>
        /// <param name="mur">Les données du mur à ajouter.</param>
        /// <returns>Le mur ajouté avec un statut HTTP approprié.</returns>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<Mur>> PostMur(Mur mur)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            await dataRepository.AddAsync(mur);

            return CreatedAtAction("GetMur", new { id = mur.MurId }, mur);
        }

        // DELETE: api/typeMurs/5
        /// <summary>
        /// Supprime un mur spécifique de la base de données.
        /// </summary>
        /// <param name="id">L'identifiant du mur à supprimer.</param>
        /// <returns>Un statut HTTP approprié après l'opération.</returns>
        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> DeleteMur(int id)
        {
            var mur = await dataRepository.GetByIdAsync(id);
            if (mur == null)
            {
                return NotFound();
            }

            await dataRepository.DeleteAsync(mur.Value);

            return NoContent();
        }

        // =====================================================================================
        //DTO
        // =====================================================================================

        // GET: api/Mur/dto
        /// <summary>
        /// Récupère tous les murs sous forme de DTOs (Data Transfer Objects).
        /// </summary>
        /// <returns>Une liste de murs au format DTO avec un statut HTTP approprié.</returns>
        [HttpGet("dto")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<IEnumerable<MurDTO>>> GetAllMurDTO()
        {
            var result = await dataRepository.GetAllAsync();
            var murs = result.Value;
            if (murs == null || !murs.Any())
            {
                return NotFound();
            }
            var murDto = _mapper.Map<IEnumerable<MurDTO>>(murs);
            return Ok(murDto);
        }


        // GET: api/Mur/dto/5
        /// <summary>
        /// Récupère un mur spécifique sous forme de DTO en fonction de son identifiant.
        /// </summary>
        /// <param name="id">L'identifiant du mur à récupérer sous forme de DTO.</param>
        /// <returns>Le mur correspondant à l'identifiant au format DTO avec un statut HTTP approprié.</returns>
        [HttpGet("dto/{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<MurDTO>> GetMurDTO(int id)
        {
            var mur = await dataRepository.GetByIdAsync(id);
            if (mur == null)
            {
                return NotFound();
            }
            var murDto = _mapper.Map<MurDTO>(mur.Value);
            return Ok(murDto);
        }


        /// <summary>
        /// Met à jour un mur spécifique à partir de son DTO.
        /// </summary>
        /// <param name="id">L'identifiant du mur à mettre à jour.</param>
        /// <param name="murDto">Le DTO contenant les nouvelles données du mur.</param>
        /// <returns>Un statut HTTP approprié après l'opération.</returns>
        [HttpPut("dto/{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> PutMurDTO(int id, MurDTO murDto)
        {

            if (id != murDto.MurId)
            {
                return BadRequest("ID mismatch");
            }

            // Vérification si le capteur existe
            var murToUpdate = await dataRepository.GetByIdAsync(id);
            if (murToUpdate == null)
            {
                return NotFound("Mur not found");
            }

            // Mapping du DTO vers l'entité Capteur
            var mur = _mapper.Map<Mur>(murDto);


            // Mise à jour
            await dataRepository.UpdateAsync(murToUpdate.Value, mur);

            return NoContent();
        }


        /// <summary>
        /// Crée un nouveau mur à partir de son DTO.
        /// </summary>
        /// <param name="murDto">Le DTO contenant les données du mur à ajouter.</param>
        /// <returns>Le mur créé sous forme de DTO avec un statut HTTP approprié.</returns>
        [HttpPost("dto")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<CapteurDTO>> PostMurDTO(MurDTO murDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            // Mapper le DTO vers l'entité
            var mur = _mapper.Map<Mur>(murDto);

            // Ajouter l'entité
            await dataRepository.AddAsync(mur);

            // Mapper l'entité retournée vers un DTO pour la réponse
            var resultDto = _mapper.Map<MurDTO>(mur);

            return CreatedAtAction("GetMur", new { id = resultDto.MurId }, resultDto);
        }

        // DELETE: api/Mur/dto/5
        /// <summary>
        /// Supprime un mur spécifique à partir de son DTO.
        /// </summary>
        /// <param name="id">L'identifiant du mur à supprimer.</param>
        /// <returns>Un statut HTTP approprié après l'opération.</returns>
        [HttpDelete("dto/{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> DeleteMurDTO(int id)
        {
            var mur = await dataRepository.GetByIdAsync(id);
            if (mur == null)
            {
                return NotFound();
            }

            await dataRepository.DeleteAsync(mur.Value);


            return NoContent();
        }

    }
}
