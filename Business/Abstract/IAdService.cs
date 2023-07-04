using Core.Utilities.Results;
using Entities.Concrete;
using Entities.Dtos;
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
        IDataResult<List<Ad>> GetAllFilteredAdByUserId(int userId);
        IDataResult<Ad> GetById(string id);
        IDataResult<List<UserForWhoWatchedAds>> GetAllUsersWhoWatchedAdsByAdId(string adId);
        IDataResult<List<Ad>> GetAllAdsByOwnerUserId(int userId);
        IDataResult<List<Ad>> GetAllUnWatchedAd(int userId);
    }
}
