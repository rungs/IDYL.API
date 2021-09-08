using Domain.Entities.PM;
using Domain.Interfaces.Repositories.PM;
using IdylAPI.Models;
using IdylAPI.Services.Repository;
using Persistence.Contexts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IdylAPI.Repositories.PM
{
    public class FreqUnitRepository : BaseRepositoryV2<FreqUnit>, IFreqUnitRespository
    {
        protected readonly AppDBContext _context;
        public FreqUnitRepository(AppDBContext context) : base(context)
        {
         
        }
    }
}
