using System;
using System.Collections.Generic;
using System.Text;

namespace HRSystem.Application.DTOs.Auth.Responses
{
    public class UserResponse
    {
        public int Id { get; set; }
        public string PersonalNumber { get; set; }
        public string FullName { get; set; }
        public string Email { get; set; }
        public string Username { get; set; }
    }
}
