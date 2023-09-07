using Microsoft.AspNetCore.Identity.UI.Services;
using System.Net;
using System.Net.Mail;

namespace ToDoList.Services
{
    public class EmailSender:IEmailSender
    {
        public Task SendEmailAsync(string email, string subject, string message)
        {
            var mail = "yourtodolist1208@gmail.com";
            var pw = "4A636D071AD3B804275874E52FAB53AF8449";

            var client = new SmtpClient("smtp.elasticemail.com", 2525)
            {
                EnableSsl = true,
                Credentials = new NetworkCredential(mail, pw)
            };

            return client.SendMailAsync(
                new MailMessage(from: mail,
                                to: email,
                                subject,
                                message));


        }
    }
}
