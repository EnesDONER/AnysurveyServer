using Azure;
using Business.Abstract;
using Business.BusinessAspects.Autofac;
using Core.Aspects.Autofac.Caching;
using Core.Entities.Concrete;
using Core.Utilities.Results;
using DataAccess.Abstract;
using DataAccess.Concrete.MongoDB;
using Entities.Concrete;
using Entities.Dtos;
using Microsoft.VisualBasic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Concrete
{
    public class AdManager : IAdService
    {
        private readonly IAdDal _addDal;
        private readonly IWatchedAdService _watchedAdService;
        private readonly IAdFilterService _adFilterService;
        private readonly IUserService _userService;

        public AdManager(IAdDal addDal, IWatchedAdService watchedAdService, IAdFilterService adFilterService, IUserService userService)
        {
            _addDal = addDal;
            _watchedAdService = watchedAdService;
            _adFilterService = adFilterService;
            _userService = userService;
        }

        [CacheRemoveAspect("IAdService.Get")]
        [SecuredOperation("partnership")]
        public IResult Add(Ad ad)
        {
            _addDal.Add(ad);
            return new SuccessResult();

        }
        public IDataResult<List<Ad>> GetAllFilteredAdByUserId(int userId)
        {
            var data = _addDal.GetAll();
            var result = ApplyFilter(data, userId);
            if (result.Success)
            {
                return new SuccessDataResult<List<Ad>>(result.Data);
            }
            return new ErrorDataResult<List<Ad>>();

        }
        [CacheAspect(10)]
        public IDataResult<List<UserForWhoWatchedAds>> GetAllUsersWhoWatchedAdsByAdId(string adId)
        {
            var ad = GetById(adId);
            if (!ad.Success)
            {
                return new ErrorDataResult<List<UserForWhoWatchedAds>>();
            }
            var watchedAds = _watchedAdService.GetAll().Data;
            List<UserForWhoWatchedAds> result = new List<UserForWhoWatchedAds>();
            foreach (var watchedAd in watchedAds)
            {
                if (ad.Data.Id == watchedAd.AdId)
                {
                    User user = _userService.GetById(watchedAd.UserId).Data;
                    UserForWhoWatchedAds userForWhoWatchedAds = new UserForWhoWatchedAds
                    {
                        Email = user.Email,
                        FirstName = user.FirstName,
                        LastName = user.LastName,
                    };
                    result.Add(userForWhoWatchedAds);
                }
            }

            return new SuccessDataResult<List<UserForWhoWatchedAds>>(result);
        }

        [CacheAspect(10)]
        public IDataResult<List<Ad>> GetAllUnWatchedAd(int userId)
        {
            return new SuccessDataResult<List<Ad>>(GetAllFilteredAdByUserId(userId).Data.Where(ad => !_watchedAdService.GetAllByUserId(userId).Data.Any(watchedAd => watchedAd.AdId == ad.Id)).ToList());
        }
        [CacheAspect(10)]
        public IDataResult<List<Ad>> GetAllAdsByOwnerUserId(int userId)
        {
            return new SuccessDataResult<List<Ad>>(_addDal.GetAll(a=>a.OwnerUserId==userId));

        }

        public IDataResult<Ad> GetById(string id)
        {
            return new SuccessDataResult<Ad>(_addDal.Get(a => a.Id == id));
        }
        private IDataResult<List<Ad>> ApplyFilter(List<Ad> ads, int userId)
        {
            List<Ad> inValidAds = new List<Ad>();
            foreach (var ad in ads)
            {
                var filter = _adFilterService.GetByAdId(ad.Id);
                if (filter.Success)
                {
                    var user = _userService.GetById(userId).Data;
                    if (filter.Data.MinAge > (DateTime.Now.Year - user.BirthDay.Year) ||
                        filter.Data.MaxAge < (DateTime.Now.Year - user.BirthDay.Year) ||
                        filter.Data.GenderId != user.GenderId
                        )
                    {
                        inValidAds.Add(ad);
                    }
                }
            } 
            return new SuccessDataResult<List<Ad>>(ads.Except(inValidAds).ToList());
        }
    }
}
