using System;
using System.Collections.Generic;

namespace Internship_Application.Models
{
    public partial class Account
    {
        public int Id { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public DateTime? DeletedAt { get; set; }
        public string Username { get; set; }
        public string Passwrd { get; set; }
        public string Email { get; set; }
    }
}
