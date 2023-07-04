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
    public class AdFilterManager : IAdFilterService
    {
        private readonly IAdFilterDal _adFilterDal;
        public AdFilterManager(IAdFilterDal adFilterDal)
        {
            _adFilterDal = adFilterDal;
        }

        public IResult Add(AdFilter adFilter)
        {
            _adFilterDal.Add(adFilter);
            return new SuccessResult();
        }

        public IDataResult<AdFilter> GetByAdId(string adId)
        {
            var data = _adFilterDal.Get(af => af.AdId == adId);
            if (data==null)
            {
                return new ErrorDataResult<AdFilter>();
            }
            return new SuccessDataResult<AdFilter>(data);
        }

        //public IDataResult<List<AdFilter>> GetAll()
        //{
        //    return new SuccessDataResult<List<AdFilter>>(_adFilterDal.GetAll());
        //}
    }
}
