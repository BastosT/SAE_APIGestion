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
                .AsNoTracking()
                .Include(b => b.Equipements)
                    .ThenInclude(b => b.TypeEquipement)
                .Include(b => b.Capteurs)
                .Include(b => b.Salle)
                .ToListAsync();
        }

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
            enseignant.Orientation = entity.Orientation;
            enseignant.Equipements = entity.Equipements;
            enseignant.Capteurs = entity.Capteurs;
            await dbContext.SaveChangesAsync();
        }
    }
}