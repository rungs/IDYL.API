using Domain.Interfaces;
using IdylAPI.Models;
using IdylAPI.Models.Authorize;
using IdylAPI.Models.Master;
using IdylAPI.Services.Interfaces.Syst;
using System.Collections.Generic;

namespace IdylAPI.Services.Master
{
    public class SectionService : ISectionService
    {
        private readonly IUnitOfWork _unitOfWork;

        public SectionService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public IEnumerable<Section> GetByCompany(int companyNo)
        {
            return _unitOfWork.SectionRepository.GetByCompany(companyNo);
        }

        public Result Retrive(WhereParameter whereParameter)
        {
             return _unitOfWork.SectionRepository.Retrive(whereParameter);
        }
    }
}
