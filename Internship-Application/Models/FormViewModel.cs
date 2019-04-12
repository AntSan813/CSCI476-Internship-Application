using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace Internship_Application.Models
{
    public class FormViewModel
    {

        //public List<FormJsonModel> Questions { get; set; }
        //public List<Forms> Forms { get; set; }
        //public List<JsonModel> Template { get; set; }
        public int Id { get; set; }
        //public List<FormJsonModel> Form { get; set; }
        public string FormName { get; set; }
        public string Disclaimer { get; set; }

        [DisplayName("Student Name")]
        public string StudentName { get; set; }

        [DisplayName("Student Email")]
        public string StudentEmail { get; set; }

        [DisplayName("Faculty Email")]
        public string FacultyEmail { get; set; }

        [DisplayName("Employer Email")]
        public string EmployerEmail { get; set; }

        [DisplayName("Last Updated")]
        public DateTime UpdatedAt { get; set; }

        public StatusCodes StatusCodesViewModel { get; set; }
        public List<TemplateViewModel> TemplateViewModels { get; set; }
    }
}
