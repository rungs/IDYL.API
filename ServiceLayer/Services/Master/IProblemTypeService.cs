using IdylAPI.Models;
using IdylAPI.Models.Master;
using System.Collections.Generic;

namespace IdylAPI.Services.Interfaces.Master
{
    public interface IProblemTypeService
    {
        Result Retrive(WhereParameter whereParameter);
    }
}
