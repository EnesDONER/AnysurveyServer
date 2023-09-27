using Business.Abstract;
using Core.Utilities.IoC;
using Entities.Concrete;
using Entities.Dtos;
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
            //int userId = Convert.ToInt16(_httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value);
            Survey newSurvey = new Survey
            {
                Title = survey.Title,
                Description =survey.Description,
                OwnerUserId = survey.OwnerUserId,
                Questions = survey.Questions
            };
            var result = _surveyService.Add(newSurvey);
            if (result.Success)
            {
                return Ok(result);
            }
            return BadRequest(result.Message);
        }



        [HttpPost("upload")]
        public async Task<IActionResult> Upload(string containerName,string id, IFormFileCollection files)
        {
            var result = await _addService.Upload(containerName,id, files);

            return Ok(result);
        }

        [HttpGet("getallfile")]
        public IActionResult GetAllFile(string containerName)
        {
            var result = _addService.GetAllFile(containerName);
            if (result.Success)
            {
                return Ok(result);
            }
            return BadRequest(result.Message);
        }

        [HttpGet("hasfile")]
        public IActionResult HasFile(string containerName,string fileName)
        {
            var result = _addService.HasFile(containerName,fileName);
            if (result.Success)
            {
                return Ok(result);
            }
            return BadRequest(result.Message);
        }





        //kişinin eklediği anketleri listeler
        [HttpGet("getallsurveysbyowneruserid")]
        public IActionResult GetAllSurveysByOwnerUserId(int userId)
        {
            //int userId = Convert.ToInt16(_httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value);

            var result = _surveyService.GetAllSurveysByOwnerUserId(userId);
            if (result.Success)
            {
                return Ok(result);
            }
            return BadRequest(result.Message);
        }
        //kişinin çözmediği anketleri listeler
        [HttpGet("getallunsolvedsurveys")]
        public IActionResult GetAllUnsolvedSurveys(int userId)
        {
            //int userId = Convert.ToInt16(_httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value);

            var result =  _surveyService.GetAllUnsolvedSurvey(userId);
            if (result.Success)
            {
                return Ok(result);
            }
            return BadRequest(result.Message);
        }

        //kişinin izlemediği reklamları listeler
        [HttpGet("getallunwatchedads")]
        public IActionResult GetAllUnWatchedAds(int userId)
        {
            //int userId = Convert.ToInt16(_httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value);

            var result = _addService.GetAllUnWatchedAd(userId);
            if (result.Success)
            {
                return Ok(result);
            }
            return BadRequest(result.Message);
        }
        // kişinin eklediği reklamları listeler
        [HttpGet("getalladsbyowneruserid")]
        public IActionResult GetAllAdsByOwnerUserId(int userId)
        {
           // int userId = Convert.ToInt16(_httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value);

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
     
            var result = _addService.Add(ad);
            if (result.Success)
            {
                return Ok(result);
            }
            return BadRequest(result.Message);
        }

        [HttpPost("addadandupload")]
        public async Task<IActionResult> AddAdandUploadAsync([FromForm] AdUploadDto adUploadDto)
        {

            var result = await _addService.AddAdandUploadAsync(adUploadDto);
            if (result.Success)
            {
                return Ok(result);
            }
            return BadRequest(result.Message);
        }
    }
}
