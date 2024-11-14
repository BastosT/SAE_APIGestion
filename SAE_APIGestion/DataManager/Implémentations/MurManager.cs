using System.Collections.Generic;
using SAE_APIGestion.DataManager.Interfaces;
using SAE_APIGestion.Models.EntityFramework;
using SAE_APIGestion.Repository.Interfaces;

namespace SAE_APIGestion.DataManager.Implémentations
{
    public class MurManager : IMurManager
    {
        private readonly IMurRepository _repository;

        public MurManager(IMurRepository repository)
        {
            _repository = repository;
        }

        public IEnumerable<Mur> GetAllMurs() => _repository.GetAll();

        public Mur GetMurById(int id) => _repository.GetById(id);

        public void CreateMur(Mur mur)
        {
            _repository.Add(mur);
        }

        public void UpdateMur(Mur mur)
        {
            _repository.Update(mur);
        }

        public void DeleteMur(int id) => _repository.Delete(id);
    }
}