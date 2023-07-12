using Business.Abstract;
using Core.Utilities.IoC;
using Entities.Concrete;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Options;
using System.Net;
using System.Security.Claims;

namespace WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PaymentController : ControllerBase
    {
        private readonly ICardService _cardService;
        private readonly IPaymentService _paymentService;
        private readonly IUserService _userService;
        private IHttpContextAccessor _httpContextAccessor;
        public PaymentController(ICardService cardService,IPaymentService paymentService,IUserService userService)
        {
            _cardService = cardService;
            _paymentService = paymentService;
            _userService = userService;
            _httpContextAccessor = ServiceTool.ServiceProvider.GetService<IHttpContextAccessor>();

        }
        //[HttpGet("getall")]
        //public IActionResult GetAll()
        //{
        //    var result = _cardService.GetAll();
        //    if (result.Success)
        //    {
        //        return Ok(result);
        //    }
        //    return BadRequest(result.Message);
        //}
        [HttpPost("addcard")]
        public IActionResult AddCard(Card card)
        {
            int userId = Convert.ToInt16(_httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value);
            Card updatedCard = new Card();
            updatedCard = card;
            updatedCard.UserId = userId;
            var result = _cardService.Add(updatedCard);
            if (result.Success)
            {
                return Ok(result);
            }
            return BadRequest(result.Message);
        }


        [HttpGet("getallcardbyuserid")]
        public IActionResult GetAllCard()
        {
            int userId = Convert.ToInt16(_httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value);

            var result = _cardService.GetAllCardByUserId(userId);
            if (result.Success)
            {
                return Ok(result);
            }
            return BadRequest(result.Message);
        }

        [HttpGet("payment")]
        public IActionResult Payment(int cardId,decimal amount)
        {
            int userId = Convert.ToInt16(_httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value);

            var result= _paymentService.Pay(userId,cardId,amount);
            if (result.Success)
            {
                return Ok(result);
            }
            return BadRequest(result.Message);
        }
    }
}
