using Common.Interfaces.Services;
using EASendMail;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Mail;
using System.Threading.Tasks;

namespace KennelAPI.Services
{
    public class EmailService : IMailService
    {
        private string mailTo = "amadeuscho@hotmail.com";
        private string mailFrom = "amadeuscho@hotmail.com";

        public EmailService()
        {

        }

        public void SendMail(string subject, string message)
        {
            System.Net.Mail.SmtpClient mailer = new System.Net.Mail.SmtpClient("smtp.live.com")
            {
                Port = 587
            };

            // set smtp-client with basicAuthentication
            var basicAuthenticationInfo = new System.Net.NetworkCredential("amadeuscho@hotmail.com", "d3lphyn3");
            mailer.Credentials = basicAuthenticationInfo;
            mailer.EnableSsl = true;

            var from = new System.Net.Mail.MailAddress("amadeuscho@hotmail.com", "TestFromName");
            var to = new System.Net.Mail.MailAddress("lausdeo1981@gmail.com");
            MailMessage myMail = new System.Net.Mail.MailMessage(from, to);

            myMail.Subject = "Deleted";
            myMail.SubjectEncoding = System.Text.Encoding.UTF8;

            myMail.DeliveryNotificationOptions = System.Net.Mail.DeliveryNotificationOptions.OnSuccess 
                | System.Net.Mail.DeliveryNotificationOptions.OnFailure;


            mailer.Send(myMail);
            Debug.WriteLine($"Mail from {mailFrom} to {mailTo}");
        }

        public void SendMailEaSender(string subject, string message)
        {
            SmtpMail oMail = new SmtpMail("TryIt");
            EASendMail.SmtpClient oSmtp = new EASendMail.SmtpClient();

            // Set sender email address, please change it to yours
            oMail.From = "test@emailarchitect.net";

            // Set recipient email address, please change it to yours
            oMail.To = "support@emailarchitect.net";

            // Set email subject
            oMail.Subject = "test email from c#, read receipt required";

            // Set email body
            oMail.TextBody = "this is a test email sent from c# project, do not reply";

            // Your SMTP server address
            SmtpServer oServer = new SmtpServer("smtp.emailarchitect.net");

            // User and password for ESMTP authentication, if your server doesn't require
            // User authentication, please remove the following codes.
            oServer.User = "test@emailarchitect.net";
            oServer.Password = "testpassword";

            // If your smtp server requires SSL connection, please add this line
            // oServer.ConnectType = SmtpConnectType.ConnectSSLAuto;

            // Request read receipt
            oMail.ReadReceipt = true;

            // Request both failure and success report
            oMail.DeliveryNotification = EASendMail.DeliveryNotificationOptions.OnFailure |
                EASendMail.DeliveryNotificationOptions.OnSuccess;

            try
            {
                Console.WriteLine("start to send email ...");
                oSmtp.SendMail(oServer, oMail);
                Console.WriteLine("email was sent successfully!");
            }
            catch (Exception ep)
            {
                Console.WriteLine("failed to send email with the following error:");
                Console.WriteLine(ep.Message);
            }
        }
    }
}
