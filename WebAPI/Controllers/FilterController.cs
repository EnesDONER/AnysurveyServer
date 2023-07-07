using Business.Abstract;
using Entities.Concrete;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FilterController : ControllerBase
    {
        IAdFilterService _adFilterService;
        ISurveyFilterService _surveyFilterService;
        public FilterController(IAdFilterService adFilterService, ISurveyFilterService surveyFilterService)
        {
            _adFilterService = adFilterService;
            _surveyFilterService = surveyFilterService;
        }

        [HttpGet("getadfilterbyadid")]
        public IActionResult GetAdFilterByAdId(string adId)
        {

            var result = _adFilterService.GetByAdId(adId);

            if (result.Success)
            {
                return Ok(result);
            }
            return BadRequest(result.Message);
        }

        [HttpGet("getsurveyfilterbysurveyid")]
        public IActionResult GetSurveyFilterBySurveyId(string surveyId)
        {

            var result = _surveyFilterService.GetBySurveyId(surveyId);

            if (result.Success)
            {
                return Ok(result);
            }
            return BadRequest(result.Message);
        }
        //[HttpPost("addadfilter")]
        //public IActionResult AddAdFilter(AdFilter adFilter)
        //{

        //    var result = _adFilterService.Add(adFilter);

        //    if (result.Success)
        //    {
        //        return Ok(result);
        //    }
        //    return BadRequest(result.Message);
        //}

        [HttpPost("updateadfilter")]
        public IActionResult UpdateAdFilter(AdFilter adFilter)
        {

            var result = _adFilterService.Update(adFilter);

            if (result.Success)
            {
                return Ok(result);
            }
            return BadRequest(result.Message);
        }
        [HttpPost("updatesurveyfilter")]
        public IActionResult UpdateSurveyFilter(SurveyFilter surveyFilter)
        {

            var result = _surveyFilterService.Update(surveyFilter);

            if (result.Success)
            {
                return Ok(result);
            }
            return BadRequest(result.Message);
        }

    }
}
