using Core.Utilities.Results;
using Entities.Concrete;
using Entities.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Abstract
{
    public interface ISolvedSurveyService
    {
        IResult Add(SolvedSurvey solvedSurvey);
        IDataResult<List<SolvedSurvey>> GetAll();
        IDataResult<List<SolvedSurvey>> GetAllByUserId(int userId);
        IDataResult<List<SolvedSurvey>> GetAllBySurveyId(string surveyId);
        IDataResult<List<SurveyStatistic>> GetAllSolvedSurveyStatisticsBySurveyId(string surveyId);


    }
}
