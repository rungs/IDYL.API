
using Domain.Interfaces;
using IdylAPI.Models;
using IdylAPI.Models.Authorize;
using IdylAPI.Models.Master;
using IdylAPI.Services.Interfaces.Syst;
using System.Collections.Generic;

namespace IdylAPI.Services.Master
{
    public class LocationService : ILocationService
    {
        private readonly IUnitOfWork _unitOfWork;

        public LocationService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public IEnumerable<Location> GetByCompany(int companyNo)
        {
            return _unitOfWork.LocationRepository.GetByCompany(companyNo);
        }

        //public PagedList<Location> GetLocations()
        //{
        //    return _unitOfWork.LocationRepository.GetLocations();
        //}

        public Result Insert(Location location, User user)
        {
            return _unitOfWork.LocationRepository.Insert(location, user);
        }

        public Result Retrive(WhereParameter whereParameter, User user)
        {
            return _unitOfWork.LocationRepository.Retrive(whereParameter, user);
        }
    }
}
