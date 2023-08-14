using Business.Abstract;
using Business.Constants;
using Core.Utilities.Results;
using DataAccess.Abstract;
using Entities.Concrete;
using Entities.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Concrete
{
    public class SolvedSurveyManager: ISolvedSurveyService
    {
        private readonly ISolvedSurveyDal _solvedSurveyDal;
        public SolvedSurveyManager(ISolvedSurveyDal solvedSurveyDal)
        {
            _solvedSurveyDal = solvedSurveyDal;
        }

        public IResult Add(SolvedSurvey solvedSurvey)
        {
            _solvedSurveyDal.Add(solvedSurvey);
            return new SuccessResult(Messages.Added);
        }

        public IDataResult<List<SolvedSurvey>> GetAll()
        {
            return new SuccessDataResult<List<SolvedSurvey>>(_solvedSurveyDal.GetAll());
        }

        public IDataResult<List<SolvedSurvey>> GetAllBySurveyId(string surveyId)
        {
            return new SuccessDataResult<List<SolvedSurvey>>(_solvedSurveyDal.GetAll(ss=>ss.SurveyId==surveyId));
        }
        

        public IDataResult<List<SolvedSurvey>> GetAllByUserId(int userId)
        {
            return new SuccessDataResult<List<SolvedSurvey>>(_solvedSurveyDal.GetAll(ss=>ss.UserId==userId));
        }

        public IDataResult<List<SurveyStatistic>> GetAllSolvedSurveyStatisticsBySurveyId(string surveyId)
        {
            List<SurveyStatistic> surveyStatistics = new List<SurveyStatistic>();
            var surveys = GetAllBySurveyId(surveyId).Data;
            foreach (var survey in surveys)
            {
                foreach (var  question in survey.QuestionsAnswers)
                {
                    foreach (var answer in question.SelectedAnswers)
                    {
                        
                        
                            var selecetedAnswer = surveyStatistics?.Find(s => s.Answer == answer.SelectedOptionDescription)?.Answer;
                            if (selecetedAnswer != null && answer.SelectedOptionDescription == selecetedAnswer )
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
    }
}
