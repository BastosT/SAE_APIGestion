using System.Collections.Generic;
using SAE_APIGestion.Models.EntityFramework;

namespace SAE_APIGestion.DataManager.Interfaces
{
    public interface IMurManager
    {
        IEnumerable<Mur> GetAllMurs();
        Mur GetMurById(int id);
        void CreateMur(Mur mur);
        void UpdateMur(Mur mur);
        void DeleteMur(int id);
    }
}