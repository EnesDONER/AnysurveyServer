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
        private readonly IAdService _adService;
        private readonly ISurveyService _surveyService;
        private readonly ISolvedSurveyService _solvedSurveyService;
        public StatisticsController(IWatchedAdService watchedAdService,IAdService adService,ISurveyService surveyService,ISolvedSurveyService solvedSurveyService)
        {
            _watchedAdService = watchedAdService;
            _httpContextAccessor = ServiceTool.ServiceProvider.GetService<IHttpContextAccessor>();
            _adService = adService;
            _surveyService = surveyService;
            _solvedSurveyService = solvedSurveyService;
        }

        //anket idsine göre çözülmüş anketleri listele
        [HttpGet("getallbysurveyidsolvedsurvey")]
        public IActionResult GetAllSolvedSurveyBySurveyId(string surveyId)
        {
            var result = _solvedSurveyService.GetAllBySurveyId(surveyId);
            if (result.Success)
            {
                return Ok(result);
            }
            return BadRequest(result.Message);

        }

        // anketin id sine göre o anketi çözen kullancıları dön
        [HttpGet("getalluserswhosolvedsurveysbysurveyid")]
        public IActionResult GetAllUsersWhoSolvedSurveysBySurveyId(string id)
        {
            var result = _surveyService.GetAllUsersWhoSolvedSurveyBySurveyId(id);

            if (result.Success)
            {
                return Ok(result);
            }
            return BadRequest(result.Message);

        }

        [HttpPost("addsolvedsurvey")]
        public IActionResult AddSolvedSurvey(SolvedSurvey solvedSurvey)
        {
            int userId = Convert.ToInt16(_httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value);
            SolvedSurvey newSolvedSurvey = new SolvedSurvey
            {
                SurveyId = solvedSurvey.SurveyId,
                UserId = userId,
                QuestionsAnswers = solvedSurvey.QuestionsAnswers
            };
            var result = _solvedSurveyService.Add(newSolvedSurvey);

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
            var result = _adService.GetAllUsersWhoWatchedAdsByAdId(id);

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
