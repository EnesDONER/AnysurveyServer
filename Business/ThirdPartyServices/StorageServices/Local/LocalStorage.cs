using Entities.Dtos;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.ThirdPartyServices.StorageServices.Local
{
    public class LocalStorage : IStorageService
    {
        public async Task<List<FileUploadResponseDto>> UploadAsync(string containerName, string id, IFormFileCollection files)
        {
             FileUploadResponseDto fileUploadResponseDto = new FileUploadResponseDto
            {
                FileName = "Local",
                PathOrContainerName = "Local"
            };
            List < FileUploadResponseDto> fileUploadResponseDtos = new List<FileUploadResponseDto>();
            fileUploadResponseDtos.Add(fileUploadResponseDto);
            return fileUploadResponseDtos;
        }
    }
}
