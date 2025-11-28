using System;
using System.Collections.Generic;
using System.Text;

namespace HRSystem.Application.DTOs.Auth.Requsets
{
    public class LoginRequest
    {
        public string Username { get; set; }
        public string Password { get; set; }
    }
}
