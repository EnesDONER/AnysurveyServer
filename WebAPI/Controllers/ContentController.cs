using Business.Abstract;
using Entities.Concrete;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Bson;

namespace WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ContentController : ControllerBase
    {
        private readonly ISurveyService _surveyService;
        private readonly IAdService _addService;
        public ContentController(ISurveyService surveyService, IAdService addService)
        {
             _surveyService=surveyService;
            _addService=addService;
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
            var result = _surveyService.Add(survey);
            if (result.Success)
            {
                return Ok(result);
            }
            return BadRequest(result.Message);
        }
        [HttpPost("deletesurvey")] 
        public IActionResult DeleteSurvey(Survey survey)
        {
            var result = _surveyService.Delete(survey);
            if (result.Success)
            {
                return Ok(result);
            }
            return BadRequest(result.Message);
        }
        [HttpPost("updatesurvey")]
        public IActionResult UpdateSurvey(Survey survey)
        {     
            var result = _surveyService.Update(survey);
            if (result.Success)
            {
                return Ok(result);
            }
            return BadRequest(result.Message);
        }

        [HttpGet("getallads")]
        public IActionResult GetAllAds()
        {
            var result = _addService.GetAll();
            if (result.Success)
            {
                return Ok(result);
            }
            return BadRequest(result.Message);
        }

        [HttpGet("getallunwatchedads")]
        public IActionResult GetAllUnWatchedAds()
        {
            var result = _addService.GetAllUnWatchedAd();
            if (result.Success)
            {
                return Ok(result);
            }
            return BadRequest(result.Message);
        }
        [HttpPost("addad")]
        public IActionResult AddAd(Ad ad)
        {
            var result = _addService.Add(ad);
            if (result.Success)
            {
                return Ok(result);
            }
            return BadRequest(result.Message);
        }
    }
}
