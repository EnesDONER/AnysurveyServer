using Core.Utilities.Results;
using Entities.Concrete;
using Entities.Dtos;
using MongoDB.Bson;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Abstract
{
    public interface ISurveyService
    {
        IResult Add(Survey survey);
        IResult Delete(Survey survey);
        IResult Update(Survey survey);
        IDataResult<Survey> GetById(string id);
        IDataResult<List<Survey>> GetAll();
        IDataResult<List<Survey>> GetAllFilteredSurveyByUserId(int userId);
        IDataResult<List<UserForWatchedOrSolvedContent>> GetAllUsersWhoSolvedSurveyBySurveyId(string surveyId);
        IDataResult<List<Survey>> GetAllSurveysByOwnerUserId(int userId);
        IDataResult<List<Survey>> GetAllUnsolvedSurvey(int userId);
    }
}
