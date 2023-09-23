using Core.Entities.Concrete;
using Core.Utilities.Results;
using Entities.Concrete;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.ThirdPartyServices.PaymentServices.PayPal
{
    public class PaypalAdapter : IThirdPartyPaymentService
    {
        public IResult Pay(User user, Card card, decimal amount)
        {
            return new SuccessResult("Paypal");
        }
    }
}
