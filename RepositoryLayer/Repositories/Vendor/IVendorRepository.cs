﻿using IdylAPI.Models;
using IdylAPI.Models.Authorize;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IdylAPI.Services.Interfaces.Master
{
    public interface IVendorRepository
    {
        Result Retrive(WhereParameter whereParameter);
    }
}
