using System.Collections.Generic;

namespace Internship_Application.Models
{
    public class FormViewModel
    {

        public List<FormJsonModel> StudentQuestions { get; set; }
        public List<FormJsonModel> EmployerQuestions { get; set; }
        public List<FormJsonModel> FacultyQuestions { get; set; }
        public List<FormJsonModel> StudentServicesQuestions { get; set; }
        public List<FormJsonModel> AdministratorQuestions { get; set; }
        public List<Forms> Forms { get; set; }
        public TemplateViewModel Template { get; set; }

    }
}
