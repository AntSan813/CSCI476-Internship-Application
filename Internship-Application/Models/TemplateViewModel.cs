using System.Collections.Generic;

namespace Internship_Application.Models
{
    public class TemplateViewModel
    {

        public List<JsonModel> Questions { get; set; }
        public List<Templates> Templates { get; set; }
        public string TemplateName { get; internal set; }
    }
}
