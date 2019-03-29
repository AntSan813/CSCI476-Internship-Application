namespace Internship_Application.Models
{
    public class JsonModel
    {

        public string Prompt { get; set; }
        public string InputType { get; set; }
        public string HelperText { get; set; }
        public string DatedSigned { get; set; }
        public string[] Options { get; set; }
        public int Order { get; set; }
        public bool Signed { get; set; }

    }
}
