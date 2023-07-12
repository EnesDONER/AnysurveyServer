using Business.Abstract;
using Business.ThirdPartyServices.IyziPay;
using Core.Entities.Concrete;
using Core.Utilities.Results;
using Entities.Concrete;
using Iyzipay.Model;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Card = Entities.Concrete.Card;
using Options = Iyzipay.Options;

namespace Business.Concrete
{
    public class PaymentManager : IPaymentService
    {
        IUserService _userService;
        ICardService _cardService;
        public PaymentManager(IUserService userService, ICardService cardService)
        {
            _cardService=cardService;
            _userService=userService;
        }
        public IDataResult<Payment> Pay(int userId, int cardId, decimal amount)
        {
            var user = _userService.GetById(userId).Data;
            if(user == null)
                return new ErrorDataResult<Payment>("user Null");

            var card = _cardService.GetById(cardId).Data;
            if (card == null)
                return new ErrorDataResult<Payment>("card Null");


            var payment = PaymentTransactions.Pay(user, card, amount);
            if (payment.ErrorMessage != null)
                return new ErrorDataResult<Payment>(payment.ErrorMessage);


            return new SuccessDataResult<Payment>(payment);
        }

    }
}
