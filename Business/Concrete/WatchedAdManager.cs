using Business.Abstract;
using Business.Constants;
using Business.ThirdPartyServices.MessageBrokerServices;
using Core.Aspects.Autofac.Caching;
using Core.Utilities.Results;
using DataAccess.Abstract;
using Entities.Concrete;
using Entities.Dtos;
using Microsoft.EntityFrameworkCore.Diagnostics;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Concrete
{
    public class WatchedAdManager:IWatchedAdService
    {
        private readonly IWatchedAdDal _watchedAdDal;
        private readonly IMessageBrokerService<EmailDto> _messageBrokerService;
        private readonly IUserService _userService;
        public WatchedAdManager(IWatchedAdDal watchedAdDal, IMessageBrokerService<EmailDto> messageBrokerService, IUserService userService)
        {
            _watchedAdDal = watchedAdDal;
            _messageBrokerService = messageBrokerService;
            _userService = userService;
        }

        [CacheRemoveAspect("IWatcedAdService.Get")]
        [CacheRemoveAspect("IUserService.Get")]
        public IResult Add(WatchedAd watcehedAd)
        {
            _watchedAdDal.Add(watcehedAd);


            var senderUser = _userService.GetById(watcehedAd.UserId);
            var senderUserName = senderUser.Data.FirstName + senderUser.Data.LastName;

            EmailDto email = new()
            {
                ConsumerUserEmail =   " ad id ile onun sahibini bul onun email adresi lazım  ",
                Body = $" {senderUser} watched your {watcehedAd.AdId} video",
                Subject= "Your Ad video Watched"
            };

            try
            {
                _messageBrokerService.AddQuee(queueName: "Email", email);

            }
            catch (Exception)
            {

                throw new Exception("RabbitMQ connecting is failed");
            }

            return new SuccessResult(Messages.Added);
        }
       
        [CacheRemoveAspect("IWatcedAdService.Get")]
        public IDataResult<List<WatchedAd>> GetAll()
        {
            return new SuccessDataResult<List<WatchedAd>>(_watchedAdDal.GetAll());
        }
        
        [CacheRemoveAspect("IWatcedAdService.Get")]
        public IDataResult<List<WatchedAd>> GetAllByUserId(int userId)
        {
            return new SuccessDataResult<List<WatchedAd>>(_watchedAdDal.GetAll(wa=>wa.UserId==userId));

        }
    }
}
