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

namespace Business.Concrete
{
    public class ContactManager : IContactService
    {
        public IResult SendMessage(Contact contact)
        {
            using (MailMessage mail = new MailMessage())
            {
                mail.From = new MailAddress("seenbilgi@outlook.com");
                mail.To.Add("seenbilgi@outlook.com");
                mail.Subject = contact.Subject;
                mail.Body = "His mail: "+contact.Email+ " \n His name: " + contact.Name + "\n His message: " +contact.Message;
                mail.IsBodyHtml = true;

                using (SmtpClient smtp = new SmtpClient("smtp.office365.com", 587))
                {
                    smtp.Credentials = new NetworkCredential("seenbilgi@outlook.com", "123456789seen");
                    smtp.EnableSsl = true;
                    smtp.Send(mail);
                }
            }
            return new SuccessResult("Mail sended");
        }
    }
}
