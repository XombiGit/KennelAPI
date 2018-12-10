using System;
using System.Collections.Generic;
using System.Text;

namespace Common.Interfaces.Services
{
    public interface IMailService
    {
        void SendMail(string subject, string message);
    }
}
