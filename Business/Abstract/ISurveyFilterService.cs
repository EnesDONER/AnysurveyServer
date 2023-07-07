using Core.Utilities.Results;
using Entities.Concrete;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Abstract
{
    public interface ISurveyFilterService
    {
        IResult Add(SurveyFilter surveyFilter);
        IResult Update(SurveyFilter surveyFilter);
        IDataResult<SurveyFilter> GetBySurveyId(string surveyId);
    }
}
