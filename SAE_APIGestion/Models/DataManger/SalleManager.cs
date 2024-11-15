using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SAE_APIGestion.Models.EntityFramework;

namespace SAE_APIGestion.Models.DataManger
{
    public class SalleManager : IDataRepository<Salle>
    {
        readonly GlobalDBContext? globalDBContext;

        public SalleManager() { }

        public SalleManager(GlobalDBContext context)
        {
            globalDBContext = context;
        }

        public async Task<ActionResult<IEnumerable<Salle>>> GetAllAsync()
        {
            return globalDBContext.Salles.ToList();
        }

        public async Task<ActionResult<Salle>> GetByIdAsync(int id)
        {
            return globalDBContext.Salles.FirstOrDefault(p => p.SalleId == id);
        }


        public async Task AddAsync(Salle entity)
        {
            globalDBContext.Salles.Add(entity);
            globalDBContext.SaveChanges();
        }

        public async Task UpdateAsync(Salle salle, Salle entity)
        {
            globalDBContext.Entry(salle).State = EntityState.Modified;
            salle.Nom = entity.Nom;
            salle.Surface = entity.Surface;
            salle.TypeSalleId = entity.TypeSalleId;
            salle.BatimentId = entity.BatimentId;
            await globalDBContext.SaveChangesAsync();
        }

        public async Task DeleteAsync(Salle salle)
        {
            globalDBContext.Salles.Remove(salle);
            globalDBContext.SaveChanges();
        }


    }
}
