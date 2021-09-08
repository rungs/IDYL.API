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
    public class EQSpecService : IEQSpecService
    {
        private readonly IUnitOfWork _unitOfWork;

        public EQSpecService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task CopySpec(int eqNo, int eqTypeNo, int userNo)
        {
            await _unitOfWork.EQSpecRepository.CopySpec(eqNo, eqTypeNo, userNo);
            await _unitOfWork.SaveChangesAsync();
        }

        public async Task Delete(int eqNo, int specNo)
        {
            _unitOfWork.EQSpecRepository.Delete(eqNo, specNo);
            await _unitOfWork.SaveChangesAsync();
        }

        public IEnumerable<EQSpec> GetByEq(int eqNo)
        {
            return _unitOfWork.EQSpecRepository.GetByEq(eqNo);
        }

        public string GetEQSpecAll(int companyNo)
        {
            return _unitOfWork.EQSpecRepository.GetEQSpecAll(companyNo);
        }

        public async Task<EQSpec> Insert(EQSpec eqSpec)
        {
            DateTime dateTime = DateTime.Now;
            eqSpec.CreatedDate = dateTime;
            eqSpec.UpdatedDate = dateTime;
            await _unitOfWork.EQSpecRepository.Add(eqSpec);
            await _unitOfWork.SaveChangesAsync();
            return eqSpec;
        }

        public async Task UpdateValue(EQSpec eQSpec)
        {
             _unitOfWork.EQSpecRepository.UpdateValue(eQSpec);
            await _unitOfWork.SaveChangesAsync();
        }
    }
}
