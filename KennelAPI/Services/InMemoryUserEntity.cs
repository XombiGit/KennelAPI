using Common.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace KennelAPI.Services
{
    public class InMemoryUserEntity : IUserEntity
    {
        public string UserID { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public string Token { get; set; }

        public IUserEntity Clone()
        {
            var copy = (InMemoryUserEntity)this.MemberwiseClone();
            return copy;
        }
    }
}
