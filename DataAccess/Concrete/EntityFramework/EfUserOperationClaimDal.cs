using Core.Entities.Concrete;
using DataAccess.Abstract;
using DataAccess.Context;
using DataAccess.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Concrete.EntityFramework
{
    public class EfUserOperationClaimDal:EfEntityRepositoryBase<UserOperationClaim,MSSqlDBContext>,IUserOperationClaimDal
    {
    }
}
