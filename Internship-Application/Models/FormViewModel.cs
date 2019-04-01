using System.Collections.Generic;

namespace Internship_Application.Models
{
    public class FormViewModel
    {

        public List<JsonModel> StudentQuestions { get; set; }
        public List<JsonModel> EmployerQuestions { get; set; }
        public List<JsonModel> FacultyQuestions { get; set; }
        public List<JsonModel> StudentServicesQuestions { get; set; }
        public List<JsonModel> AdministratorQuestions { get; set; }
        public List<Forms> Forms { get; set; }

    }
}
