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
        private readonly IAdService _adService;
        private readonly ISurveyService _surveyService;
        private readonly ISolvedSurveyService _solvedSurveyService;
        public StatisticsController(IAdService adService,ISurveyService surveyService,ISolvedSurveyService solvedSurveyService)
        {
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

        // istatistik getirir hangi soruya kaç kişi hangi cevabı verdiğini
        [HttpGet("getallsurveystatistics")]
        public IActionResult GetAllSurveyStatistics(string id)
        {
            var result = _solvedSurveyService.GetAllSolvedSurveyStatisticsBySurveyId(id);

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
       
            var result = _solvedSurveyService.Add(solvedSurvey);

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
            var result = _adService.GetAllWatchedAdByUserId(userId);
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
        public IActionResult AddWatchedAd(WatchedAd watchedAd)
        {

            var result = _adService.AddWatchedAd(watchedAd);

            if (result.Success)
            {
                return Ok(result);
            }
            return BadRequest(result.Message);
        }
    }
}
