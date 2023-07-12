using Business.Abstract;
using Business.ThirdPartyServices.PaymentServices;
using Core.Utilities.Results;


namespace Business.Concrete
{
    public class PaymentManager : IPaymentService
    {
        IUserService _userService;
        ICardService _cardService;
        IThirdPartyPaymentService _thirdPartyPaymentService;
        public PaymentManager(IUserService userService, ICardService cardService,IThirdPartyPaymentService thirdPartyPaymentService)
        {
            _cardService=cardService;
            _userService=userService;
            _thirdPartyPaymentService=thirdPartyPaymentService;
        }
        public IResult Pay(int userId, int cardId, decimal amount)
        {
            var user = _userService.GetById(userId).Data;
            if(user == null)
                return new ErrorResult("user Null");

            var card = _cardService.GetById(cardId).Data;
            if (card == null)
                return new ErrorResult("card null");

            var result = _thirdPartyPaymentService.Pay(user,card,amount);
            if (!result.Success)
                return new ErrorResult(result.Message);


            return new SuccessDataResult<string>("Payment success");
        }

    }
}
