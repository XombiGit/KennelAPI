using System;
using System.Collections.Generic;
using System.Text;

namespace Common.Entities
{
    public interface IUserEntity
    {
        string UserID { get; set; }
        string UserName { get; set; }
        string Password { get; set; }
        string Token { get; set; }

        IUserEntity Clone();
    }
}
