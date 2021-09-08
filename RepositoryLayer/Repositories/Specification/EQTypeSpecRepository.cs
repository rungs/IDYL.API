using IdylAPI.Models;
using IdylAPI.Models.Specification;
using IdylAPI.Services.Interfaces.Repository.Specification;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using Persistence.Contexts;

namespace IdylAPI.Services.Repository.Master
{
    public class EQTypeSpecRepository : BaseRepositoryV2<EqTypeSpec>, IEQTypeSpecRepository
    {
       
        public EQTypeSpecRepository(AppDBContext context) : base(context) {
          
        }

        public IEnumerable<EqTypeSpec> GetByEQType(int eqTypeNo)
        {
            return _entities.Where(x=>x.EQTypeNo == eqTypeNo).Include(i => i.Specification);

        }
        public void Delete(int eqTypeNo, int specNo)
        {
            _entities.Remove(_entities.Where(x => x.EQTypeNo == eqTypeNo && x.SpecNo == specNo).FirstOrDefault());
        }
       
    }
}
