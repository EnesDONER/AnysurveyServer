using Core.Utilities.Results;
using Entities.Concrete;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Abstract
{
    public interface IAdFilterService
    {
        IResult Update(AdFilter adFilter);
        IDataResult<AdFilter> GetByAdId(string adId);

    }
}
