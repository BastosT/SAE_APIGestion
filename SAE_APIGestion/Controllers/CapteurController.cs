using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using SAE_APIGestion.Models.DTO;
using SAE_APIGestion.Models.EntityFramework;
using System.Text.Json;

namespace SAE_APIGestion.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CapteurController : ControllerBase
    {

        private readonly IDataRepository<Capteur> dataRepository;
        private readonly IMapper _mapper;

        public CapteurController(IDataRepository<Capteur> dataRepo, IMapper mapper = null)
        {
            dataRepository = dataRepo;
            _mapper = mapper;
        }


        [HttpGet]
        public async Task<ActionResult<IEnumerable<Capteur>>> GetCapteurs()
        {
            return await dataRepository.GetAllAsync();
        }


        [HttpGet("{id}")]
        public async Task<ActionResult<Capteur>> GetCapteur(int id)
        {
            var capteur = await dataRepository.GetByIdAsync(id);

            if (capteur == null)
            {
                return NotFound();
            }

            return capteur;
        }


        [HttpPut("{id}")]
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


        [HttpPost]
        public async Task<ActionResult<Capteur>> PostCapteur(Capteur capteur)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            await dataRepository.AddAsync(capteur);

            return CreatedAtAction("GetCapteur", new { id = capteur.CapteurId }, capteur);
        }

        [HttpDelete("{id}")]
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
        [HttpGet("dto")]
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
        [HttpGet("dto/{id}")]
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


        [HttpPut("dto/{id}")]
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


        [HttpPost("dto")]
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
        [HttpDelete("dto/{id}")]
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
