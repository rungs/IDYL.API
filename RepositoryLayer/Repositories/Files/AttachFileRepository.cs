using IdylAPI.Models;
using IdylAPI.Models.Img;
using IdylAPI.Services.Interfaces.Repository.Files;
using Persistence.Contexts;
using System.Collections.Generic;
using System.Linq;

namespace IdylAPI.Services.Repository.Master
{
    public class AttachFileRepository : BaseRepositoryV2<AttachFileObject>, IAttachFileRepository
    {
        public AttachFileRepository(AppDBContext context) : base(context)
        { 
        }

        public IEnumerable<AttachFileObject> GetAttachFilesByLinkNo(int linkNo)
        {
            return _entities.Where(t => t.LinkNo == linkNo);
        }

        public IEnumerable<AttachFileObject> GetInsepctionFileByLinkNo(int linkNo)
        {
            return _entities.Where(t => t.IsInspectionFile == true && t.LinkNo == linkNo && t.FileType == "WO") ;
        }
    }
}
