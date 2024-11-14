using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using SAE_APIGestion.Models.EntityFramework;
using SAE_APIGestion.Repository.Interfaces;

namespace SAE_APIGestion.Repository.Implémentations
{
    public class MurRepository : IMurRepository
    {
        private readonly MyDbContext _context;

        public MurRepository(MyDbContext context)
        {
            _context = context;
        }

        public IEnumerable<Mur> GetAll() => _context.Murs.Include(m => m.Salle).Include(m => m.Equipements).Include(m => m.Capteurs).ToList();

        public Mur GetById(int id) => _context.Murs.Include(m => m.Salle).Include(m => m.Equipements).Include(m => m.Capteurs).FirstOrDefault(m => m.Id == id);

        public void Add(Mur mur)
        {
            _context.Murs.Add(mur);
            _context.SaveChanges();
        }

        public void Update(Mur mur)
        {
            _context.Murs.Update(mur);
            _context.SaveChanges();
        }

        public void Delete(int id)
        {
            var mur = GetById(id);
            if (mur != null)
            {
                _context.Murs.Remove(mur);
                _context.SaveChanges();
            }
        }
    }
}