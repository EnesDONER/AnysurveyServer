using Azure;
using Business.Abstract;
using Business.BusinessAspects.Autofac;
using Business.Constants;
using Business.ThirdPartyServices.MessageBrokerServices;
using Business.ThirdPartyServices.PaymentServices;
using Business.ThirdPartyServices.StorageServices;
using Core.Aspects.Autofac.Caching;
using Core.Entities.Concrete;
using Core.Utilities.Business;
using Core.Utilities.Results;
using DataAccess.Abstract;
using DataAccess.Concrete.MongoDB;
using Entities.Concrete;
using Entities.Dtos;
using log4net.Core;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Microsoft.VisualBasic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZstdSharp.Unsafe;

namespace Business.Concrete
{
    public class AdManager : IAdService
    {
        private readonly IAdDal _addDal;
        private readonly IAdFilterService _adFilterService;
        private readonly IUserService _userService;
        private readonly IWatchedAdDal _watchedAdDal;
        private readonly IMessageBrokerService<EmailDto> _messageBrokerService;
        private readonly IStorageService _storageService;
        public AdManager(IAdDal addDal, IWatchedAdDal watchedAdDal, IAdFilterService adFilterService, IUserService userService, 
            IMessageBrokerService<EmailDto> messageBrokerService, IStorageService storageService)
        {
            _addDal = addDal;
            _watchedAdDal = watchedAdDal;
            _adFilterService = adFilterService;
            _userService = userService;
            _messageBrokerService = messageBrokerService;
            _storageService = storageService;
        }

        [CacheRemoveAspect("IAdService.Get")]
        [SecuredOperation("partnership")]
        public IResult Add(Ad ad)
        {
            _addDal.Add(ad);

            return new SuccessResult(Messages.Added);
        }

        [CacheRemoveAspect("IAdService.Get")]
        [SecuredOperation("partnership")]
        public async Task<IResult> AddAdandUploadAsync(AdUploadDto adUploadDto)
        {
            var result = BusinessRules.Run(checkIfFileTypeIsVideo(adUploadDto.FormFile));
            if (result != null)
            {
                return new ErrorResult(result.Message);
            }
            Ad ad = new()
            {
                CompanyName = adUploadDto.CompanyName,
                Description = adUploadDto.Description,
                OwnerUserId = adUploadDto.OwnerUserId
            };

            var content = await _storageService.UploadSingleFileAsync(adUploadDto.ContainerName,adUploadDto.OwnerUserId.ToString() , adUploadDto.FormFile);
            ad.VideoURL = content.PathOrContainerName;
            Add(ad);
            
            return new SuccessResult(Messages.Added);
        }
        private IResult checkIfFileTypeIsVideo(IFormFile file)
        {
            if (file != null && Path.GetExtension(file.FileName).Equals(".mp4", StringComparison.OrdinalIgnoreCase))
            {
                return new SuccessResult();
            }
            else
            {
                return new ErrorResult("This file is not a video");
            }
        }
        public IDataResult<List<Ad>> GetAllFilteredAdByUserId(int userId)
        {
            var data = _addDal.GetAll();
            var result = ApplyFilter(data, userId);
            if (result.Success)
            {
                return new SuccessDataResult<List<Ad>>(result.Data);
            }
            return new ErrorDataResult<List<Ad>>();

        }

        [CacheAspect(10)]
        public IDataResult<List<UserForWatchedOrSolvedContent>> GetAllUsersWhoWatchedAdsByAdId(string adId)
        {
            var ad = GetById(adId);
            if (!ad.Success)
            {
                return new ErrorDataResult<List<UserForWatchedOrSolvedContent>>("Ad not found");
            }
            var watchedAds = GetAllWatchedAd().Data;
            List<UserForWatchedOrSolvedContent> result = new List<UserForWatchedOrSolvedContent>();
            foreach (var watchedAd in watchedAds)
            {
                if (ad.Data.Id == watchedAd.AdId)
                {
                    User user = _userService.GetById(watchedAd.UserId).Data;
                    UserForWatchedOrSolvedContent userForWhoWatchedAds = new UserForWatchedOrSolvedContent
                    {
                        Email = user.Email,
                        FirstName = user.FirstName,
                        LastName = user.LastName,
                        Age = DateTime.Now.Year - user.BirthDay.Year,
                        Gender = user.GenderId == 1 ? "Men" : "Women"
                        
                    };
                    result.Add(userForWhoWatchedAds);
                }
            }

            return new SuccessDataResult<List<UserForWatchedOrSolvedContent>>(result);
        }

        [CacheAspect(10)]
        public IDataResult<List<Ad>> GetAllUnWatchedAd(int userId)
        {
            return new SuccessDataResult<List<Ad>>(GetAllFilteredAdByUserId(userId).Data.Where(ad => !GetAllWatchedAdByUserId(userId).Data.Any(watchedAd => watchedAd.AdId == ad.Id)).ToList());
        }
        [CacheAspect(10)]
        public IDataResult<List<Ad>> GetAllAdsByOwnerUserId(int userId)
        {
            return new SuccessDataResult<List<Ad>>(_addDal.GetAll(a=>a.OwnerUserId==userId));

        }

        public IDataResult<Ad> GetById(string id)
        {
            return new SuccessDataResult<Ad>(_addDal.Get(a => a.Id == id));
        }
        private IDataResult<List<Ad>> ApplyFilter(List<Ad> ads, int userId)
        {
            List<Ad> inValidAds = new List<Ad>();
            foreach (var ad in ads)
            {
                var filter = _adFilterService.GetByAdId(ad.Id);
                if (filter.Success)
                {
                    var user = _userService.GetById(userId).Data;
                    if (filter.Data.MinAge > (DateTime.Now.Year - user.BirthDay.Year) ||
                        filter.Data.MaxAge < (DateTime.Now.Year - user.BirthDay.Year) ||
                        filter.Data.GenderId != user.GenderId
                        )
                    {
                        inValidAds.Add(ad);
                    }
                }
            } 
            return new SuccessDataResult<List<Ad>>(ads.Except(inValidAds).ToList());
        }

        [CacheRemoveAspect("IWatcedAdService.Get")]
        [CacheRemoveAspect("IUserService.Get")]
        public IResult AddWatchedAd(WatchedAd watcehedAd)
        {
            _watchedAdDal.Add(watcehedAd);

            IResult result = sendMessageRabbitMQ(watcehedAd);
            if (!result.Success)
            {
                return new ErrorResult(result.Message);
            }

            return new SuccessResult(Messages.Added);
        }

        private IResult sendMessageRabbitMQ(WatchedAd watcehedAd)
        {
            var senderUser = _userService.GetById(watcehedAd.UserId).Data; // reklamı izleyen kişi
            var addedAd = GetById(watcehedAd.AdId).Data;
            var ownerUser = addedAd.OwnerUserId; //reklamın sahibi
            var senderUserName = senderUser.FirstName + senderUser.LastName;

            EmailDto email = new()
            {
                ConsumerUserEmail = _userService.GetById(ownerUser).Data.Email,
                Body = $" {senderUserName} watched your \"{addedAd.Description}\" video",
                Subject = "Your Ad video Watched"
            };

            try
            {
                _messageBrokerService.AddQuee(queueName: "Email", email);

            }
            catch (Exception)
            {

                return new ErrorResult("RabbitMQ connecting is failed");
            }
            return new SuccessResult("Message sended");
        }

        [CacheRemoveAspect("IWatcedAdService.Get")]
        public IDataResult<List<WatchedAd>> GetAllWatchedAd()
        {
            return new SuccessDataResult<List<WatchedAd>>(_watchedAdDal.GetAll());
        }

        [CacheRemoveAspect("IWatcedAdService.Get")]
        public IDataResult<List<WatchedAd>> GetAllWatchedAdByUserId(int userId)
        {
            return new SuccessDataResult<List<WatchedAd>>(_watchedAdDal.GetAll(wa => wa.UserId == userId));

        }

        public async Task<List<FileUploadResponseDto>> Upload(string containerName, string id, IFormFileCollection files)
        {
            var result=  await _storageService.UploadAsync(containerName,id, files);
            return result;
        }

        public IDataResult<List<string>> GetAllFile(string containerName)
        {
            var result = _storageService.GetFiles(containerName);
            return new SuccessDataResult<List<string>>(result);
        }

        public IResult HasFile(string containerName, string fileName)
        {
            var result=  _storageService.HasFile(containerName, fileName);
            if (result)
            {
                return new SuccessResult();
            }
            return new ErrorResult("File not found");

        }
    }
}
