using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SAE_APIGestion.Models.EntityFramework;

namespace SAE_APIGestion.Models.DataManger
{
    /// <summary>
    /// Classe responsable de la gestion des entités <see cref="Salle"/> dans la base de données.
    /// Elle implémente l'interface <see cref="IDataRepository{Salle}"/> pour offrir des opérations CRUD.
    /// </summary>
    public class SalleManager : IDataRepository<Salle>
    {
        /// <summary>
        /// Contexte de la base de données pour interagir avec la table <see cref="Salle"/>.
        /// </summary>
        readonly GlobalDBContext? globalDBContext;

        /// <summary>
        /// Constructeur par défaut pour <see cref="SalleManager"/>.
        /// </summary>
        public SalleManager() { }

        /// <summary>
        /// Constructeur de <see cref="SalleManager"/> avec un contexte de base de données fourni.
        /// </summary>
        /// <param name="context">Le contexte de la base de données à utiliser.</param>
        public SalleManager(GlobalDBContext context)
        {
            globalDBContext = context;
        }

        /// <summary>
        /// Récupère toutes les salles, y compris les informations sur leurs murs, équipements, capteurs et données associées.
        /// </summary>
        /// <returns>Une liste de salles avec toutes leurs entités associées.</returns>
        public async Task<ActionResult<IEnumerable<Salle>>> GetAllAsync()
        {
            return globalDBContext.Salles
                .AsNoTracking()
                .Include(b => b.TypeSalle)
                .Include(b => b.Batiment)
                .Include(s => s.Murs)
                    .ThenInclude(m => m.Equipements)
                        .ThenInclude(e => e.TypeEquipement)
                .Include(s => s.Murs)
                    .ThenInclude(m => m.Capteurs)
                        .ThenInclude(c => c.DonneesCapteurs)
                .ToList();
        }

        /// <summary>
        /// Récupère une salle spécifique par son identifiant, avec ses murs, équipements, capteurs et données associées.
        /// </summary>
        /// <param name="id">L'identifiant de la salle à récupérer.</param>
        /// <returns>La salle correspondante à l'identifiant, ou null si non trouvée.</returns>
        public async Task<ActionResult<Salle>> GetByIdAsync(int id)
        {
            return globalDBContext.Salles
                .AsNoTracking()
                .Include(b => b.TypeSalle)
                .Include(b => b.Batiment)
                .Include(s => s.Murs)
                    .ThenInclude(m => m.Equipements)
                        .ThenInclude(e => e.TypeEquipement)
                .Include(s => s.Murs)
                    .ThenInclude(m => m.Capteurs)
                        .ThenInclude(c => c.DonneesCapteurs)
                .FirstOrDefault(p => p.SalleId == id);
        }

        /// <summary>
        /// Ajoute une nouvelle salle à la base de données.
        /// </summary>
        /// <param name="entity">La salle à ajouter.</param>
        /// <returns>Une tâche représentant l'opération asynchrone.</returns>
        public async Task AddAsync(Salle entity)
        {
            globalDBContext.Salles.Add(entity);
            globalDBContext.SaveChanges();
        }

        /// <summary>
        /// Met à jour une salle existante avec de nouvelles informations dans la base de données.
        /// </summary>
        /// <param name="salle">La salle à mettre à jour (ancienne salle).</param>
        /// <param name="entity">Les nouvelles valeurs à appliquer à la salle.</param>
        /// <returns>Une tâche représentant l'opération asynchrone.</returns>
        public async Task UpdateAsync(Salle salle, Salle entity)
        {
            globalDBContext.Entry(salle).State = EntityState.Modified;
            salle.Nom = entity.Nom;
            salle.Surface = entity.Surface;
            salle.TypeSalleId = entity.TypeSalleId;
            salle.BatimentId = entity.BatimentId;
            await globalDBContext.SaveChangesAsync();
        }

        /// <summary>
        /// Supprime une salle de la base de données.
        /// </summary>
        /// <param name="salle">La salle à supprimer.</param>
        /// <returns>Une tâche représentant l'opération asynchrone.</returns>
        public async Task DeleteAsync(Salle salle)
        {
            globalDBContext.Salles.Remove(salle);
            globalDBContext.SaveChanges();
        }
    }
}