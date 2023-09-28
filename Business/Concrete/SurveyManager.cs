using Business.Abstract;
using Business.BusinessAspects.Autofac;
using Business.Constants;
using Business.ThirdPartyServices.MessageBrokerServices;
using Core.Aspects.Autofac.Caching;
using Core.Aspects.Autofac.Logging;
using Core.CrossCuttingConcerns.Logging.Log4Net.Loggers;
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
using System.Collections.Generic;

namespace Business.Concrete
{
    public class SurveyManager : ISurveyService
    {
        private readonly ISurveyDal _surveyDal;
        private readonly ISurveyFilterService _surveyFilterService;
        private readonly ISolvedSurveyDal _solvedSurveyDal;
        private readonly IUserService _userService;
        private readonly IMessageBrokerService<EmailDto> _messageBrokerService;
        public SurveyManager(ISurveyDal surveydal, IUserService userService, ISurveyFilterService surveyFilterService, ISolvedSurveyDal solvedSurveyDal, IMessageBrokerService<EmailDto> messageBrokerService)
        {
            _surveyDal = surveydal;
            _userService = userService;
            _surveyFilterService = surveyFilterService;
            _solvedSurveyDal = solvedSurveyDal;
            _messageBrokerService = messageBrokerService;
        }

        [CacheRemoveAspect("ISurveyService.Get")]
        [SecuredOperation("partnership")]
        public IResult Add(Survey survey)
        {
            _surveyDal.Add(survey);
            return new SuccessResult(Messages.Added);
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
            return new SuccessResult(Messages.Deleted);
        }
        [CacheRemoveAspect("ISurveyService.Get")]
        public IResult Update(Survey survey)
        {
           _surveyDal.Update(survey);
            return new SuccessResult(Messages.Updated);
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

        public IDataResult<List<UserForWatchedOrSolvedContent>> GetAllUsersWhoSolvedSurveyBySurveyId(string surveyId)
        {
            var survey = GetById(surveyId);
            if (!survey.Success)
            {
                return new ErrorDataResult<List<UserForWatchedOrSolvedContent>>();
            }
            var solvedSurveys = GetAllSolvedSurvey().Data;
            List<UserForWatchedOrSolvedContent> result = new List<UserForWatchedOrSolvedContent>();
            foreach (var solvedSurvey in solvedSurveys)
            {
                if (survey.Data.Id == solvedSurvey.SurveyId)
                {
                    User user = _userService.GetById(solvedSurvey.UserId).Data;
                    UserForWatchedOrSolvedContent userForWhoWatchedAds = new UserForWatchedOrSolvedContent
                    {
                        Email = user.Email,
                        FirstName = user.FirstName,
                        LastName = user.LastName,
                        Age = DateTime.Now.Year - user.BirthDay.Year,
                        Gender = user.GenderId == 1 ? "Men": "Women"
                    };
                    result.Add(userForWhoWatchedAds);
                }
            }

            return new SuccessDataResult<List<UserForWatchedOrSolvedContent>>(result);
        }

        public IDataResult<List<Survey>> GetAllSurveysByOwnerUserId(int userId)
        {
            return new SuccessDataResult<List<Survey>>(_surveyDal.GetAll(s=>s.OwnerUserId==userId));
        }

        public IDataResult<List<Survey>> GetAllUnsolvedSurvey(int userId)
        {
            return new SuccessDataResult<List<Survey>>(GetAllFilteredSurveyByUserId(userId).Data.Where(survey => !GetAllSolvedSurveyByUserId(userId).Data.Any(solvedSurvey => solvedSurvey.SurveyId == survey.Id)).ToList());
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

        public IResult AddSolvedSurvey(SolvedSurvey solvedSurvey)
        {
            _solvedSurveyDal.Add(solvedSurvey);

            IResult result = sendMessageRabbitMQ(solvedSurvey);
            if (!result.Success)
            {
                return new ErrorResult(result.Message);
            }

            return new SuccessResult(Messages.Added);
        }

        public IDataResult<List<SolvedSurvey>> GetAllSolvedSurvey()
        {
            return new SuccessDataResult<List<SolvedSurvey>>(_solvedSurveyDal.GetAll());
        }

        public IDataResult<List<SolvedSurvey>> GetAllSolvedSurveyBySurveyId(string surveyId)
        {
            return new SuccessDataResult<List<SolvedSurvey>>(_solvedSurveyDal.GetAll(ss => ss.SurveyId == surveyId));
        }


        public IDataResult<List<SolvedSurvey>> GetAllSolvedSurveyByUserId(int userId)
        {
            return new SuccessDataResult<List<SolvedSurvey>>(_solvedSurveyDal.GetAll(ss => ss.UserId == userId));
        }

        public IDataResult<List<SurveyStatistic>> GetAllSolvedSurveyStatisticsBySurveyId(string surveyId)
        {
            List<SurveyStatistic> surveyStatistics = new List<SurveyStatistic>();
            var surveys = GetAllSolvedSurveyBySurveyId(surveyId).Data;
            foreach (var survey in surveys)
            {
                foreach (var question in survey.QuestionsAnswers)
                {
                    foreach (var answer in question.SelectedAnswers)
                    {


                        var selecetedAnswer = surveyStatistics?.Find(s => s.Answer == answer.SelectedOptionDescription)?.Answer;
                        if (selecetedAnswer != null && answer.SelectedOptionDescription == selecetedAnswer)
                        {
                            surveyStatistics.Find(s => s.Answer == answer.SelectedOptionDescription).Count++;
                        }
                        else
                        {
                            var statistic = new SurveyStatistic
                            {
                                Question = question.QuestionDescription,
                                Answer = answer.SelectedOptionDescription,
                                Count = 1
                            };
                            surveyStatistics.Add(statistic);
                        }


                    }

                }
            }
            return new SuccessDataResult<List<SurveyStatistic>>(surveyStatistics);
        }
        private IResult sendMessageRabbitMQ(SolvedSurvey solvedSurvey)
        {
            var senderUser = _userService.GetById(solvedSurvey.UserId).Data; // anket çözen kişi
            var addedSurvey = GetById(solvedSurvey.SurveyId).Data;
            var ownerUser = addedSurvey.OwnerUserId; //anket sahibi
            var senderUserName = senderUser.FirstName + senderUser.LastName;
            string answers = "";
            foreach (var question in solvedSurvey.QuestionsAnswers)
            {
                answers += question.QuestionDescription;

                foreach (var answer in question.SelectedAnswers)
                {
                    answers += answer.SelectedOptionDescription;
                    answers += ", \t ";
                }
                answers += "\n";
            }
            EmailDto email = new()
            {
                ConsumerUserEmail = _userService.GetById(ownerUser).Data.Email,
                Body = $"{senderUserName} solved your \"{addedSurvey.Description}\" survey \n His answers: " +
                $" \n {answers} ",
                Subject = "Your Survey Solved"
            };

            try
            {
                _messageBrokerService.AddQuee(queueName: "Email", email);

            }
            catch (Exception)
            {

                return new ErrorResult("RabbitMQ connecting is failed");
            }
            return new SuccessResult("Message sended");
        }

    }
}
