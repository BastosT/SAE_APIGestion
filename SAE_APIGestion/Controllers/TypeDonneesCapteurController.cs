using Microsoft.AspNetCore.Mvc;
using SAE_APIGestion.Models.EntityFramework;

namespace SAE_APIGestion.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TypeDonneesCapteurController : ControllerBase
    {

        private readonly IDataRepository<TypeDonneesCapteur> dataRepository;

        public TypeDonneesCapteurController(IDataRepository<TypeDonneesCapteur> dataRepo)
        {
            dataRepository = dataRepo;
        }


        [HttpGet]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<IEnumerable<TypeDonneesCapteur>>> GetTypeDonneesCapteurs()
        {
            return await dataRepository.GetAllAsync();
        }


        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<TypeDonneesCapteur>> GetTypeDonneesCapteur(int id)
        {
            var typeDonneesCapteur = await dataRepository.GetByIdAsync(id);

            if (typeDonneesCapteur == null)
            {
                return NotFound();
            }

            return typeDonneesCapteur;
        }


        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> PutTypeDonneesCapteur(int id, TypeDonneesCapteur typeDonneesCapteur)
        {
            if (id != typeDonneesCapteur.TypeDonneesCapteurId)
            {
                return BadRequest();
            }



            var typeDonneesCapteurToUpdate = await dataRepository.GetByIdAsync(id);

            if (typeDonneesCapteurToUpdate == null)
            {
                return NotFound();
            }
            else
            {
                await dataRepository.UpdateAsync(typeDonneesCapteurToUpdate.Value, typeDonneesCapteur);
                return NoContent();
            }
        }


        [HttpPost]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<TypeDonneesCapteur>> PostTypeDonneesCapteur(TypeDonneesCapteur typeDonneesCapteur)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            await dataRepository.AddAsync(typeDonneesCapteur);

            return CreatedAtAction("GetTypeDonneesCapteur", new { id = typeDonneesCapteur.TypeDonneesCapteurId }, typeDonneesCapteur);
        }

        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> DeleteTypeDonneesCapteur(int id)
        {
            var typeDonneesCapteur = await dataRepository.GetByIdAsync(id);
            if (typeDonneesCapteur == null)
            {
                return NotFound();
            }

            await dataRepository.DeleteAsync(typeDonneesCapteur.Value);


            return NoContent();
        }


    }
}
