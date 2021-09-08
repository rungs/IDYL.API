using IdylAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IdylAPI.Helper
{
    public class FilterHelper
    {
        public static void AddFilter(ref List<GroupFilter> groupFilters, List<Filter> filters, string groupName, string groupNameTH)
        {
            GroupFilter groupFilter = new GroupFilter()
            {
                groupName = groupName,
                groupNameTH = groupNameTH,
                filters = filters
            };
            groupFilters.Add(groupFilter);

        }
    }
}
