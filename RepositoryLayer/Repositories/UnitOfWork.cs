using Domain.Interfaces;
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
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Persistence.Contexts;
using System.Threading.Tasks;

namespace IdylAPI.Services.Repository
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly AppDBContext _context;
        private readonly IConfiguration _configuration;
   
       
        private readonly ICompanyRepository _companyRepository;
        private readonly ISysParameter1Repository _sysParameter1Repository;
        private readonly ISectionRepository _sectionRepository;
        private readonly ISystUserRepository _systUserRepository;
        private readonly ISystPermissionsDataRepository _systPermissionsActionDataRepository;
        private readonly ISystPermissionsActionRepository _systPermissionsActionRepository;
        private readonly ISystPermissionsRepository _systPermissionsRepository;
        private readonly ISystUserCustomerRepository _systUserCustomerRepository;
        private readonly ISystConfigRepository _systConfigRepository;
        private readonly ISystPermissionsActionCompanyRepository _systPermissionsActionCompanyRepository;
        private readonly INotifyMsgRepository _notifyMsgRepository;
        private readonly INotifyMsgRoleRepository _notifyMsgRoleRepository;
        private readonly INotifyMsgDefaultRepository _notifyMsgDefaultRepository;
        private readonly ICustomerRepository _customerRepository;
        private readonly ISystMenuRepository _systMenuRepository;
        private readonly ISystActionRepository _systActionRepository;
        private readonly IUserGroupMenuRepository _userGroupMenuRepository;
        private readonly IUserGroupRepository _userGroupRepository;
        private readonly ISpecRepository _specRepository;
        private readonly IEQTypeSpecRepository _eqTypeSpecRepository;
        private readonly IEQSpecRepository _eqSpecRepository;
        private readonly IAttachFileRepository _attachFileRepository;
        private readonly IPMResourceRepository _pmResourceRepository;
        private readonly ILocationRepository _locationRepository;
        private readonly ISystUserLocationRepository _systUserLocationRepository;
        private readonly ICraftTypeRepository _craftTypeRepository;
        private readonly ILoginHistoryRepository _loginHistoryRepository;
        private readonly IPMRepository _pmRepository;
        private readonly IProblemTypeRepository _problemTypeRepository;
        private readonly IFreqUnitRespository _freqUnitRepository;
        private readonly IFailureObjectRepository _failureObjectRepository;
        public UnitOfWork(IConfiguration configuration, AppDBContext context)
        {
            _configuration = configuration;
            _context = context;
        }
        public ICompanyRepository CompanyRepository => _companyRepository ?? new CompanyRepository(_configuration, _context);
        public ISysParameter1Repository SysParameter1Repository => _sysParameter1Repository ?? new SysParameter1Repository(_context);
        public ISectionRepository SectionRepository => _sectionRepository ?? new SectionRepository(_context);
        public ISystUserRepository SystUserRepository => _systUserRepository ?? new SystUserRepository(_configuration,_context);
        public ISystPermissionsDataRepository SystPermissionsDataRepository => _systPermissionsActionDataRepository ?? new SystPermissionsDataRepository(_context);
        public ISystPermissionsActionRepository SystPermissionsActionRepository => _systPermissionsActionRepository ?? new SystPermissionsActionRepository(_context);
        public ISystPermissionsRepository SystPermissionsRepository => _systPermissionsRepository ?? new SystPermissionsRepository(_context);
        public ISystUserCustomerRepository SystUserCustomerRepository => _systUserCustomerRepository ?? new SystUserCustomerRepository(_context);
        public ISystConfigRepository SystConfigRepository => _systConfigRepository ?? new SystConfigRepository(_context);
        public ISystPermissionsActionCompanyRepository SystPermissionsActionCompanyRepository => _systPermissionsActionCompanyRepository ?? new SystPermissionsActionCompanyRepository(_context);
        public INotifyMsgRepository NotifyMsgRepository => _notifyMsgRepository ?? new NotifyMsgRepository(_context);
        public INotifyMsgRoleRepository NotifyMsgRoleRepository => _notifyMsgRoleRepository ?? new NotifyMsgRoleRepository(_context);
        public INotifyMsgDefaultRepository NotifyMsgDefaultRepository =>  _notifyMsgDefaultRepository ?? new NotifyMsgDefaultRepository(_context);
        public ICustomerRepository CustomerRepository => _customerRepository ?? new CustomerRepository(_context);
        public ISystMenuRepository SystMenuRepository => _systMenuRepository ?? new SystMenuRepository(_context);
        public ISystActionRepository SystActionRepository => _systActionRepository ?? new SystActionRepository(_context);
        public IUserGroupMenuRepository UserGroupMenuRepository => _userGroupMenuRepository ?? new UserGroupMenuRepository(_context);
        public IUserGroupRepository UserGroupRepository => _userGroupRepository ?? new UserGroupRepository(_context);
        public ISpecRepository SpecRepository => _specRepository ?? new SpecRepository(_context);
        public IEQTypeSpecRepository EQTypeSpecRepository => _eqTypeSpecRepository ?? new EQTypeSpecRepository(_context);
        public IEQSpecRepository EQSpecRepository => _eqSpecRepository ?? new EQSpecRepository(_context);
        public IAttachFileRepository AttachFileRepository => _attachFileRepository ?? new AttachFileRepository(_context);
        public IPMResourceRepository PMResourceRepository => _pmResourceRepository ?? new PMResourceRepository(_context);
        public ILocationRepository LocationRepository => _locationRepository ?? new LocationRepository(_context);
        public ISystUserLocationRepository SystUserLocationRepository => _systUserLocationRepository ?? new SystUserLocationRepository(_context);
        public ICraftTypeRepository CraftTypeRepository => _craftTypeRepository ?? new CraftTypeRepository(_context);
        public ILoginHistoryRepository LoginHistoryRepository => _loginHistoryRepository ?? new LoginHistoryRepository(_context);
        public IPMRepository PMRepository => _pmRepository ?? new PMRepository(_configuration,_context);
        public IProblemTypeRepository ProblemTypeRepository => _problemTypeRepository ?? new ProblemTypeRepository(_context, _configuration);
        public IFreqUnitRespository FreqUnitRepository => _freqUnitRepository ?? new FreqUnitRepository(_context);
        public IFailureObjectRepository FailureObjectRepository => _failureObjectRepository ?? new FailureObjectRepository(_configuration,_context);
       
        public void Dispose()
        {
            if (_context != null)
            {
                _context.Dispose();
            }
        }

        public void SaveChanges()
        {
            _context.SaveChanges();
        }

        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }

        public void BeginTransaction()
        {
            _context.Database.BeginTransaction();
        }

        public void CommitTransaction()
        {
            _context.Database.CommitTransaction();
        }

        public void RollbackTransaction()
        {
            _context.Database.RollbackTransaction();
        }

        public Microsoft.EntityFrameworkCore.Storage.IDbContextTransaction GetCurrentTransaction()
        {
            return _context.Database.CurrentTransaction;
        }

    
    }
}
