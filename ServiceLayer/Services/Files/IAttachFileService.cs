using IdylAPI.Models;
using IdylAPI.Models.Img;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace IdylAPI.Services.Interfaces.Service.Files
{
    public interface IAttachFileService
    {
        IEnumerable<AttachFileObject> GetInsepctionFileByLinkNo(int linkNo);
        Task AddFile(AttachFileObject attachFileObject);
        Task DeleteFile(int attachNo);
        IEnumerable<AttachFileObject> GetAttachFilesByLinkNo(int linkNo);
    }
}
