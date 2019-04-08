using System;
using System.Collections.Generic;

namespace Internship_Application.Models
{
    public class Questions
    {
        public string Order { get; set; }
        public string Prompt { get; set; }
        public string InputType { get; set; }
        public string HelperText { get; set; }
        public List<string> Options { get; set; }
        public string DateSigned { get; set; }
        public Boolean Signed { get; set; }
        public Boolean Required { get; set; }
        public Boolean ProcessQuestion { get; set; }
        public string Role { get; set; }
    }
}
