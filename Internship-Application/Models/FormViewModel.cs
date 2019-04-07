using System.Collections.Generic;

namespace Internship_Application.Models
{
    public class FormViewModel
    {

        //public List<FormJsonModel> Questions { get; set; }
        //public List<Forms> Forms { get; set; }
        //public List<JsonModel> Template { get; set; }
        public List<FormJsonModel> Form { get; set; }
        public string FormName { get; set; }
        public string Disclaimer { get; set; }
    }
}
