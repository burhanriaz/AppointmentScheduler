using Mailjet.Client;
using Mailjet.Client.Resources;
using Microsoft.AspNetCore.Identity.UI.Services;
using Newtonsoft.Json.Linq;
using System;
using System.Threading.Tasks;

namespace AppointmentScheduling.Helper
{
    public class EmailSender : IEmailSender

    {
        public async Task SendEmailAsync(string email, string subject, string htmlMessage)
        {
            MailjetClient client = new MailjetClient("f81c7d4009d2dbf7c01af35e0c4ac0eb", "da41b043d42886977f93e3dd7646b007");
            {
                //  Version = ApiVersion.V3_1,
            };
            MailjetRequest request = new MailjetRequest
            {
                Resource = Send.Resource,
            }
   .Property(Send.FromEmail, "jmm.burhanriaz@gmail.com")
   .Property(Send.FromName, "Appointment Scheduler")
   .Property(Send.Subject, subject)
   .Property(Send.HtmlPart, htmlMessage)
   .Property(Send.Recipients, new JArray {
                new JObject {
                 {"Email", email }
                 }
       });
           
            MailjetResponse response = await client.PostAsync(request);
        }
    }
}
