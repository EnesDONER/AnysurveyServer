using Core.Utilities.Results;
using Entities.Concrete;
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
        IDataResult<Survey> GetById(string Id);
        IDataResult<List<Survey>> GetAll();
    }
}
