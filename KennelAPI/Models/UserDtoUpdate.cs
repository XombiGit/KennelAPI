﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace KennelAPI.Models
{
    public class UserDtoUpdate
    {
        public string UserName { get; set; }
        public string Password { get; set; }
    }
}
