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
        private  IAdDal _addDal;
        public AdManager(IAdDal addDal)
        {
            _addDal=addDal;
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
    }
}
