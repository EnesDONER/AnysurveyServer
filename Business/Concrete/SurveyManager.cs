using Business.Abstract;
using Business.BusinessAspects.Autofac;
using Core.Aspects.Autofac.Caching;
using Core.Utilities.Results;
using DataAccess.Abstract;
using Entities.Concrete;

namespace Business.Concrete
{
    public class SurveyManager : ISurveyService
    {
        private readonly ISurveyDal _surveydal;
        public SurveyManager(ISurveyDal surveydal)
        {
            _surveydal = surveydal;
        }
        
        [CacheRemoveAspect("ISurveyService.Get")]
        public IResult Add(Survey survey)
        {
            _surveydal.Add(survey);
            return new SuccessResult();
        }
        
        [SecuredOperation("admin")]
        [CacheAspect(duration: 20)]
        public IDataResult<List<Survey>> GetAll()
        {
            return new SuccessDataResult<List<Survey>>(_surveydal.GetAll());
        }
      
        [CacheRemoveAspect("ISurveyService.Get")]
        public IResult Delete(Survey survey)
        {
            _surveydal.Delete(survey);
            return new SuccessResult();
        }
        [CacheRemoveAspect("ISurveyService.Get")]
        public IResult Update(Survey survey)
        {
           _surveydal.Update(survey);
            return new SuccessResult();
        }
        [CacheAspect(duration: 2)]
        public IDataResult<Survey> GetById(string Id)
        {
            return new SuccessDataResult<Survey>(_surveydal.Get(s => s.Id == Id));
        }
    }
}
