using IdylAPI.Models;
using IdylAPI.Models.Specification;
using IdylAPI.Services.Interfaces.Repository.Specification;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using PAUtility;
using Persistence.Contexts;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace IdylAPI.Services.Repository.Master
{
    public class EQSpecRepository : BaseRepositoryV2<EQSpec>, IEQSpecRepository
    {
        private readonly AppDBContext _context;
        public EQSpecRepository(AppDBContext context) : base(context)
        {
            _context = context;
        }

        public async Task CopySpec(int eqNo, int eqTypeNo, int userNo)
        {
            DateTime updatedDate = DateTime.Now;
            var eqTypeSpecList = _context.EQTypeSpec.Where(x => x.EQTypeNo == eqTypeNo).Select(o =>
            new EQSpec
            {
                EQNo = eqNo,
                SpecNo = o.SpecNo,
                CreatedBy = userNo,
                UpdatedBy = userNo,
                CreatedDate = updatedDate,
                UpdatedDate = updatedDate,
            }).ToList();

            _context.EQSpec.AddRange(eqTypeSpecList);
            await _context.SaveChangesAsync();
        }

        public IEnumerable<EQSpec> GetByEq(int eqNo)
        {
            return _entities.Where(x => x.EQNo == eqNo).Include(x => x.Specification);
        }

        public void Delete(int eqNo, int specNo)
        {
            _entities.Remove(_entities.Where(x => x.EQNo == eqNo && x.SpecNo == specNo).FirstOrDefault());
        }

        public void UpdateValue(EQSpec eQSpec)
        {
            EQSpec eq = _entities.Where(x => x.EQNo == eQSpec.EQNo && x.SpecNo == eQSpec.SpecNo).FirstOrDefault();
            eq.Value = eQSpec.Value;
            _entities.Update(eq);
        }

        public string GetEQSpecAll(int companyNo)
        {
            var query = from spec in _context.Specification
                        join eqspec in _context.EQSpec on spec.SpecNo equals eqspec.SpecNo
                        join eq in _context.EQ on eqspec.EQNo equals eq.EQNo

                        where eq.CompanyNo == companyNo && !eq.IsDelete
                        select eqspec;
            var result = query.Include(u => u.EQ).Include(u => u.Specification).Include(u => u.EQ.LocationObj).OrderBy(t => t.EQ.EQCode).OrderBy(t => t.Specification.SpecCode);

            DataTable mytable = new DataTable();
            mytable.Columns.Add("EQCode", typeof(string));
            mytable.Columns.Add("EQName", typeof(string));
            mytable.Columns.Add("Location", typeof(string));
            List<string> vs = new List<string>();
            foreach (var item in result)
            {
                string sepcName = item.Specification.SpecName;
                DataRow[] dataRows = mytable.Select("EQCode='" + item.EQ.EQCode + "'");
                DataRow dataRow;
                if (dataRows.Length > 0)
                {
                    dataRow = dataRows[0];
                    if (!vs.Contains(sepcName))
                    {
                        mytable.Columns.Add(sepcName, typeof(string));
                        vs.Add(sepcName);
                    }
                    dataRow["EQCode"] = item.EQ.EQCode;
                    dataRow["EQName"] = item.EQ.EQName;
                    dataRow["Location"] = item.EQ.LocationObj.LocationName;
                    dataRow[sepcName] = ConvertValueType(item);
                }
                else
                {
                    dataRow = mytable.NewRow();
                    if (!vs.Contains(sepcName))
                    {
                        mytable.Columns.Add(sepcName, typeof(string));
                        vs.Add(sepcName);
                    }
                    dataRow["EQCode"] = item.EQ.EQCode;
                    dataRow["EQName"] = item.EQ.EQName;
                    dataRow["Location"] = item.EQ.LocationObj.LocationName;
                    dataRow[sepcName] = ConvertValueType(item);
                    mytable.Rows.Add(dataRow);
                }
            }
            return JsonConvert.SerializeObject(mytable);
        }

        public string ConvertValueType(EQSpec eQSpec)
        {
            if (!string.IsNullOrEmpty(eQSpec.Value))
            {
                if (eQSpec.Specification.ValueType == "datetime")
                {
                    return  InputVal.ToDatetime(eQSpec.Value).ToString("dd/MM/yyyy HH:mm");
                }
                else if (eQSpec.Specification.ValueType == "true/false")
                {
                    return eQSpec.Value == "T" ? "True" : "False";
                }
                else
                {
                    return eQSpec.Value;
                }
            }
            return eQSpec.Value;
        }
    }
}
