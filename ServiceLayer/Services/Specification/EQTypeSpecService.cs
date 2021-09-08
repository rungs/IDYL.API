using Domain.Interfaces;
using IdylAPI.Models;
using IdylAPI.Models.Specification;
using IdylAPI.Models.Syst;
using IdylAPI.Services.Interfaces;
using IdylAPI.Services.Interfaces.Service.Specification;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SocialMedia.Core.Services
{
    public class EQTypeSpecService : IEQTypeSpecService
    {
        private readonly IUnitOfWork _unitOfWork;

        public EQTypeSpecService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task Delete(int eqTypeNo, int specNo)
        {
            _unitOfWork.EQTypeSpecRepository.Delete(eqTypeNo, specNo);
            await _unitOfWork.SaveChangesAsync();
            _unitOfWork.SpecRepository.UpdateIsUse(specNo);
            await _unitOfWork.SaveChangesAsync();
        }

        public IEnumerable<EqTypeSpec> GetByEQType(int eqTypeNo)
        {
            return _unitOfWork.EQTypeSpecRepository.GetByEQType(eqTypeNo);
        }

        public async Task<EqTypeSpec> Insert(EqTypeSpec eqTypeSpec)
        {
            DateTime dateTime = DateTime.Now;
            eqTypeSpec.CreatedDate = dateTime;
            eqTypeSpec.UpdatedDate = dateTime;
            await _unitOfWork.EQTypeSpecRepository.Add(eqTypeSpec);
            await _unitOfWork.SaveChangesAsync();
            _unitOfWork.SpecRepository.UpdateIsUse(eqTypeSpec.SpecNo);
            await _unitOfWork.SaveChangesAsync();
            return eqTypeSpec;
        }
    }
}
