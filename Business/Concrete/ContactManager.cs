using Business.Abstract;
using Core.Utilities.Results;
using Entities.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Business.ThirdPartyServices.MessageBrokerServices;
using Entities.Concrete;
using MongoDB.Bson;

namespace Business.Concrete
{
    public class ContactManager : IContactService
    {
        private readonly IMessageBrokerService<EmailDto> _messageBrokerService;

        public ContactManager(IMessageBrokerService<EmailDto> messageBrokerService)
        {
            _messageBrokerService = messageBrokerService;
        }

        public IResult SendMessage(Contact contact)
        {
            IResult result = sendMessageRabbitMQ(contact);
            if (!result.Success)
            {
                return new ErrorResult(result.Message);
            }

            return new SuccessResult(result.Message);
        }

        private IResult sendMessageRabbitMQ(Contact contact)
        {

            EmailDto email = new()
            {
                ConsumerUserEmail = "seenbilgi@outlook.com",
                Body = "His mail: " + contact.Email + " \n His name: " + contact.Name + "\n His message: " + contact.Message,
                Subject = contact.Subject
            };

            try
            {
                _messageBrokerService.AddQuee(queueName: "Email", email);

            }
            catch (Exception ex)
            {

                return new ErrorResult(ex.Message);
            }
            return new SuccessResult("Message sended");
        }
    }
}
