using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IdylAPI.Models.WO
{
    public static class InitFilterMaster
    {
        public static List<Filter> CreateWoType(IEnumerable<WO> wOs)
        {
            return wOs.GroupBy(c => new
            {
                c.WOTypeNo,
                c.WOTypeCode,
            })
               .Select(gcs => new Filter()
               {
                   id = gcs.Key.WOTypeNo.ToString(),
                   text = gcs.Key.WOTypeCode,
                   filtertype = "wotype"
               }).OrderBy(o => o.text).Where(w => w.text != null).ToList();
        }

        public static List<Filter> CreateLocation(IEnumerable<WO> wOs)
        {
            return wOs.GroupBy(c => new
            {
                c.LocationNo,
                c.LocationName,
            })
               .Select(gcs => new Filter()
               {
                   id = gcs.Key.LocationNo.ToString(),
                   text = gcs.Key.LocationName,
                   filtertype = "location"
               }).OrderBy(o => o.text).Where(w => w.text != null).ToList();
        }

        public static List<Filter> CreateEQ(IEnumerable<WO> wOs)
        {
            return wOs.GroupBy(c => new
            {
                c.EQNo,
                c.EQName,
            })
               .Select(gcs => new Filter()
               {
                   id = gcs.Key.EQNo.ToString(),
                   text = gcs.Key.EQName,
                   filtertype = "eq"
               }).OrderBy(o => o.text).Where(w => w.text != null).ToList();
        }

        public static List<Filter> CreatePlnDate()
        {
            List<Filter> filters = new List<Filter>();
            Filter filter = new Filter()
            {
                id = "today",
                text = "วันนี้",
                filtertype = "plndate",
            };
            filters.Add(filter);

            filter = new Filter()
            {
                id = "tomorrow",
                text = "วันพรุ่งนี้",
                filtertype = "plndate",
            };
            filters.Add(filter);

            filter = new Filter()
            {
                id = "custom",
                text = "เลือกปฏิทิน",
                filtertype = "plndate",
            };
            filters.Add(filter);

            return filters;
        }
    }
}
