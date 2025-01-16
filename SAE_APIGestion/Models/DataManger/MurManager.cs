using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SAE_APIGestion.Models.EntityFramework;

namespace SAE_APIGestion.Models.DataManger
{
    /// <summary>
    /// Cette classe est responsable de la gestion des entités <see cref="Mur"/> dans la base de données.
    /// Elle implémente l'interface <see cref="IDataRepository{Mur}"/> pour fournir des opérations CRUD.
    /// </summary>
    public class MurManager : IDataRepository<Mur>
    {
        /// <summary>
        /// Contexte de la base de données pour interagir avec la table <see cref="Mur"/>.
        /// </summary>
        readonly GlobalDBContext? dbContext;

        /// <summary>
        /// Constructeur par défaut pour <see cref="MurManager"/>.
        /// </summary>
        public MurManager() { }

        /// <summary>
        /// Constructeur de <see cref="MurManager"/> avec un contexte de base de données fourni.
        /// </summary>
        /// <param name="context">Le contexte de la base de données à utiliser.</param>
        public MurManager(GlobalDBContext context)
        {
            dbContext = context;
        }

        /// <summary>
        /// Récupère tous les murs, y compris leurs équipements, capteurs, et salles associés.
        /// </summary>
        /// <returns>Une liste de murs avec toutes leurs entités associées.</returns>
        public async Task<ActionResult<IEnumerable<Mur>>> GetAllAsync()
        {
            return await dbContext.Murs
                .AsNoTracking()
                .Include(b => b.Equipements)
                    .ThenInclude(b => b.TypeEquipement)
                .Include(b => b.Capteurs)
                .Include(b => b.Salle)
                .ToListAsync();
        }

        /// <summary>
        /// Récupère un mur spécifique par son identifiant, avec tous ses équipements, capteurs, et salle associés.
        /// </summary>
        /// <param name="id">L'identifiant du mur à récupérer.</param>
        /// <returns>Le mur correspondant à l'identifiant, ou null si non trouvé.</returns>
        public async Task<ActionResult<Mur>> GetByIdAsync(int id)
        {
            return await dbContext.Murs
                .AsNoTracking()
                .Include(b => b.Equipements)
                    .ThenInclude(b => b.TypeEquipement)
                .Include(b => b.Capteurs)
                .Include(b => b.Salle)
                .FirstOrDefaultAsync(u => u.MurId == id);
        }

        /// <summary>
        /// Ajoute un nouveau mur à la base de données.
        /// </summary>
        /// <param name="entity">Le mur à ajouter.</param>
        /// <returns>Une tâche représentant l'opération asynchrone.</returns>
        public async Task AddAsync(Mur entity)
        {
            await dbContext.Murs.AddAsync(entity);
            await dbContext.SaveChangesAsync();
        }

        /// <summary>
        /// Supprime un mur de la base de données.
        /// </summary>
        /// <param name="entity">Le mur à supprimer.</param>
        /// <returns>Une tâche représentant l'opération asynchrone.</returns>
        public async Task DeleteAsync(Mur entity)
        {
            dbContext.Murs.Remove(entity);
            await dbContext.SaveChangesAsync();
        }

        /// <summary>
        /// Met à jour un mur existant dans la base de données avec de nouvelles valeurs.
        /// </summary>
        /// <param name="enseignant">Le mur à mettre à jour (ancien mur).</param>
        /// <param name="entity">Les nouvelles valeurs à appliquer au mur.</param>
        /// <returns>Une tâche représentant l'opération asynchrone.</returns>
        public async Task UpdateAsync(Mur enseignant, Mur entity)
        {
            dbContext.Entry(enseignant).State = EntityState.Modified;
            enseignant.MurId = entity.MurId;
            enseignant.Nom = entity.Nom;
            enseignant.Longueur = entity.Longueur;
            enseignant.Hauteur = entity.Hauteur;
            enseignant.Orientation = entity.Orientation;
            enseignant.Equipements = entity.Equipements;
            enseignant.Capteurs = entity.Capteurs;
            await dbContext.SaveChangesAsync();
        }
    }
}