using System;
using System.ComponentModel.DataAnnotations;

namespace Domain.Entities
{
    public class BaseEntity
    {
        public Guid Id { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public DateTime? ModifiedAt { get; set; }
        public bool IsActive { get; set; } = true;
    }
}