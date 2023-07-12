using Business.Abstract;
using Business.ValidationRules;
using Core.Aspects.Autofac.Caching;
using Core.Aspects.Autofac.Validation;
using Core.Utilities.Business;
using Core.Utilities.Results;
using DataAccess.Abstract;
using Entities.Concrete;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Concrete
{
    public class CardManager : ICardService
    {
        private ICardDal _cardDal;
        public CardManager(ICardDal cardDal)
        {
            _cardDal = cardDal;
        }

        [CacheRemoveAspect("ICardDal.Get")]
        //[ValidationAspect(typeof(CardValidator))]
        public IResult Add(Card card)
        {
            _cardDal.Add(card);
            return new SuccessResult();
        }

        public IDataResult<List<Card>> GetAllCardByUserId(int userId)
        {
            return new SuccessDataResult<List<Card>>(_cardDal.GetAll(c=>c.UserId==userId));
        }

        //[CacheAspect(10)]
        //public IDataResult<List<Card>> GetAll()
        //{
        //    return new SuccessDataResult<List<Card>>(_cardDal.GetAll());
        //}


        public IDataResult<Card> GetById(int id)
        {
            return new SuccessDataResult<Card>(_cardDal.Get(c => c.Id == id));
        }
    }
}
