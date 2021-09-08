using Microsoft.EntityFrameworkCore.Infrastructure;
using System.Data;


namespace Domain.Interfaces
{
    public interface  IApplicationDbContext
    {
        public IDbConnection Connection { get; }
        DatabaseFacade Database { get; }
    }
}
