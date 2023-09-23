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
        public PaymentController(ICardService cardService,IPaymentService paymentService)
        {
            _cardService = cardService;
            _paymentService = paymentService;

        }
        [HttpPost("addcard")]
        public IActionResult AddCard(Card card)
        {
          
            var result = _cardService.Add(card);
            if (result.Success)
            {
                return Ok(result);
            }
            return BadRequest(result.Message);
        }


        [HttpGet("getallcardbyuserid")]
        public IActionResult GetAllCard(int userId)
        {

            var result = _cardService.GetAllCardByUserId(userId);
            if (result.Success)
            {
                return Ok(result);
            }
            return BadRequest(result.Message);
        }

        [HttpGet("payment")]
        public IActionResult Payment(int cardId,decimal amount,int userId)
        {

            var result= _paymentService.Pay(userId,cardId,amount);
            if (result.Success)
            {
                return Ok(result);
            }
            return BadRequest(result.Message);
        }
    }
}
