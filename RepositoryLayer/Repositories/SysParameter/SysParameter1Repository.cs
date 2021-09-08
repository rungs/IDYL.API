using IdylAPI.Models;
using IdylAPI.Models.Syst;
using IdylAPI.Services.Interfaces.Syst;
using Microsoft.EntityFrameworkCore;
using Persistence.Contexts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IdylAPI.Services.Repository.Company
{
    public class SysParameter1Repository : BaseRepositoryV2<SysParameter1>, ISysParameter1Repository
    {

        protected readonly AppDBContext _context;
        public SysParameter1Repository(AppDBContext context) : base(context) {
            _context = context;
        }

        public async Task GenCodeFormat(int companyNo, string paraNo)
        {
            DateTime now = DateTime.Now;
            string yearStr = now.Year.ToString();
            string monthStr = "100" + now.Month.ToString();
            SysParameter1 sysParameter1 = await _entities.Where(x => x.CompanyNo == companyNo && x.PARANO == paraNo).FirstOrDefaultAsync();


            string currentYear = sysParameter1.UseYear == "T"? yearStr.Substring(yearStr.Length - 2, 2) : "";
            string currentMonth = sysParameter1.UseMonth == "T" ? monthStr.Substring(monthStr.Length - 2, 2) : "";
            int lastRunNo = sysParameter1.LAST_RUNNO;
            if (sysParameter1.IsDocCodeRunning == "T")
            {
                if (sysParameter1.YearNo != currentYear)
                {
                    lastRunNo = 0;
                }
                string runNo = "1000000000" + lastRunNo;
                string runNoStr = (Convert.ToInt64(runNo) + 1).ToString();
                string docLastNo = $"{sysParameter1.PREFIX}{currentYear}{currentMonth}{sysParameter1.SeparetChar}{runNoStr.Substring(runNoStr.Length - sysParameter1.ORDINAL_RUNNO, sysParameter1.ORDINAL_RUNNO)}";

                sysParameter1.Doc_LastNo = docLastNo;
                sysParameter1.LAST_RUNNO = sysParameter1.LAST_RUNNO + 1;
                sysParameter1.YearNo = currentYear;
                _entities.Update(sysParameter1);
            }
        }

        public async Task<IEnumerable<SysParameter1>> GetSysParameterByCompany(int companyNo)
        {
            return await _entities.Where(x => x.CompanyNo == companyNo).ToListAsync();
        }

        public async Task<SysParameter1> GetSysParameterByParaAndCompany(int companyNo, string paraNo)
        {
            return await _entities.Where(x => x.CompanyNo == companyNo && x.PARANO == paraNo).FirstOrDefaultAsync();
        }
    }
}
