using Business.Abstract;
using Business.BusinessAspects.Autofac;
using Core.Aspects.Autofac.Caching;
using Core.Entities;
using Core.Entities.Concrete;
using Core.Utilities.Results;
using DataAccess.Abstract;
using DataAccess.Concrete.MongoDB;
using Entities.Concrete;
using Entities.Dtos;
using MongoDB.Bson;
using MongoDB.Driver;
using SharpCompress.Common;
using System;

namespace Business.Concrete
{
    public class SurveyManager : ISurveyService
    {
        private readonly ISurveyDal _surveyDal;
        private readonly ISurveyFilterService _surveyFilterService;
        private readonly ISolvedSurveyService _solvedSurveyService;
        private readonly IUserService _userService;
        public SurveyManager(ISurveyDal surveydal,IUserService userService, ISurveyFilterService surveyFilterService,ISolvedSurveyService solvedSurveyService)
        {
            _surveyDal = surveydal;
            _userService = userService;
            _surveyFilterService = surveyFilterService;
            _solvedSurveyService = solvedSurveyService;
        }
        
        [CacheRemoveAspect("ISurveyService.Get")]
        [SecuredOperation("partnership")]
        public IResult Add(Survey survey)
        {
            _surveyDal.Add(survey);
            return new SuccessResult();
        }
        
        //[SecuredOperation("admin")]
        //[CacheAspect(duration: 10)]
        public IDataResult<List<Survey>> GetAll()
        {
            return new SuccessDataResult<List<Survey>>(_surveyDal.GetAll());
        }
      
        [CacheRemoveAspect("ISurveyService.Get")]
        public IResult Delete(Survey survey)
        {
            _surveyDal.Delete(survey);
            return new SuccessResult();
        }
        [CacheRemoveAspect("ISurveyService.Get")]
        public IResult Update(Survey survey)
        {
           _surveyDal.Update(survey);
            return new SuccessResult();
        }
        public IDataResult<Survey> GetById(string id)
        {
            return new SuccessDataResult<Survey>(_surveyDal.Get(s=>s.Id==id));
        }

        public IDataResult<List<Survey>> GetAllFilteredSurveyByUserId(int userId)
        {
            var data = _surveyDal.GetAll();
            var result = ApplyFilter(data, userId);
            if (result.Success)
            {
                return new SuccessDataResult<List<Survey>>(result.Data);
            }
            return new ErrorDataResult<List<Survey>>();

        }

        public IDataResult<List<UserForWhoWatchedAds>> GetAllUsersWhoSolvedSurveyBySurveyId(string surveyId)
        {
            var survey = GetById(surveyId);
            if (!survey.Success)
            {
                return new ErrorDataResult<List<UserForWhoWatchedAds>>();
            }
            var solvedSurveys = _solvedSurveyService.GetAll().Data;
            List<UserForWhoWatchedAds> result = new List<UserForWhoWatchedAds>();
            foreach (var solvedSurvey in solvedSurveys)
            {
                if (survey.Data.Id == solvedSurvey.SurveyId)
                {
                    User user = _userService.GetById(solvedSurvey.UserId).Data;
                    UserForWhoWatchedAds userForWhoWatchedAds = new UserForWhoWatchedAds
                    {
                        Email = user.Email,
                        FirstName = user.FirstName,
                        LastName = user.LastName,
                    };
                    result.Add(userForWhoWatchedAds);
                }
            }

            return new SuccessDataResult<List<UserForWhoWatchedAds>>(result);
        }

        public IDataResult<List<Survey>> GetAllSurveysByOwnerUserId(int userId)
        {
            return new SuccessDataResult<List<Survey>>(_surveyDal.GetAll(s=>s.OwnerUserId==userId));
        }

        public IDataResult<List<Survey>> GetAllUnsolvedSurvey(int userId)
        {
            return new SuccessDataResult<List<Survey>>(GetAllFilteredSurveyByUserId(userId).Data.Where(survey => !_solvedSurveyService.GetAllByUserId(userId).Data.Any(solvedSurvey => solvedSurvey.SurveyId == survey.Id)).ToList());
        }
        private IDataResult<List<Survey>> ApplyFilter(List<Survey> surveys, int userId)
        {
            List<Survey> inValidSurveys = new List<Survey>();
            foreach (var survey in surveys)
            {
                var filter = _surveyFilterService.GetBySurveyId(survey.Id);
                if (filter.Success)
                {
                    var user = _userService.GetById(userId).Data;
                    if (filter.Data.MinAge > (DateTime.Now.Year - user.BirthDay.Year) ||
                        filter.Data.MaxAge < (DateTime.Now.Year - user.BirthDay.Year) ||
                        filter.Data.GenderId != user.GenderId
                        )
                    {
                        inValidSurveys.Add(survey);
                    }
                }
            }
            return new SuccessDataResult<List<Survey>>(surveys.Except(inValidSurveys).ToList());
        }
    }
}
