using IdylAPI.Models;
using IdylAPI.Models.Specification;
using IdylAPI.Services.Interfaces.Repository.Specification;
using Microsoft.EntityFrameworkCore;
using Persistence.Contexts;
using System.Collections.Generic;
using System.Linq;

namespace IdylAPI.Services.Repository.Master
{
    public class SpecRepository : BaseRepositoryV2<Spec>, ISpecRepository
    {
        protected readonly AppDBContext _context;
        public SpecRepository(AppDBContext context) : base(context)
        {
            _context = context;
        }

        public Spec GetByCode(string code, int companyNo)
        {
            return _entities.Where(x => x.CompanyNo == companyNo && x.SpecCode == code).FirstOrDefault();
        }
        public IEnumerable<Spec> GetByCompany(int companyNo, bool isDelete)
        {
            if (isDelete)
            {
                return _entities.Where(x => x.CompanyNo == companyNo).AsEnumerable();
            }
            else
            {
                return _entities.Where(x => x.CompanyNo == companyNo && !x.IsDelete).AsEnumerable();
            }
        }
        public IEnumerable<Spec> ExcludeEqType(int eqTypeNo)
        {
            var query = from spec in _context.Specification
                        join eqtype_spec in _context.EQTypeSpec on spec.SpecNo equals eqtype_spec.SpecNo into gj
                        from x in gj.DefaultIfEmpty()
                        where x.EQTypeNo != eqTypeNo && spec.IsDelete == false &&  !(from o in _context.EQTypeSpec
                                                                                where o.EQTypeNo == eqTypeNo
                                                                                     select o.SpecNo).Contains(spec.SpecNo)
                        select spec;
            return query.GroupBy(d => new { d.SpecNo, d.SpecCode, d.SpecName, d.Unit, d.ValueType, d.Remark })
                    .Select(g => new Spec()
                    {
                        SpecNo = g.Key.SpecNo,
                        SpecCode = g.Key.SpecCode,
                        SpecName = g.Key.SpecName,
                        ValueType = g.Key.ValueType,
                        Remark = g.Key.Remark,
                        Unit = g.Key.Unit
                    }).ToList();
        }
        public List<Spec> ExcludeEq(int eqNo, int eqTypeNo)
        {
            var query = from spec in _context.Specification
                        join eqtypespec in _context.EQTypeSpec on spec.SpecNo equals eqtypespec.SpecNo into eqtype_spec
                        from eqtypespec in eqtype_spec.DefaultIfEmpty()

                        join eq in _context.EQ on eqtypespec.EQTypeNo equals eq.EQType into eq_spec
                        from eq in eq_spec.DefaultIfEmpty()

                        where spec.IsDelete == false && eq.EQType == eqTypeNo && !(from o in _context.EQSpec
                                                                                   where o.EQNo == eqNo
                                                                                   select o.SpecNo).Contains(spec.SpecNo)

                        select spec;
            return query.GroupBy(d => new { d.SpecNo, d.SpecCode, d.SpecName, d.Unit, d.ValueType, d.Remark })
                    .Select(g => new Spec()
                    {
                        SpecNo = g.Key.SpecNo,
                        SpecCode = g.Key.SpecCode,
                        SpecName = g.Key.SpecName,
                        ValueType = g.Key.ValueType,
                        Remark = g.Key.Remark,
                        Unit = g.Key.Unit
                    }).ToList();

        }

        public void UpdateIsUse(int specNo)
        {
            var query = from eqspec in _context.EQTypeSpec
                        where eqspec.SpecNo == specNo
                        select eqspec;

            var q = _entities.Where(x => x.SpecNo == specNo).FirstOrDefault();
            q.isUse = query.Count() > 0;
            _entities.Update(q);
        }
    }
}
