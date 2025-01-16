using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SAE_APIGestion.Models.EntityFramework;

namespace SAE_APIGestion.Models.DataManger
{ 
    /// <summary>
    /// Cette classe est responsable de la gestion des entités <see cref="Batiment"/> dans la base de données.
    /// Elle implémente l'interface <see cref="IDataRepository{Batiment}"/> pour fournir des opérations CRUD.
    /// </summary>
    public class BatimentManager : IDataRepository<Batiment>
    {
        /// <summary>
        /// Contexte de la base de données pour interagir avec la table <see cref="Batiment"/>.
        /// </summary>
        readonly GlobalDBContext? dbContext;

        /// <summary>
        /// Constructeur par défaut pour <see cref="BatimentManager"/>.
        /// </summary>
        public BatimentManager() { }

        /// <summary>
        /// Constructeur de <see cref="BatimentManager"/> avec un contexte de base de données fourni.
        /// </summary>
        /// <param name="context">Le contexte de la base de données à utiliser.</param>
        public BatimentManager(GlobalDBContext context)
        {
            dbContext = context;
        }

        /// <summary>
        /// Récupère tous les bâtiments, y compris leurs salles, murs, équipements, capteurs et types de salles associés.
        /// </summary>
        /// <returns>Une liste de bâtiments avec toutes leurs entités associées.</returns>
        public async Task<ActionResult<IEnumerable<Batiment>>> GetAllAsync()
        {
            return await dbContext.Batiments
                .AsNoTracking()
                .Include(b => b.Salles)
                    .ThenInclude(s => s.Murs)
                        .ThenInclude(m => m.Equipements)
                            .ThenInclude(e => e.TypeEquipement)
                .Include(b => b.Salles)
                    .ThenInclude(s => s.Murs)
                        .ThenInclude(m => m.Capteurs)
                            .ThenInclude(c => c.DonneesCapteurs)
                .Include(b => b.Salles)
                    .ThenInclude(s => s.TypeSalle)
                .ToListAsync();
        }

        /// <summary>
        /// Récupère un bâtiment spécifique par son identifiant, avec toutes ses entités associées.
        /// </summary>
        /// <param name="id">L'identifiant du bâtiment à récupérer.</param>
        /// <returns>Le bâtiment correspondant à l'identifiant, ou null si non trouvé.</returns>
        public async Task<ActionResult<Batiment>> GetByIdAsync(int id)
        {
            return await dbContext.Batiments
                .AsNoTracking()
                .Include(b => b.Salles)
                    .ThenInclude(s => s.Murs)
                        .ThenInclude(m => m.Equipements)
                            .ThenInclude(e => e.TypeEquipement)
                .Include(b => b.Salles)
                    .ThenInclude(s => s.Murs)
                        .ThenInclude(m => m.Capteurs)
                            .ThenInclude(c => c.DonneesCapteurs)
                .Include(b => b.Salles)
                    .ThenInclude(s => s.TypeSalle)
                .FirstOrDefaultAsync(b => b.BatimentId == id);
        }

        /// <summary>
        /// Ajoute un nouveau bâtiment à la base de données.
        /// </summary>
        /// <param name="entity">Le bâtiment à ajouter.</param>
        /// <returns>Une tâche représentant l'opération asynchrone.</returns>
        public async Task AddAsync(Batiment entity)
        {
            await dbContext.Batiments.AddAsync(entity);
            await dbContext.SaveChangesAsync();
        }

        /// <summary>
        /// Supprime un bâtiment de la base de données.
        /// </summary>
        /// <param name="entity">Le bâtiment à supprimer.</param>
        /// <returns>Une tâche représentant l'opération asynchrone.</returns>
        public async Task DeleteAsync(Batiment entity)
        {
            dbContext.Batiments.Remove(entity);
            await dbContext.SaveChangesAsync();
        }

        /// <summary>
        /// Met à jour un bâtiment existant dans la base de données avec de nouvelles valeurs.
        /// </summary>
        /// <param name="enseignant">L'entité à mettre à jour (un bâtiment existant).</param>
        /// <param name="entity">Les nouvelles valeurs à appliquer à l'entité.</param>
        /// <returns>Une tâche représentant l'opération asynchrone.</returns>
        public async Task UpdateAsync(Batiment enseignant, Batiment entity)
        {
            dbContext.Entry(enseignant).State = EntityState.Modified;
            enseignant.BatimentId = entity.BatimentId;
            enseignant.Adresse = entity.Adresse;
            enseignant.Salles = entity.Salles;
            enseignant.Nom = entity.Nom;

            await dbContext.SaveChangesAsync();
        }
    }
}