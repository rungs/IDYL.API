using Domain.Interfaces;
using IdylAPI.Models.Master;
using IdylAPI.Models.Syst;
using IdylAPI.Services.Interfaces;
using IdylAPI.Services.Interfaces.Company;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SocialMedia.Core.Services
{
    public class SysParameter1Service : ISysParameter1Service
    {
        private readonly IUnitOfWork _unitOfWork;
     

        public SysParameter1Service(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<IEnumerable<SysParameter1>> GetSysParameterByCompany(int companyNo)
        {
           return  await _unitOfWork.SysParameter1Repository.GetSysParameterByCompany(companyNo);
        }
    }
}
