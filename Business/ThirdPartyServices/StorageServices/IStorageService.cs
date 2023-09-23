using Entities.Dtos;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Business.ThirdPartyServices.StorageServices
{
    public interface IStorageService
    {
        public Task<List<FileUploadResponseDto>> UploadAsync(string containerName, string id, IFormFileCollection files);
    }
}
