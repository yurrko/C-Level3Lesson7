using MHser.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace MHser.Domain.Entities
{
    public class User : IPerson
    {
        public int UserId { get; set; }
        public string Name { get; set; }
        [Required]
        [StringLength(255)]
        public string AdName { get; set; }
        public string Position { get; set; }
        public string Company { get; set; }
        public DateTime? LastAccess { get; set; }
        public ICollection<Role> Roles { get; set; }
        public User()
        {
            Roles = new List<Role>();
        }
    }
}