using Common.Interfaces.Services;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace KennelAPI.Services
{
    public class EmailService : IMailService
    {
        private string mailTo = "amadeuscho@hotmail.com";
        private string mailFrom = "amadeuscho@hotmail.com";

        public void SendMail(string subject, string message)
        {
            Debug.WriteLine($"Mail from {mailFrom} to {mailTo}");
        }
    }
}
