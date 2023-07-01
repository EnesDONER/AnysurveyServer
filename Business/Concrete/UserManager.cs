using Business.Abstract;
using Core.Aspects.Autofac.Caching;
using Core.Entities.Concrete;
using Core.Utilities.Results;
using DataAccess.Abstract;
using DataAccess.Concrete.MongoDB;
using Entities.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Concrete
{
    public class UserManager : IUserService
    {
        IUserDal _userDal;
        IAdService _adService;
        IWatchedAdService _watchedAdService;
        public UserManager(IUserDal userDal, IAdService adService, IWatchedAdService watchedAdService)
        {
            _userDal = userDal;
            _adService = adService;
            _watchedAdService = watchedAdService;
        }

        public List<OperationClaim> GetClaims(User user)
        {
            return _userDal.GetClaims(user);
        }

        public IResult Add(User user)
        {
            _userDal.Add(user);
            return new SuccessResult();
        }

        public User GetByMail(string email)
        {
            return _userDal.Get(u => u.Email == email);
        }
        
        [CacheAspect(10)]
        public IDataResult<List<UserForWhoWatchedAds>> GetAllUsersWhoWatchedAdsByAdId(string adId)
        {

            var ad = _adService.GetById(adId);
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
                    User user = GetById(watchedAd.UserId).Data;
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

        private IDataResult<User> GetById(int id)
        {
            return new SuccessDataResult<User>(_userDal.Get(u => u.Id == id));
        }
    }
}
