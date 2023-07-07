using Business.Abstract;
using Core.Utilities.Results;
using DataAccess.Abstract;
using DataAccess.Concrete.MongoDB;
using Entities.Concrete;
using Entities.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Concrete
{
    public class SurveyFilterManager : ISurveyFilterService
    {
        private readonly ISurveyFilterDal _surveyFilterDal;
        public SurveyFilterManager(ISurveyFilterDal surveyFilterDal)
        {
            _surveyFilterDal = surveyFilterDal;
        }

        public IResult Add(SurveyFilter surveyFilter)
        {
            throw new NotImplementedException();
        }

        public IDataResult<SurveyFilter> GetBySurveyId(string surveyId)
        {
            var data = _surveyFilterDal.Get(sf => sf.SurveyId == surveyId);
            if (data==null)
            {
                return new ErrorDataResult<SurveyFilter>();  
            }
            return new SuccessDataResult<SurveyFilter>(data);
        }

        public IResult Update(SurveyFilter surveyFilter)
        {
            var result = GetBySurveyId(surveyFilter.SurveyId);
            if (result.Success)
            {
                var id = result.Data.Id;
                SurveyFilter updatedSurveyFilter = new SurveyFilter()
                {
                    Id = id,
                    SurveyId = surveyFilter.SurveyId,
                    MaxAge = surveyFilter.MaxAge,
                    MinAge = surveyFilter.MinAge,
                    GenderId = surveyFilter.GenderId
                };
                _surveyFilterDal.Update(updatedSurveyFilter);
                return new SuccessResult();
            }
            _surveyFilterDal.Add(surveyFilter);
            return new SuccessResult();
        }
    }
}
