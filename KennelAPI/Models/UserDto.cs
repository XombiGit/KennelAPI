using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace KennelAPI.Models
{
    public class UserDto
    {
        public string Name { get; set; }
        public int UserID { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }

        public UserDto Clone()
        {
            var copy = (UserDto)this.MemberwiseClone();
            return copy;
        }
    }
}
