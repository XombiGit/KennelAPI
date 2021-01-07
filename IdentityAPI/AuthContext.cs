﻿using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IdentityAPI
{
    public class AuthContext : IdentityDbContext<IdentityUser>
    {
        public AuthContext()
        {

        }
    }
}
