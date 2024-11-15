﻿using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SAE_APIGestion.Models.EntityFramework;

namespace SAE_APIGestion.Models.DataManger
{
    public class TypeSalleManager : IDataRepository<TypeSalle>
    {
        readonly GlobalDBContext? globalDBContext;

        public TypeSalleManager() { }

        public TypeSalleManager(GlobalDBContext context)
        {
            globalDBContext = context;
        }

        public async Task<ActionResult<IEnumerable<TypeSalle>>> GetAllAsync()
        {
            return globalDBContext.TypesSalles.ToList();
        }

        public async Task<ActionResult<TypeSalle>> GetByIdAsync(int id)
        {
            return globalDBContext.TypesSalles.FirstOrDefault(p => p.IdTypeSalle == id);
        }


        public async Task AddAsync(TypeSalle entity)
        {
            globalDBContext.TypesSalles.Add(entity);
            globalDBContext.SaveChanges();
        }

        public async Task UpdateAsync(TypeSalle TypeSalle, TypeSalle entity)
        {
            globalDBContext.Entry(TypeSalle).State = EntityState.Modified;
            TypeSalle.IdTypeSalle = entity.IdTypeSalle;
            TypeSalle.NomTypeSalle = entity.NomTypeSalle;
            TypeSalle.Description = entity.Description;
    

            globalDBContext.SaveChanges();
        }

        public async Task DeleteAsync(TypeSalle TypeSalle)
        {
            globalDBContext.TypesSalles.Remove(TypeSalle);
            globalDBContext.SaveChanges();
        }


    }
}