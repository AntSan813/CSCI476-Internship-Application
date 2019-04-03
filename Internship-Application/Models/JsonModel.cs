using System.Collections.Generic;

namespace Internship_Application.Models
{
    public class JsonModel
    {
        public string Prompt { get; set; }

        //can be small-text, large-text, signature, dropdown, ...
        public string InputType { get; set; }
        public string HelperText { get; set; }
        public string DateSigned { get; set; }
        public List<string> Options { get; set; }
        public int Order { get; set; }
        public bool Signed { get; set; }
        public bool Required { get; set; }

        //can be only student, admin, faculty, student services, or employer
        public string Person { get; set; }
    }
}
