using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SAE_APIGestion.Models.EntityFramework;

namespace SAE_APIGestion.Models.DataManger
{
    public class TypeEquipementManager : IDataRepository<TypeEquipement>
    {

        readonly GlobalDBContext? globalDBContext;

        public TypeEquipementManager() { }

        public TypeEquipementManager(GlobalDBContext context)
        {
            globalDBContext = context;
        }

        public async Task<ActionResult<IEnumerable<TypeEquipement>>> GetAllAsync()
        {
            return globalDBContext.TypesEquipements.ToList();
        }

        public async Task<ActionResult<TypeEquipement>> GetByIdAsync(int id)
        {
            return globalDBContext.TypesEquipements.FirstOrDefault(p => p.TypeEquipementId == id);
        }


        public async Task AddAsync(TypeEquipement entity)
        {
            globalDBContext.TypesEquipements.Add(entity);
            globalDBContext.SaveChanges();
        }

        public async Task UpdateAsync(TypeEquipement typeEquipement, TypeEquipement entity)
        {
            globalDBContext.Entry(typeEquipement).State = EntityState.Modified;
            typeEquipement.TypeEquipementId = entity.TypeEquipementId;
            typeEquipement.Nom = entity.Nom;
   
            globalDBContext.SaveChanges();
        }

        public async Task DeleteAsync(TypeEquipement typeEquipement)
        {
            globalDBContext.TypesEquipements.Remove(typeEquipement);
            globalDBContext.SaveChanges();
        }

    }
}
