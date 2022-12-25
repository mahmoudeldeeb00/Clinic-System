﻿using Clinic_System.DTOS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Clinic_System.BL.IRepsitory
{
    public interface IIdentityService
    {

         Task<AuthenticationModel> RegisterAsync(RegisterDTO model);
         Task<AuthenticationModel> LoginAsync(LoginDTO model);
    }
}
