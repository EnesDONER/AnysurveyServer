using Business.Abstract;
using Business.Concrete;
using Business.ThirdPartyServices.MessageBrokerServices.RabbitMQ;
using Business.ThirdPartyServices.MessageBrokerServices;
using Business.ThirdPartyServices.PaymentServices;
using Business.ThirdPartyServices.StorageServices;
using Core.Utilities.Security.JWT;
using DataAccess.Abstract;
using DataAccess.Concrete.EntityFramework;
using DataAccess.Concrete.MongoDB;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.IdentityModel.Tokens;
using Entities.Dtos;
using Core.CrossCuttingConcerns.Caching.Microsoft;
using Core.CrossCuttingConcerns.Caching;
using Microsoft.AspNetCore.Http;
using System.Diagnostics;
using Castle.DynamicProxy;
using Core.Utilities.Interceptors;
using System.Reflection;
using Autofac.Core;
using Microsoft.EntityFrameworkCore.Diagnostics;

namespace Business.DependencyResolvers
{
    public static class ServiceRegistration
    {
        public static void Load(this IServiceCollection serviceCollection)
        {
            serviceCollection.AddScoped<IPaymentService,PaymentManager>();
            serviceCollection.AddScoped<IUserService,UserManager>();
            serviceCollection.AddScoped<IUserDal, EfUserDal>();
            serviceCollection.AddScoped<ICardDal, EfCardDal>();
            serviceCollection.AddScoped<ICardService, CardManager>();
            
            serviceCollection.AddScoped<IAdService, AdManager>();
            serviceCollection.AddScoped<IAdDal, MAdDal>();
            serviceCollection.AddScoped<IAdFilterService, AdFilterManager>();
            serviceCollection.AddScoped<IAdFilterDal, MAdFilterDal>();
            serviceCollection.AddScoped<IWatchedAdDal, MWatchedAdDal>();
            serviceCollection.AddScoped<ISurveyService, SurveyManager>();
            serviceCollection.AddScoped<ISurveyDal, MSurveyDal>();
            serviceCollection.AddScoped<ISurveyFilterService, SurveyFilterManager>();
            serviceCollection.AddScoped<ISurveyFilterDal, MSurveyFilterDal>();
            serviceCollection.AddScoped<ISolvedSurveyDal, MSolvedSurveyDal>();
            serviceCollection.AddScoped<ISurveyService, SurveyManager>();
            serviceCollection.AddScoped<ISurveyService, SurveyManager>();
            serviceCollection.AddScoped<IContactService, ContactManager>();
            serviceCollection.AddScoped<IUserOperationClaimService, UserOperationClaimManager>();
            serviceCollection.AddScoped<IUserOperationClaimDal, EfUserOperationClaimDal>();

            //serviceCollection.AddMemoryCache();
            //serviceCollection.AddSingleton<ICacheManager, MemoryCacheManager>();
            //serviceCollection.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            //serviceCollection.AddSingleton<Stopwatch>();


            serviceCollection.AddScoped<IAuthService, AuthManager>();
            serviceCollection.AddScoped<ITokenHelper, JwtHelper>();

            serviceCollection.AddScoped(typeof(IMessageBrokerService<>), typeof(RabbitMQAdapter<>));
        }

        public static void AddStorage<T>(this IServiceCollection serviceCollection) 
            where T : class, IStorageService
        {
            serviceCollection.AddScoped<IStorageService, T>();
        }
        public static void AddPaymentService<T>(this IServiceCollection serviceCollection)
        where T : class, IThirdPartyPaymentService
        {
            serviceCollection.AddScoped<IThirdPartyPaymentService, T>();
        }

    }
}
