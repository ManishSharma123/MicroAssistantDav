using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace MicroAssistant.Lib
{
    public class AlertService
    {

        public void SendMail(string subject, string[] receipents, string message)
        {

            SmtpClient client = new SmtpClient();
            client.Host = "mail.mydomain.com";
            client.Host = "relay-hosting.secureserver.net";
            client.Port = 25;
            client.UseDefaultCredentials = false;
            client.EnableSsl = false;

                        
           MailMessage msg = new MailMessage();

            MailAddress strfrom = new MailAddress("activecraft.naresh@yahoo.com");
            if (receipents.Length > 0)
            {
                foreach (string email in receipents)
                {
                    if (email != null)
                    {
                        msg.To.Add(new MailAddress(email));
                    }
                }
                if (msg.To.Count > 0)
                {
                    msg.From = strfrom;
                    msg.Subject = subject;
                    msg.IsBodyHtml = true;
                    msg.Body = message;

                    client.Send(msg);
                }
            }
          
        }

    }
}
