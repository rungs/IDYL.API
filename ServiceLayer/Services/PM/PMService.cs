using Domain.Entities.PM;
using Domain.Interfaces;
using IdylAPI.Models;
using IdylAPI.Models.Authorize;
using IdylAPI.Models.Syst;
using IdylAPI.Services.Interfaces.Syst;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SocialMedia.Core.Services
{
    public class PMService : IPMService
    {
        private readonly IUnitOfWork _unitOfWork;

        public PMService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public Result GetPmAllByEq(WhereParameter whereParameter)
        {
            return _unitOfWork.PMRepository.GetPmAllByEq(whereParameter);
        }

        public PM GetPmByCode(string code, int companyNo)
        {
            return _unitOfWork.PMRepository.GetPmByCode(code, companyNo);
        }
     
        public PM GetPmById(int pmNo)
        {
            return _unitOfWork.PMRepository.GetPmById(pmNo);
        }

        public async Task<Result> Update(PM pM, User user)
        {
            pM.UpdatedBy = user.CustomerNo;
            return _unitOfWork.PMRepository.UpdatePm(pM, user);
            //    Result result = new Result();
            //    if (string.IsNullOrEmpty(pM.Pmcode))
            //    {
            //        await _unitOfWork.SysParameter1Repository.GenCodeFormat(0, "SYS2270");
            //        SysParameter1 sysParameter1 = await _unitOfWork.SysParameter1Repository.GetSysParameterByParaAndCompany(0, "SYS2270");
            //        pM.Pmcode = sysParameter1.Doc_LastNo;
            //    }
            //    PM pMOld = _unitOfWork.PMRepository.GetPmByCode(pM.Pmcode, pM.CompanyNo);
            //    if (pMOld != null)
            //    {
            //        result.ErrMsg = "ไม่สามารถทำการบันทึกได้ เนื่องจากรหัสซ้ำ";
            //        //return 
            //    }
            //    if (pM.Pmno != 0)
            //    {

            //    }
            //    else
            //    {
            //        pM.CraftNo = 1;
            //        pM.EqhistoryNo = pM.Eqno;
            //        pM.LocationHisNo = pM.LocationNo;
            //        pM.SchTypeNo = 1;
            //        pM.DueMethodNo = 1;
            //        pM.FlagJobPlan = false;
            //        pM.CreatedDate = System.DateTime.Now;
            //        pM.Eqdown = false;
            //        pM.PlantDown  = false;
            //        await _unitOfWork.PMRepository.Add(pM);
            //         _unitOfWork.SaveChanges();


            //    }
            //    return result;
            //    // throw new System.NotImplementedException();
            }
        }
}
