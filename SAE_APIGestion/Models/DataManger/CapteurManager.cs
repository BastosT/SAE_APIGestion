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
                .ToListAsync();
        }

        public async Task<ActionResult<Capteur>> GetByIdAsync(int id)
        {
            return await dbContext.Capteurs.FirstOrDefaultAsync(u => u. == id);

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


        public async Task UpdateAsync(Capteur enseignant, Batiment entity)
        {
            dbContext.Entry(enseignant).State = EntityState.Modified;
            enseignant.Id = entity.Id;

            await dbContext.SaveChangesAsync();
        }
    }


}
