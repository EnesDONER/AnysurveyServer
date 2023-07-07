using Business.Abstract;
using Core.Utilities.IoC;
using Entities.Concrete;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Bson;
using System.Security.Claims;

namespace WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ContentController : ControllerBase
    {
        private readonly ISurveyService _surveyService;
        private readonly IAdService _addService;
        private IHttpContextAccessor _httpContextAccessor;

        public ContentController(ISurveyService surveyService, IAdService addService)
        {
             _surveyService=surveyService;
            _addService=addService;
            _httpContextAccessor = ServiceTool.ServiceProvider.GetService<IHttpContextAccessor>();

        }
        [HttpGet("getallsurveys")]
        public IActionResult GetAllSurvey()
        {
            var result = _surveyService.GetAll();
            if (result.Success)
            {
                return Ok(result);
            }
            return BadRequest(result.Message);
        }
        [HttpGet("getsurveybyid")]
        public IActionResult GetSurveyById(string id)
        {
            var result = _surveyService.GetById(id);
            if (result.Success)
            {
                return Ok(result);
            }
            return BadRequest(result.Message);
        }
        [HttpPost("addsurvey")]
        public IActionResult AddSurvey (Survey survey)
        {
            int userId = Convert.ToInt16(_httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value);
            Survey newSurvey = new Survey
            {
                Title = survey.Title,
                Description =survey.Description,
                OwnerUserId = userId,
                Questions = survey.Questions
            };
            var result = _surveyService.Add(newSurvey);
            if (result.Success)
            {
                return Ok(result);
            }
            return BadRequest(result.Message);
        }
        //kişinin eklediği anketleri listeler
        [HttpGet("getallsurveysbyowneruserid")]
        public IActionResult GetAllSurveysByOwnerUserId()
        {
            int userId = Convert.ToInt16(_httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value);

            var result = _surveyService.GetAllSurveysByOwnerUserId(userId);
            if (result.Success)
            {
                return Ok(result);
            }
            return BadRequest(result.Message);
        }
        //kişinin çözmediği anketleri listeler
        [HttpGet("getallunsolvedsurveys")]
        public IActionResult GetAllUnsolvedSurveys()
        {
            int userId = Convert.ToInt16(_httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value);

            var result =  _surveyService.GetAllUnsolvedSurvey(userId);
            if (result.Success)
            {
                return Ok(result);
            }
            return BadRequest(result.Message);
        }

        //[HttpPost("deletesurvey")] 
        //public IActionResult DeleteSurvey(Survey survey)
        //{
        //    var result = _surveyService.Delete(survey);
        //    if (result.Success)
        //    {
        //        return Ok(result);
        //    }
        //    return BadRequest(result.Message);
        //}
        //[HttpPost("updatesurvey")]
        //public IActionResult UpdateSurvey(Survey survey)
        //{     
        //    var result = _surveyService.Update(survey);
        //    if (result.Success)
        //    {
        //        return Ok(result);
        //    }
        //    return BadRequest(result.Message);
        //}

        //[HttpGet("getallads")]
        //public IActionResult GetAllAds()
        //{
        //    var result = _addService.GetAll();
        //    if (result.Success)
        //    {
        //        return Ok(result);
        //    }
        //    return BadRequest(result.Message);
        //}

        //kişinin izlemediği reklamları listeler
        [HttpGet("getallunwatchedads")]
        public IActionResult GetAllUnWatchedAds()
        {
            int userId = Convert.ToInt16(_httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value);

            var result = _addService.GetAllUnWatchedAd(userId);
            if (result.Success)
            {
                return Ok(result);
            }
            return BadRequest(result.Message);
        }
        // kişinin eklediği reklamları listeler
        [HttpGet("getalladsbyowneruserid")]
        public IActionResult GetAllAdsByOwnerUserId()
        {
            int userId = Convert.ToInt16(_httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value);

            var result =   _addService.GetAllAdsByOwnerUserId(userId);
            if (result.Success)
            {
                return Ok(result);
            }
            return BadRequest(result.Message);
        }
        [HttpPost("addad")]
        public IActionResult AddAd(Ad ad)
        {
            int userId = Convert.ToInt16(_httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value);
            Ad newAd = new Ad
            {
                CompanyName = ad.CompanyName,
                Description = ad.Description,
                OwnerUserId = userId,
                VideoURL = ad.VideoURL,
            };
            var result = _addService.Add(newAd);
            if (result.Success)
            {
                return Ok(result);
            }
            return BadRequest(result.Message);
        }
    }
}
