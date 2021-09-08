using Domain.Interfaces.Repositories.PM;
using Domain.Interfaces.Repositories.Syst;
using IdylAPI.Repositories.PM;
using IdylAPI.Services.Interfaces.Master;
using IdylAPI.Services.Interfaces.Repository.Files;
using IdylAPI.Services.Interfaces.Repository.Specification;
using IdylAPI.Services.Interfaces.Syst;
using IdylAPI.Services.Repository.Company;
using IdylAPI.Services.Repository.Master;
using IdylAPI.Services.Repository.Syst;
using System;
using System.Threading.Tasks;

namespace Domain.Interfaces
{
    public interface IUnitOfWork : IDisposable
    {
        ICompanyRepository CompanyRepository { get; }
        ISysParameter1Repository SysParameter1Repository { get; }
        ISectionRepository SectionRepository { get; }
        ISystUserRepository SystUserRepository { get; }
        ICustomerRepository CustomerRepository { get; }
        ISystPermissionsDataRepository SystPermissionsDataRepository { get; }
        ISystPermissionsActionRepository SystPermissionsActionRepository { get; }
        ISystPermissionsRepository SystPermissionsRepository { get; }
        ISystMenuRepository SystMenuRepository { get; }
        ISystActionRepository SystActionRepository { get; }
        ISystUserCustomerRepository SystUserCustomerRepository { get; }
        ISystConfigRepository SystConfigRepository { get; }
        ISystPermissionsActionCompanyRepository SystPermissionsActionCompanyRepository { get; }
        INotifyMsgRepository NotifyMsgRepository { get; }
        INotifyMsgRoleRepository NotifyMsgRoleRepository { get; }
        INotifyMsgDefaultRepository NotifyMsgDefaultRepository { get; }
        IUserGroupMenuRepository UserGroupMenuRepository { get; }
        IUserGroupRepository UserGroupRepository { get; }
        ISpecRepository SpecRepository { get; }
        IEQTypeSpecRepository EQTypeSpecRepository { get; }
        IEQSpecRepository EQSpecRepository { get; }
        IAttachFileRepository AttachFileRepository { get; }
        IPMResourceRepository PMResourceRepository { get; }
        ILocationRepository LocationRepository { get; }
        ISystUserLocationRepository SystUserLocationRepository { get; }
        ICraftTypeRepository CraftTypeRepository { get; }
        ILoginHistoryRepository LoginHistoryRepository { get; }
        IPMRepository PMRepository { get; }
        IProblemTypeRepository ProblemTypeRepository { get; }
        IFreqUnitRespository FreqUnitRepository { get; }
        IFailureObjectRepository FailureObjectRepository { get; }
        void SaveChanges();
        Task SaveChangesAsync();
        void BeginTransaction();
        void CommitTransaction();
        void RollbackTransaction();

    }
}
