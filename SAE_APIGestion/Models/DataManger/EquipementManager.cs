using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SAE_APIGestion.Models.EntityFramework;

namespace SAE_APIGestion.Models.DataManger
{
    /// <summary>
    /// Cette classe est responsable de la gestion des entités <see cref="Equipement"/> dans la base de données.
    /// Elle implémente l'interface <see cref="IDataRepository{Equipement}"/> pour fournir des opérations CRUD.
    /// </summary>
    public class EquipementManager : IDataRepository<Equipement>
    {
        /// <summary>
        /// Contexte de la base de données pour interagir avec la table <see cref="Equipement"/>.
        /// </summary>
        readonly GlobalDBContext? globalDbContext;

        /// <summary>
        /// Constructeur par défaut pour <see cref="EquipementManager"/>.
        /// </summary>
        public EquipementManager() { }

        /// <summary>
        /// Constructeur de <see cref="EquipementManager"/> avec un contexte de base de données fourni.
        /// </summary>
        /// <param name="context">Le contexte de la base de données à utiliser.</param>
        public EquipementManager(GlobalDBContext context)
        {
            globalDbContext = context;
        }

        /// <summary>
        /// Récupère tous les équipements, y compris leurs murs, salles et types d'équipements associés.
        /// </summary>
        /// <returns>Une liste d'équipements avec toutes leurs entités associées.</returns>
        public async Task<ActionResult<IEnumerable<Equipement>>> GetAllAsync()
        {
            return await globalDbContext.Equipements
                .AsNoTracking()
                .Include(b => b.Mur)
                .Include(b => b.Salle)
                .Include(b => b.TypeEquipement)
                
                .ToListAsync();
        }

        /// <summary>
        /// Récupère un équipement spécifique par son identifiant, avec toutes ses entités associées.
        /// </summary>
        /// <param name="id">L'identifiant de l'équipement à récupérer.</param>
        /// <returns>L'équipement correspondant à l'identifiant, ou null si non trouvé.</returns>
        public async Task<ActionResult<Equipement>> GetByIdAsync(int id)
        {
            return await globalDbContext.Equipements
                .AsNoTracking()
                 .Include(b => b.Mur)
                .Include(b => b.Salle)
                .Include(b => b.TypeEquipement)
                .FirstOrDefaultAsync(u => u.EquipementId == id);
        }


        /// <summary>
        /// Récupère un équipement spécifique par son nom.
        /// </summary>
        /// <param name="nom">Le nom de l'équipement à récupérer.</param>
        /// <returns>L'équipement correspondant au nom donné, ou null si non trouvé.</returns>
        public async Task<ActionResult<Equipement>> GetByStringAsync(string nom)
        {
            return await globalDbContext.Equipements.FirstOrDefaultAsync(u => u.Nom.ToUpper() == nom.ToUpper());
        }

        /// <summary>
        /// Ajoute un nouvel équipement à la base de données.
        /// </summary>
        /// <param name="entity">L'équipement à ajouter.</param>
        /// <returns>Une tâche représentant l'opération asynchrone.</returns>
        public async Task AddAsync(Equipement entity)
        {
            await globalDbContext.Equipements.AddAsync(entity);
            await globalDbContext.SaveChangesAsync();
        }

        /// <summary>
        /// Met à jour un équipement existant dans la base de données avec de nouvelles valeurs.
        /// </summary>
        /// <param name="equipement">L'équipement à mettre à jour (l'équipement existant).</param>
        /// <param name="entity">Les nouvelles valeurs à appliquer à l'équipement.</param>
        /// <returns>Une tâche représentant l'opération asynchrone.</returns>
        public async Task UpdateAsync(Equipement equipement, Equipement entity)
        {
            globalDbContext.Entry(equipement).State = EntityState.Modified;
            equipement.Nom = entity.Nom;
            equipement.TypeEquipementId = entity.TypeEquipementId;
            equipement.Hauteur = entity.Hauteur;
            equipement.Longueur = entity.Longueur;
            equipement.PositionX = entity.PositionX;
            equipement.PositionY = entity.PositionY;
            equipement.MurId = entity.MurId;
            equipement.SalleId = entity.SalleId;
            await globalDbContext.SaveChangesAsync();
        }

        /// <summary>
        /// Supprime un équipement de la base de données.
        /// </summary>
        /// <param name="equipement">L'équipement à supprimer.</param>
        /// <returns>Une tâche représentant l'opération asynchrone.</returns>
        public async Task DeleteAsync(Equipement equipement)
        {
            globalDbContext.Equipements.Remove(equipement);
            await globalDbContext.SaveChangesAsync();
        }
    }
}
