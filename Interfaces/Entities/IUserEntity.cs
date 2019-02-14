using System;
using System.Collections.Generic;
using System.Text;

namespace Common.Entities
{
    public interface IUserEntity
    {
        string UserID { get; set; }
        string Name { get; set; }
        string Phone { get; set; }
        string Email { get; set; }

        IUserEntity Clone();
    }
}
