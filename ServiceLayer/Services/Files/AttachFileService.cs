using Domain.Interfaces;
using IdylAPI.Models.Img;
using IdylAPI.Services.Interfaces.Service.Files;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SocialMedia.Core.Services
{
    public class AttachFileService : IAttachFileService
    {
        private readonly IUnitOfWork _unitOfWork;

        public AttachFileService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task AddFile(AttachFileObject attachFileObject)
        {
            AttachFileObject @object = new AttachFileObject()
            {
                FileName = attachFileObject.FileName,
                Path = attachFileObject.Path,
                FileType = attachFileObject.FileType,
                Extension = attachFileObject.Extension,
                LinkNo = attachFileObject.LinkNo,
                CompanyNo = attachFileObject.CompanyNo,
                IsUrl = false,
                IsInspectionFile = false,
                IsDelete = false,
                IsActive = true,
                CreatedBy = 1,
                CreatedDate = DateTime.Now
            };
            await _unitOfWork.AttachFileRepository.Add(@object);
            await _unitOfWork.SaveChangesAsync();
        }

        public async Task DeleteFile(int attachNo)
        {
            await _unitOfWork.AttachFileRepository.Delete(attachNo);
            await _unitOfWork.SaveChangesAsync();
        }

        public IEnumerable<AttachFileObject> GetAttachFilesByLinkNo(int linkNo)
        {
            return _unitOfWork.AttachFileRepository.GetAttachFilesByLinkNo(linkNo);
        }

        public IEnumerable<AttachFileObject> GetInsepctionFileByLinkNo(int linkNo)
        {
            return _unitOfWork.AttachFileRepository.GetInsepctionFileByLinkNo(linkNo);
        }
    }
}
