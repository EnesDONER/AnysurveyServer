using Business.Abstract;
using Business.ThirdPartyServices.PaymentServices;
using Core.Utilities.Results;


namespace Business.Concrete
{
    public class PaymentManager : IPaymentService
    {
        private readonly IUserService _userService;
        private readonly ICardService _cardService;
        private readonly IThirdPartyPaymentService  _thirdPartyPaymentService;

        public PaymentManager(IUserService userService, ICardService cardService,
            IThirdPartyPaymentService thirdPartyPaymentService)
        {
            _cardService=cardService;
            _userService=userService;
            _thirdPartyPaymentService=thirdPartyPaymentService;
        }


        public IResult Pay(int userId, int cardId, decimal amount)
        {
            var user = _userService.GetById(userId).Data;
            if(user == null)
                return new ErrorResult("user was not exist");

            var card = _cardService.GetById(cardId).Data;
            if (card == null)
                return new ErrorResult("card was not exist");

            var result = _thirdPartyPaymentService.Pay(user,card,amount);
            if (!result.Success)
                return new ErrorResult(result.Message);


            return new SuccessResult(result.Message);
        }

    }
}
