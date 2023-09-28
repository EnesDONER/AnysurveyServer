using Business.Abstract;
using Business.Concrete;
using Business.ThirdPartyServices.MessageBrokerServices;
using Business.ThirdPartyServices.MessageBrokerServices.RabbitMQ;
using Business.ThirdPartyServices.PaymentServices;
using Business.ThirdPartyServices.PaymentServices.IyziPay;
using Business.ThirdPartyServices.StorageServices;
using Business.ThirdPartyServices.StorageServices.Azure;
using Castle.Core.Configuration;
using Core.DataAccess.MongoOptions;
using Core.Utilities.Results;
using DataAccess.Abstract;
using DataAccess.Concrete.EntityFramework;
using DataAccess.Concrete.MongoDB;
using Entities.Dtos;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using NUnit.Framework;

namespace TestProject
{
    public class Tests
    {
        [SetUp]
        public void Setup()
        {
        }
    }
}