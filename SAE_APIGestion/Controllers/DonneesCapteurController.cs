using Microsoft.AspNetCore.Mvc;
using SAE_APIGestion.Models.EntityFramework;

namespace SAE_APIGestion.Controllers
{
    /// <summary>
    /// Contrôleur API pour gérer les entités <see cref="DonneesCapteur"/>.
    /// Expose des points de terminaison pour effectuer des opérations CRUD sur les données des capteurs.
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class DonneesCapteurController : ControllerBase
    {

        private readonly IDataRepository<DonneesCapteur> dataRepository;

        /// <summary>
        /// Constructeur pour initialiser le contrôleur avec un repository.
        /// </summary>
        /// <param name="dataRepo">Le repository des données des capteurs pour interagir avec la base de données.</param>
        public DonneesCapteurController(IDataRepository<DonneesCapteur> dataRepo)
        {
            dataRepository = dataRepo;
        }


        /// <summary>
        /// Récupère toutes les données des capteurs de la base de données.
        /// </summary>
        /// <returns>Une liste de données des capteurs avec un statut HTTP approprié.</returns>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<DonneesCapteur>>> GetDonneesCapteurs()
        {
            return await dataRepository.GetAllAsync();
        }


        /// <summary>
        /// Récupère les données d'un capteur spécifique en fonction de son identifiant.
        /// </summary>
        /// <param name="id">L'identifiant des données du capteur à récupérer.</param>
        /// <returns>Les données du capteur avec un statut HTTP approprié.</returns>
        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<DonneesCapteur>> GetDonneesCapteur(int id)
        {
            var donneesCapteur = await dataRepository.GetByIdAsync(id);

            if (donneesCapteur == null)
            {
                return NotFound();
            }

            return donneesCapteur;
        }


        /// <summary>
        /// Met à jour les données d'un capteur existant avec de nouvelles informations.
        /// </summary>
        /// <param name="id">L'identifiant des données du capteur à mettre à jour.</param>
        /// <param name="donneesCapteur">Les nouvelles données du capteur.</param>
        /// <returns>Le statut HTTP approprié après l'opération.</returns>
        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> PutDonneesCapteur(int id, DonneesCapteur donneesCapteur)
        {
            if (id != donneesCapteur.DonneesCapteurId)
            {
                return BadRequest();
            }



            var donneesCapteurToUpdate = await dataRepository.GetByIdAsync(id);

            if (donneesCapteurToUpdate == null)
            {
                return NotFound();
            }
            else
            {
                await dataRepository.UpdateAsync(donneesCapteurToUpdate.Value, donneesCapteur);
                return NoContent();
            }
        }


        /// <summary>
        /// Crée de nouvelles données pour un capteur dans la base de données.
        /// </summary>
        /// <param name="donneesCapteur">Les données du capteur à ajouter.</param>
        /// <returns>Les données du capteur créées avec un statut HTTP approprié.</returns>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<DonneesCapteur>> PostDonneesCapteur(DonneesCapteur donneesCapteur)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            await dataRepository.AddAsync(donneesCapteur);

            return CreatedAtAction("GetDonneesCapteur", new { id = donneesCapteur.DonneesCapteurId }, donneesCapteur);
        }

        /// <summary>
        /// Supprime les données d'un capteur de la base de données en fonction de son identifiant.
        /// </summary>
        /// <param name="id">L'identifiant des données du capteur à supprimer.</param>
        /// <returns>Le statut HTTP approprié après l'opération.</returns>
        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> DeleteDonneesCapteur(int id)
        {
            var donneesCapteur = await dataRepository.GetByIdAsync(id);
            if (donneesCapteur == null)
            {
                return NotFound();
            }

            await dataRepository.DeleteAsync(donneesCapteur.Value);


            return NoContent();
        }


    }
}
