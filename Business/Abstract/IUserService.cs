using Core.Entities.Concrete;
using Core.Utilities.Results;
using Entities.Concrete;
using Entities.Dtos;
using Microsoft.EntityFrameworkCore;

namespace Business.Abstract
{
    public interface IUserService
    {
        List<OperationClaim> GetClaims(User user);
        IDataResult<List<UserForWhoWatchedAds>> GetAllUsersWhoWatchedAdsByAdId(string adId);
        IResult Add(User user);
        User GetByMail(string email);
    }
}
