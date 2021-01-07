using Common.Interfaces.Services;
using KennelAPI.Services;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace Tests
{
    public class MailTests
    {
        [Fact(Skip ="Not necessary")]
        public void SendMail_SubjectAndMessageExists_ReturnOK()
        {
            IMailService mailService = new EmailService();
            mailService.SendMail("hello", "world");
        }
    }
}
