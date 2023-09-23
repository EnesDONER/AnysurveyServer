using Entities.Dtos;
using Newtonsoft.Json;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Net.Mail;
using System.Net;
using System.Text;
using Core.Utilities.Results;

try
{
    IConnectionFactory factory;

    factory = new ConnectionFactory()
    {
        HostName = "localhost",
        Port = 5672,
        UserName = "guest",
        Password = "guest"
    };
    using var connection = factory.CreateConnection();

    using (var channel = connection.CreateModel())
    {
        string queueName = "Email";
        List<string> result = new List<string>();

        // Kuyruğu tanımla (mesajları almak ve göndermek için aynı isimle olmalıdır)
        channel.QueueDeclare(queue: queueName,
                             durable: false,
                             exclusive: false,
                             autoDelete: false,
                             arguments: null);

        // Mesajları almak için bir tüketici oluştur
        var consumer = new EventingBasicConsumer(channel);


        // Tüketiciyi kuyruğa bağla
        channel.BasicConsume(queue: queueName,
                             autoAck: true,
                             consumer: consumer);

        consumer.Received += Consumer_Received;
        Console.ReadLine();




        static void Consumer_Received(object? sender, BasicDeliverEventArgs e)
        {

            string emailString = Encoding.UTF8.GetString(e.Body.ToArray());
            EmailDto? emailDto = JsonConvert.DeserializeObject<EmailDto>(emailString);
            if (emailDto != null)
            {
                SendMail(emailDto);

            }
        }
        static void SendMail(EmailDto emailDto)
        {
            using (MailMessage mail = new MailMessage())
            {
                mail.From = new MailAddress("seenbilgi@outlook.com");
                mail.To.Add(emailDto.ConsumerUserEmail);
                mail.Subject = emailDto.Subject;
                mail.Body = emailDto.Body;
                mail.IsBodyHtml = true;

                using (SmtpClient smtp = new SmtpClient("smtp.office365.com", 587))
                {
                    smtp.Credentials = new NetworkCredential("seenbilgi@outlook.com", "123456789seen");
                    smtp.EnableSsl = true;
                    smtp.Send(mail);
                }
            }
        }
    }

}
catch (Exception ex)
{

   new ErrorResult(ex.Message);
}


