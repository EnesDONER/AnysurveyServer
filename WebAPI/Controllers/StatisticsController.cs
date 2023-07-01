using Business.Abstract;
using Core.Entities.Concrete;
using Core.Utilities.IoC;
using Entities.Concrete;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StatisticsController : ControllerBase
    {
        private readonly IWatchedAdService _watchedAdService;
        private IHttpContextAccessor _httpContextAccessor;
        private readonly IUserService _userService;
        public StatisticsController(IWatchedAdService watchedAdService, IUserService userService)
        {
            _watchedAdService = watchedAdService;
            _httpContextAccessor = ServiceTool.ServiceProvider.GetService<IHttpContextAccessor>();
            _userService = userService;
        }
        [HttpGet("getallwatchedad")]
        public IActionResult GetAllWatchedAd()
        {
            var result = _watchedAdService.GetAll();
            if (result.Success)
            {
                return Ok(result);
            }
            return BadRequest(result.Message);
            
        }
        //id si verilen kişinin izlediği reklamları listeler
        [HttpGet("getallbyuseridwatchedad")]
        public IActionResult GetAllByUserIdWatchedAd(int userId)
        {
            var result = _watchedAdService.GetAllByUserId(userId);
            if (result.Success)
            {
                return Ok(result);
            }
            return BadRequest(result.Message);

        }
        // reklamın id sine göre o reklamı izleyen kullancıları dön
        [HttpGet("getalluserswhowatchedadsbyadid")]
        public IActionResult GetAllUsersWhoWatchedAdsByAdId(string id)
        {
            var result = _userService.GetAllUsersWhoWatchedAdsByAdId(id);

            if (result.Success)
            {
                return Ok(result);
            }
            return BadRequest(result.Message);

        }

        [HttpPost("addwatchedad")]
        public IActionResult AddWatchedAd(string adId)
        {
            int userId = Convert.ToInt16(_httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value);
            WatchedAd watchedAd = new WatchedAd
            {
                UserId = userId,
                AdId = adId,

            };
            var result = _watchedAdService.Add(watchedAd);

            if (result.Success)
            {
                return Ok(result);
            }
            return BadRequest(result.Message);
        }
    }
}
