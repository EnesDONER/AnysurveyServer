using Core.Entities.Concrete;
using DataAccess.Abstract;
using DataAccess.Context;
using DataAccess.Repository;
using Entities.Concrete;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Concrete.EntityFramework
{
    public class EfCardDal : EfEntityRepositoryBase<Card, MSSqlDBContext>, ICardDal
    {
    }
}
