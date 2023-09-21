using Core.Entities;
using RabbitMQ.Client.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.ThirdPartyServices.MessageBrokerServices
{
    public interface IMessageBrokerService<T> where T : class, IDto, new()
    {
        public void AddQuee(string queueName, T messageDto);
    }
}
