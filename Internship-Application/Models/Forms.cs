using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Internship_Application.Models
{
    public partial class Forms
    {
        public int Id { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public DateTime? DeletedAt { get; set; }

        [Display(Name = "Winthrop Id")]
        public string WuId { get; set; }

        public string StudentQuestions { get; set; }
        public string EmployerQuestions { get; set; }
        public string FacultyQuestions { get; set; }
        public string StudentServicesQuestions { get; set; }
        public string AdministratorQuestions { get; set; }
        public int TemplateId { get; set; }
    }
}
