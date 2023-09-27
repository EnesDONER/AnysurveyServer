using Core.Utilities.Results;
using Entities.Concrete;
using Entities.Dtos;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Abstract
{
    public interface IAdService
    {
        IResult Add(Ad ad);
        Task<IResult> AddAdandUploadAsync(AdUploadDto adUploadDto);
        IDataResult<List<Ad>> GetAllFilteredAdByUserId(int userId);
        IDataResult<Ad> GetById(string id);
        IDataResult<List<UserForWatchedOrSolvedContent>> GetAllUsersWhoWatchedAdsByAdId(string adId);
        IDataResult<List<Ad>> GetAllAdsByOwnerUserId(int userId);
        IDataResult<List<Ad>> GetAllUnWatchedAd(int userId);

        IResult AddWatchedAd(WatchedAd watcehedAd);
        IDataResult<List<WatchedAd>> GetAllWatchedAd();
        IDataResult<List<WatchedAd>> GetAllWatchedAdByUserId(int userId);


        Task<List<FileUploadResponseDto>> Upload(string containerName,string id, IFormFileCollection files);
        IDataResult<List<string>> GetAllFile(string containerName);
        IResult HasFile(string containerName, string fileName);

    }
}
