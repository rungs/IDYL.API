using Domain.Entities.PM;
using IdylAPI.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace IdylAPI.Services.Interfaces.Syst
{
    public interface IFreqUnitService
    {
        IEnumerable<FreqUnit> GetAll();
    }
}
