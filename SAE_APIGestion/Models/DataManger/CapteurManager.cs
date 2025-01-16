using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SAE_APIGestion.Models.EntityFramework;

namespace SAE_APIGestion.Models.DataManger
{
    /// <summary>
    /// Cette classe est responsable de la gestion des entités <see cref="Capteur"/> dans la base de données.
    /// Elle implémente l'interface <see cref="IDataRepository{Capteur}"/> pour fournir des opérations CRUD.
    /// </summary>
    public class CapteurManager : IDataRepository<Capteur>
    {

        /// <summary>
        /// Contexte de la base de données pour interagir avec la table <see cref="Capteur"/>.
        /// </summary>
        readonly GlobalDBContext? dbContext;

        /// <summary>
        /// Constructeur par défaut pour <see cref="CapteurManager"/>.
        /// </summary>
        public CapteurManager() { }

        /// <summary>
        /// Constructeur de <see cref="CapteurManager"/> avec un contexte de base de données fourni.
        /// </summary>
        /// <param name="context">Le contexte de la base de données à utiliser.</param>
        public CapteurManager(GlobalDBContext context)
        {
            dbContext = context;
        }

        /// <summary>
        /// Récupère tous les capteurs, y compris leurs murs, salles, bâtiments et données associées.
        /// </summary>
        /// <returns>Une liste de capteurs avec toutes leurs entités associées.</returns>
        public async Task<ActionResult<IEnumerable<Capteur>>> GetAllAsync()
        {
            return await dbContext.Capteurs
                .AsNoTracking()
                  .Include(b => b.Mur)
                  .Include(b => b.Salle)
                    .ThenInclude(b => b.Batiment)
                  .Include(b => b.DonneesCapteurs)
                  .ToListAsync();
        }

        /// <summary>
        /// Récupère un capteur spécifique par son identifiant, avec toutes ses entités associées.
        /// </summary>
        /// <param name="id">L'identifiant du capteur à récupérer.</param>
        /// <returns>Le capteur correspondant à l'identifiant, ou null si non trouvé.</returns>
        public async Task<ActionResult<Capteur>> GetByIdAsync(int id)
        {
            return await dbContext.Capteurs
                .AsNoTracking()
                  .Include(b => b.Mur)
                  .Include(b => b.Salle)
                  .ThenInclude(b => b.Batiment)
                  .Include(b => b.DonneesCapteurs)
                  .FirstOrDefaultAsync(b => b.CapteurId == id);
        }

        /// <summary>
        /// Ajoute un nouveau capteur à la base de données.
        /// </summary>
        /// <param name="entity">Le capteur à ajouter.</param>
        /// <returns>Une tâche représentant l'opération asynchrone.</returns>
        public async Task AddAsync(Capteur entity)
        {
            await dbContext.Capteurs.AddAsync(entity);
            await dbContext.SaveChangesAsync();
        }

        /// <summary>
        /// Supprime un capteur de la base de données.
        /// </summary>
        /// <param name="entity">Le capteur à supprimer.</param>
        /// <returns>Une tâche représentant l'opération asynchrone.</returns>
        public async Task DeleteAsync(Capteur entity)
        {
            dbContext.Capteurs.Remove(entity);
            await dbContext.SaveChangesAsync();
        }


        /// <summary>
        /// Met à jour un capteur existant dans la base de données avec de nouvelles valeurs.
        /// </summary>
        /// <param name="capteur">L'entité à mettre à jour (un capteur existant).</param>
        /// <param name="entity">Les nouvelles valeurs à appliquer à l'entité.</param>
        /// <returns>Une tâche représentant l'opération asynchrone.</returns>
        public async Task UpdateAsync(Capteur capteur, Capteur entity)
        {
            dbContext.Entry(capteur).State = EntityState.Modified;
            capteur.Nom = entity.Nom;
            capteur.Longueur = entity.Longueur;
            capteur.Hauteur = entity.Hauteur;
            capteur.DistanceChauffage = entity.DistanceChauffage;
            capteur.DistancePorte = entity.DistancePorte;
            capteur.DistanceFenetre = entity.DistanceFenetre;
            capteur.PositionX = entity.PositionX;
            capteur.PositionY = entity.PositionY;
            capteur.EstActif = entity.EstActif;
            capteur.MurId = entity.MurId;
            capteur.SalleId = entity.SalleId;

            await dbContext.SaveChangesAsync();
        }


    }

    /// <summary>
    /// Cette classe est responsable de la gestion des entités <see cref="TypeDonneesCapteur"/> dans la base de données.
    /// Elle implémente l'interface <see cref="IDataRepository{TypeDonneesCapteur}"/> pour fournir des opérations CRUD.
    /// </summary>
    public class TypeDonneesCapteurManager : IDataRepository<TypeDonneesCapteur>
    {

        /// <summary>
        /// Contexte de la base de données pour interagir avec la table <see cref="TypeDonneesCapteur"/>.
        /// </summary>
        readonly GlobalDBContext? dbContext;

        /// <summary>
        /// Constructeur par défaut pour <see cref="TypeDonneesCapteurManager"/>.
        /// </summary>
        public TypeDonneesCapteurManager() { }

        /// <summary>
        /// Constructeur de <see cref="TypeDonneesCapteurManager"/> avec un contexte de base de données fourni.
        /// </summary>
        /// <param name="context">Le contexte de la base de données à utiliser.</param>
        public TypeDonneesCapteurManager(GlobalDBContext context)
        {
            dbContext = context;
        }

        /// <summary>
        /// Récupère tous les types de données de capteurs, y compris leurs données associées.
        /// </summary>
        /// <returns>Une liste de types de données de capteurs avec toutes leurs entités associées.</returns>
        public async Task<ActionResult<IEnumerable<TypeDonneesCapteur>>> GetAllAsync()
        {
            return await dbContext.TypesDonneesCapteurs
                .Include(b => b.DonneesCapteurs)
                .ToListAsync();
        }

        /// <summary>
        /// Récupère un type de données de capteur spécifique par son identifiant, avec toutes ses données associées.
        /// </summary>
        /// <param name="id">L'identifiant du type de données de capteur à récupérer.</param>
        /// <returns>Le type de données de capteur correspondant à l'identifiant, ou null si non trouvé.</returns>
        public async Task<ActionResult<TypeDonneesCapteur>> GetByIdAsync(int id)
        {
            return await dbContext.TypesDonneesCapteurs.Include(b => b.DonneesCapteurs).FirstOrDefaultAsync(u => u.TypeDonneesCapteurId == id);

        }

        /// <summary>
        /// Ajoute un nouveau type de données de capteur à la base de données.
        /// </summary>
        /// <param name="entity">Le type de données de capteur à ajouter.</param>
        /// <returns>Une tâche représentant l'opération asynchrone.</returns>
        public async Task AddAsync(TypeDonneesCapteur entity)
        {
            await dbContext.TypesDonneesCapteurs.AddAsync(entity);
            await dbContext.SaveChangesAsync();
        }

        /// <summary>
        /// Supprime un type de données de capteur de la base de données.
        /// </summary>
        /// <param name="entity">Le type de données de capteur à supprimer.</param>
        /// <returns>Une tâche représentant l'opération asynchrone.</returns>
        public async Task DeleteAsync(TypeDonneesCapteur entity)
        {
            dbContext.TypesDonneesCapteurs.Remove(entity);
            await dbContext.SaveChangesAsync();
        }


        /// <summary>
        /// Met à jour un type de données de capteur existant dans la base de données avec de nouvelles valeurs.
        /// </summary>
        /// <param name="capteur">L'entité à mettre à jour (un type de données de capteur existant).</param>
        /// <param name="entity">Les nouvelles valeurs à appliquer à l'entité.</param>
        /// <returns>Une tâche représentant l'opération asynchrone.</returns>
        public async Task UpdateAsync(TypeDonneesCapteur capteur, TypeDonneesCapteur entity)
        {
            dbContext.Entry(capteur).State = EntityState.Modified;
            capteur.TypeDonneesCapteurId = entity.TypeDonneesCapteurId;
            capteur.Unite = entity.Unite;
            capteur.Nom = entity.Nom;
            capteur.DonneesCapteurs = entity.DonneesCapteurs;
            await dbContext.SaveChangesAsync();
        }

    }

    /// <summary>
    /// Cette classe est responsable de la gestion des entités <see cref="DonneesCapteur"/> dans la base de données.
    /// Elle implémente l'interface <see cref="IDataRepository{DonneesCapteur}"/> pour fournir des opérations CRUD.
    /// </summary>
    public class DonneesCapteurManager : IDataRepository<DonneesCapteur>
    {

        /// <summary>
        /// Contexte de la base de données pour interagir avec la table <see cref="DonneesCapteur"/>.
        /// </summary>
        readonly GlobalDBContext? dbContext;

        /// <summary>
        /// Constructeur par défaut pour <see cref="DonneesCapteurManager"/>.
        /// </summary>
        public DonneesCapteurManager() { }

        /// <summary>
        /// Constructeur de <see cref="DonneesCapteurManager"/> avec un contexte de base de données fourni.
        /// </summary>
        /// <param name="context">Le contexte de la base de données à utiliser.</param>
        public DonneesCapteurManager(GlobalDBContext context)
        {
            dbContext = context;
        }

        /// <summary>
        /// Récupère toutes les données des capteurs, y compris les capteurs et leurs types de données associés.
        /// </summary>
        /// <returns>Une liste de données de capteurs avec toutes leurs entités associées.</returns>
        public async Task<ActionResult<IEnumerable<DonneesCapteur>>> GetAllAsync()
        {
            return await dbContext.DonneesCapteurs
                .Include(b => b.Capteur)
                .Include(b => b.TypeDonnees)
                .ToListAsync();
        }

        /// <summary>
        /// Récupère une donnée de capteur spécifique par son identifiant, avec toutes ses entités associées.
        /// </summary>
        /// <param name="id">L'identifiant de la donnée de capteur à récupérer.</param>
        /// <returns>La donnée de capteur correspondant à l'identifiant, ou null si non trouvé.</returns>
        public async Task<ActionResult<DonneesCapteur>> GetByIdAsync(int id)
        {
            return await dbContext.DonneesCapteurs
                .Include(b => b.Capteur)
                .Include(b => b.TypeDonnees)
                .FirstOrDefaultAsync(u => u.DonneesCapteurId == id);

        }

        /// <summary>
        /// Ajoute une nouvelle donnée de capteur à la base de données.
        /// </summary>
        /// <param name="entity">La donnée de capteur à ajouter.</param>
        /// <returns>Une tâche représentant l'opération asynchrone.</returns>
        public async Task AddAsync(DonneesCapteur entity)
        {
            await dbContext.DonneesCapteurs.AddAsync(entity);
            await dbContext.SaveChangesAsync();
        }

        /// <summary>
        /// Supprime une donnée de capteur de la base de données.
        /// </summary>
        /// <param name="entity">La donnée de capteur à supprimer.</param>
        /// <returns>Une tâche représentant l'opération asynchrone.</returns>
        public async Task DeleteAsync(DonneesCapteur entity)
        {
            dbContext.DonneesCapteurs.Remove(entity);
            await dbContext.SaveChangesAsync();
        }


        /// <summary>
        /// Met à jour une donnée de capteur existante dans la base de données avec de nouvelles valeurs.
        /// </summary>
        /// <param name="capteur">L'entité à mettre à jour (une donnée de capteur existante).</param>
        /// <param name="entity">Les nouvelles valeurs à appliquer à l'entité.</param>
        /// <returns>Une tâche représentant l'opération asynchrone.</returns>
        public async Task UpdateAsync(DonneesCapteur capteur, DonneesCapteur entity)
        {
            dbContext.Entry(capteur).State = EntityState.Modified;
            capteur.DonneesCapteurId = entity.DonneesCapteurId;
            capteur.Capteur= entity.Capteur;
            capteur.CapteurId= entity.CapteurId;
            capteur.Valeur= entity.Valeur;
            capteur.Timestamp= entity.Timestamp;
            capteur.TypeDonnees= entity.TypeDonnees;
            capteur.TypeDonneesId= entity.TypeDonneesId;

            await dbContext.SaveChangesAsync();
        }

    }
}
