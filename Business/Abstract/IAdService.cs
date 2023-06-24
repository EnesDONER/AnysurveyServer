using Core.Utilities.Results;
using Entities.Concrete;
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
        IDataResult<List<Ad>> GetAll();
    }
}
