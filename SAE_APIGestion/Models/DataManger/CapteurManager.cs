using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SAE_APIGestion.Models.EntityFramework;

namespace SAE_APIGestion.Models.DataManger
{
    public class CapteurManager : IDataRepository<Capteur>
    {

        readonly GlobalDBContext? dbContext;

        public CapteurManager() { }

        public CapteurManager(GlobalDBContext context)
        {
            dbContext = context;
        }

        public async Task<ActionResult<IEnumerable<Capteur>>> GetAllAsync()
        {
            return await dbContext.Capteurs
                 .Include(b => b.DonneesCapteurs)
                    .ThenInclude(s => s.TypeDonnees)
                .ToListAsync();
        }

        public async Task<ActionResult<Capteur>> GetByIdAsync(int id)
        {
            return await dbContext.Capteurs
                .Include(b => b.DonneesCapteurs)
                    .ThenInclude(s => s.TypeDonnees)
                .FirstOrDefaultAsync(u => u.CapteurId == id);

        }

        public async Task AddAsync(Capteur entity)
        {
            await dbContext.Capteurs.AddAsync(entity);
            await dbContext.SaveChangesAsync();
        }

        public async Task DeleteAsync(Capteur entity)
        {
            dbContext.Capteurs.Remove(entity);
            await dbContext.SaveChangesAsync();
        }


        public async Task UpdateAsync(Capteur capteur, Capteur entity)
        {
            dbContext.Entry(capteur).State = EntityState.Modified;
            capteur.CapteurId = entity.CapteurId;
            capteur.Nom = entity.Nom;
            capteur.Longueur = entity.Longueur;
            capteur.Hauteur = entity.Hauteur;
            capteur.DistanceChauffage = entity.DistanceChauffage;
            capteur.DistancePorte = entity.DistancePorte;
            capteur.DistanceFenetre = entity.DistanceFenetre;
            capteur.EstActif = entity.EstActif;
            capteur.Mur = entity.Mur;
            capteur.DonneesCapteurs = entity.DonneesCapteurs;

            await dbContext.SaveChangesAsync();
        }

    }

    public class TypeDonneesCapteurManager : IDataRepository<TypeDonneesCapteur>
    {

        readonly GlobalDBContext? dbContext;

        public TypeDonneesCapteurManager() { }

        public TypeDonneesCapteurManager(GlobalDBContext context)
        {
            dbContext = context;
        }

        public async Task<ActionResult<IEnumerable<TypeDonneesCapteur>>> GetAllAsync()
        {
            return await dbContext.TypesDonneesCapteurs
                .Include(b => b.DonneesCapteurs)
                .ToListAsync();
        }

        public async Task<ActionResult<TypeDonneesCapteur>> GetByIdAsync(int id)
        {
            return await dbContext.TypesDonneesCapteurs.Include(b => b.DonneesCapteurs).FirstOrDefaultAsync(u => u.TypeDonneesCapteurId == id);

        }

        public async Task AddAsync(TypeDonneesCapteur entity)
        {
            await dbContext.TypesDonneesCapteurs.AddAsync(entity);
            await dbContext.SaveChangesAsync();
        }

        public async Task DeleteAsync(TypeDonneesCapteur entity)
        {
            dbContext.TypesDonneesCapteurs.Remove(entity);
            await dbContext.SaveChangesAsync();
        }


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

    public class DonneesCapteurManager : IDataRepository<DonneesCapteur>
    {

        readonly GlobalDBContext? dbContext;

        public DonneesCapteurManager() { }

        public DonneesCapteurManager(GlobalDBContext context)
        {
            dbContext = context;
        }

        public async Task<ActionResult<IEnumerable<DonneesCapteur>>> GetAllAsync()
        {
            return await dbContext.DonneesCapteurs
                .Include(b => b.Capteur)
                .Include(b => b.TypeDonnees)
                .ToListAsync();
        }

        public async Task<ActionResult<DonneesCapteur>> GetByIdAsync(int id)
        {
            return await dbContext.DonneesCapteurs
                .Include(b => b.Capteur)
                .Include(b => b.TypeDonnees)
                .FirstOrDefaultAsync(u => u.DonneesCapteurId == id);

        }

        public async Task AddAsync(DonneesCapteur entity)
        {
            await dbContext.DonneesCapteurs.AddAsync(entity);
            await dbContext.SaveChangesAsync();
        }

        public async Task DeleteAsync(DonneesCapteur entity)
        {
            dbContext.DonneesCapteurs.Remove(entity);
            await dbContext.SaveChangesAsync();
        }


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
