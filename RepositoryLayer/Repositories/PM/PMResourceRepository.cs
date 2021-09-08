using Dapper;
using Domain.Entities.PM;
using Domain.Interfaces;
using Persistence.Contexts;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Threading.Tasks;

namespace IdylAPI.Services.Repository.Company
{
    public class PMResourceRepository : BaseRepositoryV2<PMResource> , IPMResourceRepository
    {
        private readonly AppDBContext _context;
        public PMResourceRepository(AppDBContext context) : base(context)
        {
            _context = context;
        }

        public async Task<IEnumerable<dynamic>> GetPMResourcePivotMTRequirement()
        {
            using (SqlConnection conn
                     = new SqlConnection(_context.Connection.ConnectionString))
            {
                var result = await conn.QueryAsync(@"SELECT PMResource.RescNo AS [No], 
                     DATEPART(MONTH, CAST(PMSched.DUEDATE AS DATETIME)) AS[Month_Qty],
                   PMResource.QtyMH,
                   PMResource.Amount,
                   PMResource.ResTypeCode,
                   Resource.RescCode,
                   Resource.RescName
            FROM PMSched
                 LEFT OUTER JOIN PM ON PMSched.PMNO = PM.PMNO
                 INNER JOIN PMResource ON PM.PMNo = PMResource.PMNo
                 INNER JOIN Resource ON PMResource.RescNo = Resource.RescNo
                 LEFT OUTER JOIN Section ON PM.SectionNo = Section.SectionNo
                 LEFT OUTER JOIN Company ON PM.CompanyNo = Company.CompanyNo
                 LEFT OUTER JOIN FreqUnit ON PM.FREQUNITNO = FreqUnit.FREQUNITNO
                 LEFT OUTER JOIN EQ ON EQ.EQNO = PM.EQNO
            WHERE PMResource.CompanyNo = 57 ");
                return result;
            }
        }
    }
}
