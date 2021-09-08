using Domain.Interfaces;
using IdylAPI.Models.Master;
using Persistence.Contexts;

namespace IdylAPI.Services.Repository.Company
{
    public class Section2Repository : BaseRepositoryV2<Section>, ISection2Repository
    {
        public Section2Repository(AppDBContext context) : base(context) { }
    }
}
