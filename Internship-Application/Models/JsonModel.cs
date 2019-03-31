using System.Collections.Generic;

namespace Internship_Application.Models
{
    public class JsonModel
    {

        public string Prompt { get; set; }
        public string InputType { get; set; }
        public string HelperText { get; set; }
        public string DateSigned { get; set; }
        public List<string> Options { get; set; }
        public int Order { get; set; }
        public bool Signed { get; set; }

    }
}
