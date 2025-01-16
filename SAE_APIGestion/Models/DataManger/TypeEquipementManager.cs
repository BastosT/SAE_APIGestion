using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SAE_APIGestion.Models.EntityFramework;

namespace SAE_APIGestion.Models.DataManger
{
    /// <summary>
    /// Classe responsable de la gestion des entités <see cref="TypeEquipement"/> dans la base de données.
    /// Elle implémente l'interface <see cref="IDataRepository{TypeEquipement}"/> pour offrir des opérations CRUD.
    /// </summary>
    public class TypeEquipementManager : IDataRepository<TypeEquipement>
    {

        /// <summary>
        /// Contexte de la base de données pour interagir avec la table <see cref="TypeEquipement"/>.
        /// </summary>
        readonly GlobalDBContext? globalDBContext;

        /// <summary>
        /// Constructeur par défaut pour <see cref="TypeEquipementManager"/>.
        /// </summary>
        public TypeEquipementManager() { }

        /// <summary>
        /// Constructeur de <see cref="TypeEquipementManager"/> avec un contexte de base de données fourni.
        /// </summary>
        /// <param name="context">Le contexte de la base de données à utiliser.</param>
        public TypeEquipementManager(GlobalDBContext context)
        {
            globalDBContext = context;
        }

        /// <summary>
        /// Récupère tous les types d'équipements, y compris les équipements associés.
        /// </summary>
        /// <returns>Une liste de tous les types d'équipements avec les équipements associés.</returns>
        public async Task<ActionResult<IEnumerable<TypeEquipement>>> GetAllAsync()
        {
            return globalDBContext.TypesEquipements
                .AsNoTracking()
                .Include(s => s.Equipements)
                .ToList();
        }

        /// <summary>
        /// Récupère un type d'équipement spécifique par son identifiant, avec les équipements associés.
        /// </summary>
        /// <param name="id">L'identifiant du type d'équipement à récupérer.</param>
        /// <returns>Le type d'équipement correspondant à l'identifiant, ou null si non trouvé.</returns>
        public async Task<ActionResult<TypeEquipement>> GetByIdAsync(int id)
        {
            return globalDBContext.TypesEquipements
                .AsNoTracking()
                .Include(s => s.Equipements)
                .FirstOrDefault(p => p.TypeEquipementId == id);
        }


        /// <summary>
        /// Ajoute un nouveau type d'équipement à la base de données.
        /// </summary>
        /// <param name="entity">Le type d'équipement à ajouter.</param>
        /// <returns>Une tâche représentant l'opération asynchrone.</returns>
        public async Task AddAsync(TypeEquipement entity)
        {
            globalDBContext.TypesEquipements.Add(entity);
            globalDBContext.SaveChanges();
        }

        /// <summary>
        /// Met à jour un type d'équipement existant avec de nouvelles informations dans la base de données.
        /// </summary>
        /// <param name="typeEquipement">Le type d'équipement existant à mettre à jour.</param>
        /// <param name="entity">Les nouvelles valeurs à appliquer au type d'équipement.</param>
        /// <returns>Une tâche représentant l'opération asynchrone.</returns>
        public async Task UpdateAsync(TypeEquipement typeEquipement, TypeEquipement entity)
        {
            globalDBContext.Entry(typeEquipement).State = EntityState.Modified;
            typeEquipement.Nom = entity.Nom;
            typeEquipement.Couleur = entity.Couleur;
   
            globalDBContext.SaveChanges();
        }

        /// <summary>
        /// Supprime un type d'équipement de la base de données.
        /// </summary>
        /// <param name="typeEquipement">Le type d'équipement à supprimer.</param>
        /// <returns>Une tâche représentant l'opération asynchrone.</returns>
        public async Task DeleteAsync(TypeEquipement typeEquipement)
        {
            globalDBContext.TypesEquipements.Remove(typeEquipement);
            globalDBContext.SaveChanges();
        }

    }
}
