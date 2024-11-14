using System.Collections.Generic;
using SAE_APIGestion.Models.EntityFramework;

namespace SAE_APIGestion.Repository.Interfaces
{
    public interface IEquipementRepository
    {
        IEnumerable<Equipement> GetAll();
        Equipement GetById(int id);
        void Add(Equipement equipement);
        void Update(Equipement equipement);
        void Delete(int id);
    }
}