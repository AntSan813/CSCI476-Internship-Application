using System;
using System.Collections.Generic;

namespace Internship_Application.Models
{
    public partial class Form
    {
        public int Id { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public DateTime? DeletedAt { get; set; }
        public string StudentId { get; set; }
        public string FacultyId { get; set; }
        public string EmployerId { get; set; }
        public int StudentDetailsId { get; set; }
        public int EmployerDetailsId { get; set; }
        public int OfficeDetailsId { get; set; }
        public int SignaturesId { get; set; }
    }
}
