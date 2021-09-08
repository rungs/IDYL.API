using Domain.Interfaces;
using IdylAPI.Models;
using IdylAPI.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Persistence.Contexts;
using Microsoft.Extensions.Configuration;

namespace IdylAPI.Services.Repository
{
    public class BaseRepositoryV2<T> : IRepository<T> where T : BaseEntity 
    {
        private readonly AppDBContext _context;
        protected readonly DbSet<T> _entities;
     
        public BaseRepositoryV2(AppDBContext context)
        {
            _context = context;
            _entities = context.Set<T>();
        }

        public IEnumerable<T> GetAll()
        {
            return _entities.AsEnumerable();
        }

        public async Task<T> GetById(int id)
        {
            return await _entities.FindAsync(id);
        }

        public async Task Add(T entity)
        {
            await _entities.AddAsync(entity);
        }

        public void Update(T entity)
        {
            _entities.Update(entity);
        }

        public async Task Delete(int id)
        {
            T entity = await GetById(id);
            _entities.Remove(entity);
        }
    }
}
