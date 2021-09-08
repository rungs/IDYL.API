using Domain.Interfaces;
using IdylAPI.Models;

using IdylAPI.Models.Syst;
using IdylAPI.Services.Interfaces;
using IdylAPI.Services.Interfaces.Service.Specification;
using IdylAPI.Services.Interfaces.Syst;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SocialMedia.Core.Services
{
    public class PMResourceService : IPMResourceService
    {
        private readonly IUnitOfWork _unitOfWork;

        public PMResourceService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

      
        public void  GetPMResourcePivotMTRequirement()
        {
            //return _unitOfWork.PMResourceRepository.GetPMResourcePivotMTRequirement();
        }
    }
}
