using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace Internship_Application.Models
{
    public class StatusCodesViewModel
    {
        public int Id { get; set; }

        [DisplayName("Status Code")]
        public string StatusCode { get; set; }

        public string Details { get; set; }
    }
}
