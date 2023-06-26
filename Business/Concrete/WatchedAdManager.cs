using Business.Abstract;
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
    public class WatchedAdManager:IWatchedAdService
    {
        private readonly IWatchedAdDal _watchedAdDal;
        public WatchedAdManager(IWatchedAdDal watchedAdDal)
        {
            _watchedAdDal = watchedAdDal;
        }

        public IResult Add(WatchedAd watcehedAd)
        {
            _watchedAdDal.Add(watcehedAd);
            return new SuccessResult();
        }

        public IDataResult<List<WatchedAd>> GetAll()
        {
            return new SuccessDataResult<List<WatchedAd>>(_watchedAdDal.GetAll());
        }

        public IDataResult<List<WatchedAd>> GetByUserId(int userId)
        {
            return new SuccessDataResult<List<WatchedAd>>(_watchedAdDal.GetAll(wa=>wa.UserId==userId));

        }
    }
}
