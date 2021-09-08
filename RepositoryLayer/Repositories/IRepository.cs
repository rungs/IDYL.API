using IdylAPI.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Interfaces
{
    public interface IRepository<T> where T : BaseEntity
    {
        IEnumerable<T> GetAll();
        Task<T> GetById(int id);
        Task Add(T entity);
        void Update(T entity);
        Task Delete(int id);
    }
}
