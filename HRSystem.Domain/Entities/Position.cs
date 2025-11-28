using HRSystem.Domain.Common;
using System;
using System.Collections.Generic;
using System.Text;

namespace HRSystem.Domain.Entities
{
    public class Position : BaseEntity
    {
        public string Name { get; set; }

        public int? ParentId { get; set; }
        public Position Parent { get; set; }

        public ICollection<Position> Children { get; set; } = new List<Position>();

        public ICollection<Employee> Employees { get; set; } = new List<Employee>();
    }
}
