using Azure;
using Business.Abstract;
using Business.BusinessAspects.Autofac;
using Core.Aspects.Autofac.Caching;
using Core.Entities.Concrete;
using Core.Utilities.Results;
using DataAccess.Abstract;
using DataAccess.Concrete.MongoDB;
using Entities.Concrete;
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

        public AdManager(IAdDal addDal, IWatchedAdService watchedAdService)
        {
            _addDal = addDal;
            _watchedAdService = watchedAdService;
        }

        [CacheRemoveAspect("IAdService.Get")]
        [SecuredOperation("partnership")]
        public IResult Add(Ad add)
        {
            _addDal.Add(add);
            return new SuccessResult();

        }
        [CacheAspect(10)]
        public IDataResult<List<Ad>> GetAll()
        {
            return new SuccessDataResult<List<Ad>>(_addDal.GetAll());
        }

        [CacheAspect(10)]
        public IDataResult<List<Ad>> GetAllUnWatchedAd(int userId)
        {
 
            return new SuccessDataResult<List<Ad>>(_addDal.GetAll().Where(ad => !_watchedAdService.GetAllByUserId(userId).Data.Any(watchedAd => watchedAd.AdId == ad.Id)).ToList());

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
    }
}
