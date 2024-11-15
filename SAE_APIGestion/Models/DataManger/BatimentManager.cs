using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SAE_APIGestion.Models.EntityFramework;

namespace SAE_APIGestion.Models.DataManger
{
    public class BatimentManager : IDataRepository<Batiment>
    {

        readonly GlobalDBContext? dbContext;

        public BatimentManager() { }

        public BatimentManager(GlobalDBContext context)
        {
            dbContext = context;
        }

        public async Task<ActionResult<IEnumerable<Batiment>>> GetAllAsync()
        {
            return await dbContext.Batiments
                .ToListAsync();
        }

        public async Task<ActionResult<Batiment>> GetByIdAsync(int id)
        {
            return await dbContext.Batiments.FirstOrDefaultAsync(u => u.Id == id);

        }

        public async Task AddAsync(Batiment entity)
        {
            await dbContext.Batiments.AddAsync(entity);
            await dbContext.SaveChangesAsync();
        }

        public async Task DeleteAsync(Batiment entity)
        {
            dbContext.Batiments.Remove(entity);
            await dbContext.SaveChangesAsync();
        }


        public async Task UpdateAsync(Batiment enseignant, Batiment entity)
        {
            dbContext.Entry(enseignant).State = EntityState.Modified;
            enseignant.Id = entity.Id;
            enseignant.Adresse = entity.Adresse;
            enseignant.Salles = entity.Salles;
            enseignant.Nom = entity.Nom;

            await dbContext.SaveChangesAsync();
        }
    }
}
