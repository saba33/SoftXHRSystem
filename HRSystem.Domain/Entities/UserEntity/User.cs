using HRSystem.Domain.Common;
using HRSystem.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace HRSystem.Domain.Entities.UserEntity
{
    public class User : BaseEntity
    {
        public string PersonalNumber { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public Gender Gender { get; set; }
        public DateTime BirthDate { get; set; }

        public string Username { get; set; }
        public string Email { get; set; }
        public string PasswordHash { get; set; }
        public string PasswordSalt { get; set; }
        public bool IsActive { get; set; } = true;
        public ICollection<Employee> CreatedEmployees { get; set; } = new List<Employee>();
        public ICollection<Employee> UpdatedEmployees { get; set; } = new List<Employee>();
    }
}
