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
    public class AdManager : IAdService
    {
        private readonly IAdDal _addDal;
        public AdManager(IAdDal addDal)
        {
            _addDal=addDal; 
        }
        public IResult Add(Ad add)
        {
            _addDal.Add(add);
            return new SuccessResult();

        }

        public IDataResult<List<Ad>> GetAll()
        {
            return new SuccessDataResult<List<Ad>>(_addDal.GetAll());
        }
    }
}
