using Domain.Entities.Syst;
using Domain.Interfaces.Repositories.Syst;
using Persistence.Contexts;
namespace IdylAPI.Services.Repository.Syst
{
    public class LoginHistoryRepository : BaseRepositoryV2<LogInHistory>, ILoginHistoryRepository
    {
        public LoginHistoryRepository(AppDBContext context) : base(context) {
        
        }
    }
}
