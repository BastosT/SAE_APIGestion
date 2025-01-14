using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SAE_APIGestion.Models.EntityFramework;

namespace SAE_APIGestion.Models.DataManger
{
    public class EquipementManager : IDataRepository<Equipement>
    {
        readonly GlobalDBContext? globalDbContext;

        public EquipementManager() { }

        public EquipementManager(GlobalDBContext context)
        {
            globalDbContext = context;
        }

        public async Task<ActionResult<IEnumerable<Equipement>>> GetAllAsync()
        {
            return await globalDbContext.Equipements
                .AsNoTracking()
                .Include(b => b.Mur)
                .Include(b => b.Salle)
                .Include(b => b.TypeEquipement)
                
                .ToListAsync();
        }

        public async Task<ActionResult<Equipement>> GetByIdAsync(int id)
        {
            return await globalDbContext.Equipements
                .AsNoTracking()
                 .Include(b => b.Mur)
                .Include(b => b.Salle)
                .Include(b => b.TypeEquipement)
                .FirstOrDefaultAsync(u => u.EquipementId == id);
        }

        public async Task<ActionResult<Equipement>> GetByStringAsync(string nom)
        {
            return await globalDbContext.Equipements.FirstOrDefaultAsync(u => u.Nom.ToUpper() == nom.ToUpper());
        }

        public async Task AddAsync(Equipement entity)
        {
            await globalDbContext.Equipements.AddAsync(entity);
            await globalDbContext.SaveChangesAsync();
        }

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

        public async Task DeleteAsync(Equipement equipement)
        {
            globalDbContext.Equipements.Remove(equipement);
            await globalDbContext.SaveChangesAsync();
        }
    }
}
