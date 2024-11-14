using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using SAE_APIGestion.Models.EntityFramework;
using SAE_APIGestion.Repository.Interfaces;

namespace SAE_APIGestion.Repository.Implémentations
{
    public class EquipementRepository : IEquipementRepository
    {
        private readonly MyDbContext _context;

        public EquipementRepository(MyDbContext context)
        {
            _context = context;
        }

        public IEnumerable<Equipement> GetAll() => _context.Equipements.Include(e => e.Type).Include(e => e.Mur).Include(e => e.Salle).ToList();

        public Equipement GetById(int id) => _context.Equipements.Include(e => e.Type).Include(e => e.Mur).Include(e => e.Salle).FirstOrDefault(e => e.Id == id);

        public void Add(Equipement equipement)
        {
            _context.Equipements.Add(equipement);
            _context.SaveChanges();
        }

        public void Update(Equipement equipement)
        {
            _context.Equipements.Update(equipement);
            _context.SaveChanges();
        }

        public void Delete(int id)
        {
            var equipement = GetById(id);
            if (equipement != null)
            {
                _context.Equipements.Remove(equipement);
                _context.SaveChanges();
            }
        }
    }
}