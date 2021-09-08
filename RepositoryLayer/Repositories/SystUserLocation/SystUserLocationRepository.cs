using Domain.Entities.Syst;
using IdylAPI.Services.Interfaces.Syst;
using Persistence.Contexts;

namespace IdylAPI.Services.Repository.Syst
{
    public class SystUserLocationRepository : BaseRepositoryV2<SystUserLocation>, ISystUserLocationRepository
    {
        public SystUserLocationRepository(AppDBContext context) : base(context) { }

    }
}
