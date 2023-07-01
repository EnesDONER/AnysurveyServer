using Business.Abstract;
using Core.Entities.Concrete;
using Core.Utilities.Results;
using DataAccess.Abstract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Concrete
{
    public class UserOperationClaimManager : IUserOperationClaimService
    {
        IUserOperationClaimDal _userOperationClaimDal;
        public UserOperationClaimManager(IUserOperationClaimDal userOperationClaimDal)
        {
            _userOperationClaimDal = userOperationClaimDal;
        }

        public IResult Add(UserOperationClaim userOperationClaim)
        {
            if (!CheckPartnershipClaimExists(userOperationClaim.UserId).Success)
            {
                return new ErrorResult();
            }
            _userOperationClaimDal.Add(userOperationClaim);
            return new SuccessResult();
        }
        
        public IDataResult<List<UserOperationClaim>> GetAll()
        {
            return new SuccessDataResult<List<UserOperationClaim>>(_userOperationClaimDal.GetAll());
        }

        public IDataResult<List<UserOperationClaim>> GetAllByUserId(int userId)
        {
            return new SuccessDataResult<List<UserOperationClaim>>(_userOperationClaimDal.GetAll(u => u.UserId == userId));
        }

        private IResult CheckPartnershipClaimExists(int userId)
        {
            foreach (var userOperationClaim in _userOperationClaimDal.GetAll())
            {
                if (userOperationClaim.UserId == userId && userOperationClaim.OperationClaimId == 1)
                {
                    return new ErrorResult(message: "User AllReady Exist");
                }
            }
            return new SuccessResult();
            
        }
    }
}
