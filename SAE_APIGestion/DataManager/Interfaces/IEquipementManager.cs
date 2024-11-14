using System.Collections.Generic;
using SAE_APIGestion.Models.EntityFramework;

namespace SAE_APIGestion.DataManager.Interfaces
{
    public interface IEquipementManager
    {
        IEnumerable<Equipement> GetAllEquipements();
        Equipement GetEquipementById(int id);
        void CreateEquipement(Equipement equipement);
        void UpdateEquipement(Equipement equipement);
        void DeleteEquipement(int id);
    }
}