using System;
using System.Collections.Generic;

namespace Internship_Application.Models
{
    public partial class StudentDetails
    {
        public int Id { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public DateTime? DeletedAt { get; set; }
        public string StudentId { get; set; }
        public string Class { get; set; }
        public string SemesterYear { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string GraduationYear { get; set; }
        public string Major { get; set; }
        public bool LeagallyAuth { get; set; }
    }
}
