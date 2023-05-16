using Business.Abstract;
using Core.Utilities.Results;
using DataAccess.Abstract;
using Entities.Concrete;
using MongoDB.Bson;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Concrete
{
    public class SurveyManager : ISurveyService
    {
        private readonly ISurveyDal _surveydal;
        public SurveyManager(ISurveyDal surveydal)
        {
            _surveydal = surveydal;
        }

        public IResult Add(Survey survey)
        {
            _surveydal.Add(survey);
            return new SuccessResult();
        }

        public IDataResult<List<Survey>> GetAll()
        {
            return new SuccessDataResult<List<Survey>>(_surveydal.GetAll());
        }

        public IResult Delete(string Id)
        {
            _surveydal.Delete(Id);
            return new SuccessResult();
        }

        public IResult Update(Survey survey)
        {
           _surveydal.Update(survey);
            return new SuccessResult();
        }

        public IDataResult<Survey> GetById(string Id)
        {
            return new SuccessDataResult<Survey>(_surveydal.Get(s => s.Id == Id));
        }
    }
}
