using Domain.Interfaces;
using IdylAPI.Models;

using IdylAPI.Models.Syst;
using IdylAPI.Services.Interfaces;
using IdylAPI.Services.Interfaces.Service.Specification;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SocialMedia.Core.Services
{
    public class SpecService : ISpecService
    {
        private readonly IUnitOfWork _unitOfWork;

        public SpecService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task DeleteSpec(int id, int updatedBy)
        {
            Spec Spec = await _unitOfWork.SpecRepository.GetById(id);
            Spec.IsDelete = true;
            Spec.UpdatedBy = updatedBy;
            Spec.UpdatedDate = DateTime.Now;
            _unitOfWork.SpecRepository.Update(Spec);
            await _unitOfWork.SaveChangesAsync();
        }

        public IEnumerable<Spec> ExcludeEqType(int eqTypeNo)
        {
            return _unitOfWork.SpecRepository.ExcludeEqType(eqTypeNo);
        }

        public IEnumerable<Spec> ExcludeEq(int eqNo, int eqTypeNo)
        {
            return _unitOfWork.SpecRepository.ExcludeEq(eqNo, eqTypeNo);
        }

        public IEnumerable<Spec> GetByCompany(int companyNo, bool isDelete)
        {
            return _unitOfWork.SpecRepository.GetByCompany(companyNo, isDelete);
        }

        public async Task<Result> InsertSpec(Spec Spec)
        {
            Result result = new Result();
            DateTime dateTime = DateTime.Now;
            try
            {
                _unitOfWork.BeginTransaction();
                if(string.IsNullOrEmpty(Spec.SpecCode))
                {
                    await _unitOfWork.SysParameter1Repository.GenCodeFormat(0, "SYS2794");
                    SysParameter1 sysParameter1 = await _unitOfWork.SysParameter1Repository.GetSysParameterByParaAndCompany(0, "SYS2794");
                    Spec.SpecCode = sysParameter1.Doc_LastNo;
                }
                else
                {
                    Spec specOld =  _unitOfWork.SpecRepository.GetByCode(Spec.SpecCode, Spec.CompanyNo);
                    if(specOld != null && specOld.SpecNo != Spec.SpecNo)
                    {
                        result.StatusCode = 500;
                        result.ErrMsg = "duplicate";
                        return result;
                    }
                }
               
                if(Spec.SpecNo == 0)
                {
                    Spec.CreatedDate = dateTime;
                    Spec.IsActive = true;
                    Spec.IsDelete = false;
                    Spec.isUse = false;
                    await _unitOfWork.SpecRepository.Add(Spec);
                }
                else
                {
                    Spec specUpdate =  await _unitOfWork.SpecRepository.GetById(Spec.SpecNo);
                   
                    specUpdate.SpecCode = Spec.SpecCode;
                    specUpdate.SpecName = Spec.SpecName;
                    specUpdate.ValueType = Spec.ValueType;
                    specUpdate.Remark = Spec.Remark;
                    specUpdate.Unit = Spec.Unit;
                    specUpdate.IsDelete = Spec.IsDelete;
                    specUpdate.UpdatedBy = Spec.UpdatedBy;
                    specUpdate.UpdatedDate = dateTime;
                    _unitOfWork.SpecRepository.Update(specUpdate);
                 
                }
                await _unitOfWork.SaveChangesAsync();
                _unitOfWork.CommitTransaction();
                result.StatusCode = 200;
                result.Data = Spec;
                return result;
            }

            catch (Exception ex)
            {
                _unitOfWork.RollbackTransaction();
                result.StatusCode = 500;
                result.Data = ex.Message;
                return result;
               
                throw;
            }
        }

     
    }
}
