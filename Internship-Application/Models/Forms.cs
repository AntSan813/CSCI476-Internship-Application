using System;
using System.Collections.Generic;

namespace Internship_Application.Models
{
    public partial class Forms
    {
        public int Id { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public string WuId { get; set; }
        public string StudentEmail { get; set; }
        public string FacultyEmail { get; set; }
        public string EmployerEmail { get; set; }
        public string StudentName { get; set; }
        public string Answers { get; set; }
        public int TemplateId { get; set; }
        public int? CompanyId { get; set; }
        public int StatusCodeId { get; set; }
        public int? Paid { get; set; }
    }
}
