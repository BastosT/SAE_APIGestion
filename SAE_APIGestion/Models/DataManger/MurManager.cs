using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SAE_APIGestion.Models.EntityFramework;

namespace SAE_APIGestion.Models.DataManger
{
    public class MurManager : IDataRepository<Mur>
    {

        readonly GlobalDBContext? dbContext;

        public MurManager() { }

        public MurManager(GlobalDBContext context)
        {
            dbContext = context;
        }

        public async Task<ActionResult<IEnumerable<Mur>>> GetAllAsync()
        {
            return await dbContext.Murs
                .Include(b => b.Salle)
                .Include(b => b.Equipements)
                .Include(b => b.Capteurs)
                .ToListAsync();
        }

        public async Task<ActionResult<Mur>> GetByIdAsync(int id)
        {
            return await dbContext.Murs
                .Include(b => b.Salle)
                .Include(b => b.Equipements)
                .Include(b => b.Capteurs)
                .FirstOrDefaultAsync(u => u.MurId == id);

        }

        public async Task AddAsync(Mur entity)
        {
            await dbContext.Murs.AddAsync(entity);
            await dbContext.SaveChangesAsync();
        }

        public async Task DeleteAsync(Mur entity)
        {
            dbContext.Murs.Remove(entity);
            await dbContext.SaveChangesAsync();
        }


        public async Task UpdateAsync(Mur enseignant, Mur entity)
        {
            dbContext.Entry(enseignant).State = EntityState.Modified;
            enseignant.MurId = entity.MurId;
            enseignant.Nom = entity.Nom;
            enseignant.Longueur = entity.Longueur;
            enseignant.Hauteur = entity.Hauteur;
            enseignant.SalleId = entity.SalleId;
            enseignant.Salle = entity.Salle;
            enseignant.Equipements = entity.Equipements;
            enseignant.Capteurs = entity.Capteurs;

            await dbContext.SaveChangesAsync();
        }

    }
}
