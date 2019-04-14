using System;

namespace Internship_Application.Models
{
    public class InputViewModel
    {
        public int DisplayOrder { get; set; }
        public int Order { get; set; }
        public string Prompt { get; set; }
        public string InputType { get; set; }
        public string HelperText { get; set; }
        public string Options { get; set; }
        public Boolean Signed { get; set; }
        public Boolean Required { get; set; }
        public Boolean ProcessQuestion { get; set; }
        public string Role { get; set; }
        public string Value { get; set; }
        public string DateSigned { get; set; }
        public bool isRequired { get; set; }
        public bool isDisabled { get; set; }
    }
}
