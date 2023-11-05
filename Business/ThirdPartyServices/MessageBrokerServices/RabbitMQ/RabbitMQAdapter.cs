using RabbitMQ.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using RabbitMQ.Client.Events;
using System.Text;
using System.Threading.Tasks;
using Core.Entities;
using Autofac.Core;
using Newtonsoft.Json;

namespace Business.ThirdPartyServices.MessageBrokerServices.RabbitMQ
{
    public class RabbitMQAdapter<T> : IMessageBrokerService<T>
        where T : class, IDto, new()
    {
        private static IConnectionFactory factory;

        public RabbitMQAdapter()
        {
            if (factory == null)
            {
                factory = new ConnectionFactory();
                factory.Uri = new Uri("amqps://yoeopshd:g3ENK9omcMwWqeGY8PsRvF2HXwAj83y5@moose.rmq.cloudamqp.com/yoeopshd");
            }
        }
        public void AddQuee(string queueName, T messageDto)
        {

            using (var connection = factory.CreateConnection())
            using (var channel = connection.CreateModel())
            {

                // Kuyruğu tanımla (mesajları almak ve göndermek için aynı isimle olmalıdır)
                channel.QueueDeclare(queue: queueName,
                                     durable: false,
                                     exclusive: false,
                                     autoDelete: false,
                                     arguments: null);

                // Mesajı gönder
                var body = Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(messageDto));
                channel.BasicPublish(String.Empty, queueName, null, body);

            }

        }
    }
}
