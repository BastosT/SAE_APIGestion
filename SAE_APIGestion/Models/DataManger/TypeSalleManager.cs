using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SAE_APIGestion.Models.EntityFramework;

namespace SAE_APIGestion.Models.DataManger
{
    /// <summary>
    /// Classe responsable de la gestion des entités <see cref="TypeSalle"/> dans la base de données.
    /// Elle implémente l'interface <see cref="IDataRepository{TypeSalle}"/> pour offrir des opérations CRUD.
    /// </summary>
    public class TypeSalleManager : IDataRepository<TypeSalle>
    {
        /// <summary>
        /// Contexte de la base de données pour interagir avec la table <see cref="TypeSalle"/>.
        /// </summary>
        readonly GlobalDBContext? globalDBContext;

        /// <summary>
        /// Constructeur par défaut pour <see cref="TypeSalleManager"/>.
        /// </summary>
        public TypeSalleManager() { }

        /// <summary>
        /// Constructeur de <see cref="TypeSalleManager"/> avec un contexte de base de données fourni.
        /// </summary>
        /// <param name="context">Le contexte de la base de données à utiliser.</param>
        public TypeSalleManager(GlobalDBContext context)
        {
            globalDBContext = context;
        }

        /// <summary>
        /// Récupère tous les types de salle, y compris les salles associées.
        /// </summary>
        /// <returns>Une liste de tous les types de salle avec les salles associées.</returns>
        public async Task<ActionResult<IEnumerable<TypeSalle>>> GetAllAsync()
        {
            return globalDBContext.TypesSalles.AsNoTracking()
                .Include(s => s.Salles)
                .ToList();
        }

        /// <summary>
        /// Récupère un type de salle spécifique par son identifiant, avec les salles associées.
        /// </summary>
        /// <param name="id">L'identifiant du type de salle à récupérer.</param>
        /// <returns>Le type de salle correspondant à l'identifiant, ou null si non trouvé.</returns>
        public async Task<ActionResult<TypeSalle>> GetByIdAsync(int id)
        {
            return globalDBContext.TypesSalles.AsNoTracking()
                .Include(s => s.Salles)
                .FirstOrDefault(p => p.TypeSalleId == id);
        }


        /// <summary>
        /// Ajoute un nouveau type de salle à la base de données.
        /// </summary>
        /// <param name="entity">Le type de salle à ajouter.</param>
        /// <returns>Une tâche représentant l'opération asynchrone.</returns>
        public async Task AddAsync(TypeSalle entity)
        {
            globalDBContext.TypesSalles.Add(entity);
            globalDBContext.SaveChanges();
        }

        /// <summary>
        /// Met à jour un type de salle existant avec de nouvelles informations dans la base de données.
        /// </summary>
        /// <param name="TypeSalle">Le type de salle existant à mettre à jour.</param>
        /// <param name="entity">Les nouvelles valeurs à appliquer au type de salle.</param>
        /// <returns>Une tâche représentant l'opération asynchrone.</returns>
        public async Task UpdateAsync(TypeSalle TypeSalle, TypeSalle entity)
        {
            globalDBContext.Entry(TypeSalle).State = EntityState.Modified;
            TypeSalle.Nom = entity.Nom;
            TypeSalle.Description = entity.Description;
    

            globalDBContext.SaveChanges();
        }

        /// <summary>
        /// Supprime un type de salle de la base de données.
        /// </summary>
        /// <param name="TypeSalle">Le type de salle à supprimer.</param>
        /// <returns>Une tâche représentant l'opération asynchrone.</returns>
        public async Task DeleteAsync(TypeSalle TypeSalle)
        {
            globalDBContext.TypesSalles.Remove(TypeSalle);
            globalDBContext.SaveChanges();
        }


    }
}