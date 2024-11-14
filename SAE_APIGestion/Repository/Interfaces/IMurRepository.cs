using System.Collections.Generic;
using SAE_APIGestion.Models.EntityFramework;

namespace SAE_APIGestion.Repository.Interfaces
{
    public interface IMurRepository
    {
        IEnumerable<Mur> GetAll();
        Mur GetById(int id);
        void Add(Mur mur);
        void Update(Mur mur);
        void Delete(int id);
    }
}