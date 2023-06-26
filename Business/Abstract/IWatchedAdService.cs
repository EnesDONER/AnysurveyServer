using Core.Utilities.Results;
using Entities.Concrete;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Abstract
{
    public interface IWatchedAdService
    {
        IResult Add(WatchedAd watcehedAd);
        IDataResult<List<WatchedAd>> GetAll();
        IDataResult<List<WatchedAd>> GetAllByUserId(int userId);

    }
}
