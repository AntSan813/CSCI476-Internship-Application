using System.Collections.Generic;

namespace Internship_Application.Models
{
    public class TemplateViewModel
    {

        public List<JsonModel> StudentQuestions { get; set; }
        public List<JsonModel> EmployerQuestions { get; set; }
        public List<JsonModel> FacultyQuestions { get; set; }
        public List<JsonModel> StudentServicesQuestions { get; set; }
        public List<JsonModel> AdministratorQuestions { get; set; }
        public Templates Template { get; set; }

    }
}
