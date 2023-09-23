using Business.ThirdPartyServices.MessageBrokerServices;
using Core.Entities;
using Entities.Concrete;
using Entities.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.ThirdPartyServices.MessageBrokerServices.NewFolder
{
    public class Deneme<T> : IMessageBrokerService<T> 
        where T : class, IDto, new()
    {

        public void AddQuee(string queueName, T messageDto)
        {
            throw new NotImplementedException("Deneme");
        }
    }
}
