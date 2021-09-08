using Domain.Interfaces;
using IdylAPI.Models;
using IdylAPI.Models.Authorize;
using IdylAPI.Models.Master;
using IdylAPI.Services.Interfaces.Master;
using IdylAPI.Services.Interfaces.Syst;
using System.Collections.Generic;

namespace IdylAPI.Services.Master
{
    public class ProblemTypeService : IProblemTypeService
    {
        private readonly IUnitOfWork _unitOfWork;

        public ProblemTypeService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public Result Retrive(WhereParameter whereParameter)
        {
             return _unitOfWork.SectionRepository.Retrive(whereParameter);
        }
    }
}
