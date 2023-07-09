using Business.Abstract;
using Core.Utilities.Results;
using DataAccess.Abstract;
using Entities.Concrete;
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
            return new SuccessResult();
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
    }
}
