using Business.Abstract;
using Core.Aspects.Autofac.Caching;
using Core.Utilities.Results;
using DataAccess.Abstract;
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
            _addDal=addDal;
            _watchedAdService=watchedAdService;
        }
        [CacheRemoveAspect("IAdService.Get")]
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
        public IDataResult<List<Ad>> GetAllUnWatchedAd()
        {
 
            return new SuccessDataResult<List<Ad>>(_addDal.GetAll().Where(ad => !_watchedAdService.GetAll().Data.Any(watchedAd => watchedAd.AdId == ad.Id)).ToList());

        }
    }
}
