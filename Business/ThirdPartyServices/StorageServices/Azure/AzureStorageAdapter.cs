using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using Core.Utilities.Results;
using Entities.Dtos;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.ThirdPartyServices.StorageServices.Azure
{
    public class AzureStorageAdapter:IStorageService
    {
        readonly BlobServiceClient _blobServiceClient;
        BlobContainerClient _blobContainerClient;
        private readonly string uri = "https://anysurvey.blob.core.windows.net";
        public AzureStorageAdapter(IConfiguration configuration)
        {
            _blobServiceClient = new(configuration["Storage:Azure"]);
        }

        public async Task DeleteAsync(string containerName, string fileName)
        {
            _blobContainerClient = _blobServiceClient.GetBlobContainerClient(containerName);
            BlobClient blobClient = _blobContainerClient.GetBlobClient(fileName);
            await blobClient.DeleteAsync();
        }

        public List<string> GetFiles(string containerName)
        {
            _blobContainerClient = _blobServiceClient.GetBlobContainerClient(containerName);
            return _blobContainerClient.GetBlobs().Select(b => uri + "/" +containerName+ "/"+  b.Name).ToList();
        }

        public bool HasFile(string containerName, string fileName)
        {
            var files =  GetFiles(containerName);
            if (files.Contains(uri + "/" + containerName + "/" + fileName))
            {
                return true;
            }
            return false;
        }
        public async Task<List<FileUploadResponseDto>> UploadAsync(string containerName,string id , IFormFileCollection files)
        {
            _blobContainerClient = _blobServiceClient.GetBlobContainerClient(containerName);
            await _blobContainerClient.CreateIfNotExistsAsync();
            await _blobContainerClient.SetAccessPolicyAsync(PublicAccessType.BlobContainer);

            List<FileUploadResponseDto> datas = new();
            foreach (IFormFile file in files)
            {
                string fileNewName = id+"-"+file.FileName;
                BlobClient blobClient = _blobContainerClient.GetBlobClient(fileNewName);
                await blobClient.UploadAsync(file.OpenReadStream());
                FileUploadResponseDto fileUploadResponseDto = new()
                {
                    FileName= fileNewName,
                    PathOrContainerName = $"{uri}/{containerName}/{fileNewName}"
                };
                
                datas.Add(fileUploadResponseDto);
            }
            return datas;
        }

        public async Task<FileUploadResponseDto> UploadSingleFileAsync(string containerName, string id, IFormFile file)
        {
            _blobContainerClient = _blobServiceClient.GetBlobContainerClient(containerName);
            await _blobContainerClient.CreateIfNotExistsAsync();
            await _blobContainerClient.SetAccessPolicyAsync(PublicAccessType.BlobContainer);

            string fileNewName = id + "-" + file.FileName;
            BlobClient blobClient = _blobContainerClient.GetBlobClient(fileNewName);
            await blobClient.UploadAsync(file.OpenReadStream());
            FileUploadResponseDto fileUploadResponseDto = new()
            {
                FileName = fileNewName,
                PathOrContainerName = $"{uri}/{containerName}/{fileNewName}"
            };
            
            return fileUploadResponseDto;
        }
    }
}
