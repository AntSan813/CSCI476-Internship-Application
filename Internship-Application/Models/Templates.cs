using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Internship_Application.Models
{
    public partial class Templates
    {
        public int Id { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public DateTime? DeletedAt { get; set; }
        [Display(Name = "Form Title")]
        [Required]
        public string FormTitle { get; set; }

        [Required]
        [Display(Name = "Disclaimer")]
        public string Disclaimer { get; set; }

        [Required]
        [Display(Name = "Template Name")]
        public string TemplateName { get; set; }

        [Display(Name = "Student Section")]
        public string StudentQuestions { get; set; }

        [Display(Name = "Employer Section")]
        public string EmployerQuestions { get; set; }

        [Display(Name = "Faculty Section")]
        public string FacultyQuestions { get; set; }

        [Display(Name = "Student Services Section")]
        public string StudentServicesQuestions { get; set; }

        [Display(Name = "Administrator Section")]
        public string AdministratorQuestions { get; set; }

        [Required]
        [Display(Name = "Is Active")]
        public bool IsActive { get; set; }

        [Required]
        public bool? IsModifiable { get; set; }
    }
}
