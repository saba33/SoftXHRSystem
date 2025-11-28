using HRSystem.Domain.Common;
using HRSystem.Domain.Entities.UserEntity;
using HRSystem.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace HRSystem.Domain.Entities
{
    public class Employee : BaseEntity
    {
        public string PersonalNumber { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public Gender Gender { get; set; }
        public DateTime BirthDate { get; set; }

        public string Email { get; set; }
        public int PositionId { get; set; }
        public Position Position { get; set; }

        public EmployeeStatus Status { get; set; }
        public DateTime? TerminationDate { get; set; }
        public DateTime? ActivatedAt { get; set; }
        public int? CreatedByUserId { get; set; }
        public User CreatedByUser { get; set; }

        public int? UpdatedByUserId { get; set; }
        public User UpdatedByUser { get; set; }
    }
}
