using Microsoft.AspNetCore.Mvc;
using SAE_APIGestion.Models.EntityFramework;

namespace SAE_APIGestion.Controllers
{
    /// <summary>
    /// Contrôleur API pour gérer les entités <see cref="TypeDonneesCapteur"/>.
    /// Fournit des points de terminaison pour effectuer des opérations CRUD sur les types de données des capteurs.
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class TypeDonneesCapteurController : ControllerBase
    {

        private readonly IDataRepository<TypeDonneesCapteur> dataRepository;

        /// <summary>
        /// Constructeur permettant d'initialiser le contrôleur avec un repository.
        /// </summary>
        /// <param name="dataRepo">Le repository permettant d'accéder aux données des <see cref="TypeDonneesCapteur"/>.</param>
        public TypeDonneesCapteurController(IDataRepository<TypeDonneesCapteur> dataRepo)
        {
            dataRepository = dataRepo;
        }


        /// <summary>
        /// Récupère tous les types de données des capteurs dans la base de données.
        /// </summary>
        /// <returns>Une liste des types de données des capteurs avec un statut HTTP approprié.</returns>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<IEnumerable<TypeDonneesCapteur>>> GetTypeDonneesCapteurs()
        {
            return await dataRepository.GetAllAsync();
        }


        /// <summary>
        /// Récupère un type de données capteur spécifique en fonction de son identifiant.
        /// </summary>
        /// <param name="id">L'identifiant du type de données capteur à récupérer.</param>
        /// <returns>Le type de données capteur correspondant avec un statut HTTP approprié.</returns>
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


        /// <summary>
        /// Met à jour les informations d'un type de données capteur spécifique.
        /// </summary>
        /// <param name="id">L'identifiant du type de données capteur à mettre à jour.</param>
        /// <param name="typeDonneesCapteur">Les nouvelles données du type de données capteur.</param>
        /// <returns>Un statut HTTP approprié après l'opération.</returns>
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


        /// <summary>
        /// Crée un nouveau type de données capteur dans la base de données.
        /// </summary>
        /// <param name="typeDonneesCapteur">Les données du type de données capteur à ajouter.</param>
        /// <returns>Le type de données capteur créé avec un statut HTTP approprié.</returns>
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

        /// <summary>
        /// Supprime un type de données capteur spécifique de la base de données.
        /// </summary>
        /// <param name="id">L'identifiant du type de données capteur à supprimer.</param>
        /// <returns>Un statut HTTP approprié après l'opération.</returns>
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
