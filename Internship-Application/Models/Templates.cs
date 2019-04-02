using System;
using System.Collections.Generic;

namespace Internship_Application.Models
{
    public partial class Templates
    {
        public int Id { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public DateTime? RetiredAt { get; set; }
        public string TemplateName { get; set; }
        public string DisplayName { get; set; }
        public string Disclaimer { get; set; }
        public string Questions { get; set; }
        public bool IsActive { get; set; }
        public bool IsRetired { get; set; }
    }
}
