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
        public string Name { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }

        public IUserEntity Clone()
        {
            var copy = (InMemoryUserEntity)this.MemberwiseClone();
            return copy;
        }
    }
}
