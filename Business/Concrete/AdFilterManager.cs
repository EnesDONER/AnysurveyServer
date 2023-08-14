using Business.Abstract;
using Business.Constants;
using Business.ValidationRules;
using Core.Aspects.Autofac.Validation;
using Core.Utilities.Business;
using Core.Utilities.Results;
using DataAccess.Abstract;
using Entities.Concrete;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Concrete
{
    public class AdFilterManager : IAdFilterService
    {
        private readonly IAdFilterDal _adFilterDal;
        public AdFilterManager(IAdFilterDal adFilterDal)
        {
            _adFilterDal = adFilterDal;
        }

        [ValidationAspect(typeof(AdFilterValidator))]
        public IResult Update(AdFilter adFilter)
        {
            var result = GetByAdId(adFilter.AdId);
            if (result.Success)
            {
                var id = result.Data.Id;
                AdFilter updatedAdFilter = new AdFilter()
                {
                    Id = id,
                    AdId = adFilter.AdId,
                    MaxAge = adFilter.MaxAge,
                    MinAge = adFilter.MinAge,
                    GenderId = adFilter.GenderId
                };
                _adFilterDal.Update(updatedAdFilter);
                return new SuccessResult();
            }
            _adFilterDal.Add(adFilter);
            return new SuccessResult(Messages.Added);
        }

        public IDataResult<AdFilter> GetByAdId(string adId)
        {
            var data = _adFilterDal.Get(af => af.AdId == adId);
            if (data == null )
            {
                return new ErrorDataResult<AdFilter>();
            }
            return new SuccessDataResult<AdFilter>(data);
        }

        //private IResult CheckAdFilterIsExist(string adId)
        //{
        //    if (GetByAdId(adId).Success)
        //    {
        //        return new SuccessResult();
        //    }
        //    return new ErrorResult();
        //}

    }
}
