using Domain.Interfaces;
using IdylAPI.Models;
using Microsoft.EntityFrameworkCore;
using Persistence.Contexts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;



namespace RepositoryLayer.Repository
{
    public class Repository<T> : IRepository<T> where T : BaseEntity
    {
        private readonly AppDBContext _context;
        protected readonly DbSet<T> _entities;

        public Repository(AppDBContext context)
        {
            _context = context;
            _entities = _context.Set<T>();
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
           if(_entities == null)
            {
                throw new ArgumentNullException("entity");
            }
            _entities.Add(entity);
            await _context.SaveChangesAsync();
        }

        public async Task Delete(int id)
        {
            if (_entities == null)
            {
                throw new ArgumentNullException("entity");
            }
            T entity = await GetById(id);
            _entities.Remove(entity);
            await _context.SaveChangesAsync();
        }

        public async Task SaveChanges()
        {
            await _context.SaveChangesAsync();
        }

        public async Task Update(T entity)
        {
            if (_entities == null)
            {
                throw new ArgumentNullException("entity");
            }
           
            _entities.Update(entity);
            await _context.SaveChangesAsync();
        }
    }
}
