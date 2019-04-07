using System.Collections.Generic;

namespace Internship_Application.Models
{
    public class FormViewModel
    {

        public List<FormJsonModel> Questions { get; set; }
        public List<Forms> Forms { get; set; }
        public TemplateViewModel Template { get; set; }

    }
}
