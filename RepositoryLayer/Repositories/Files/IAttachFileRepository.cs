using Domain.Interfaces;
using IdylAPI.Models.Img;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace IdylAPI.Services.Interfaces.Repository.Files
{
    public interface IAttachFileRepository : IRepository<AttachFileObject>
    {
        IEnumerable<AttachFileObject> GetInsepctionFileByLinkNo(int linkNo);
        IEnumerable<AttachFileObject> GetAttachFilesByLinkNo(int linkNo);
     

    }
}
