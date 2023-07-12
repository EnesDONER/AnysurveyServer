using Core.Entities.Concrete;
using Core.Utilities.Results;
using Entities.Concrete;
using Iyzipay.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Card = Entities.Concrete.Card;

namespace Business.Abstract
{
    public interface IPaymentService
    {
        IDataResult<Payment> Pay(int userId, int cardId, decimal amount);
    }
}
