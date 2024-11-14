using System.Collections.Generic;
using SAE_APIGestion.DataManager.Interfaces;
using SAE_APIGestion.Models.EntityFramework;
using SAE_APIGestion.Repository.Interfaces;

namespace SAE_APIGestion.DataManager.Implémentations
{
    public class EquipementManager : IEquipementManager
    {
        private readonly IEquipementRepository _repository;

        public EquipementManager(IEquipementRepository repository)
        {
            _repository = repository;
        }

        public IEnumerable<Equipement> GetAllEquipements() => _repository.GetAll();

        public Equipement GetEquipementById(int id) => _repository.GetById(id);

        public void CreateEquipement(Equipement equipement)
        {
            _repository.Add(equipement);
        }

        public void UpdateEquipement(Equipement equipement)
        {
            _repository.Update(equipement);
        }

        public void DeleteEquipement(int id) => _repository.Delete(id);
    }
}