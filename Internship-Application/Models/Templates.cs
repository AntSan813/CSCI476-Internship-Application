using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;  //needed for Display annotation
using System.ComponentModel;

namespace Internship_Application.Models
{
    public partial class Templates
    {
        public int Id { get; set; }

        [Display(Name = "Created")]
        public DateTime CreatedAt { get; set; }

        [Display(Name = "Last Updated")]
        public DateTime UpdatedAt { get; set; }

        [Display(Name = "Retired")]
        public DateTime? RetiredAt { get; set; }

        [Display(Name = "Template Name")]
        public string TemplateName { get; set; }

        [Display(Name = "Template Display Name")]
        public string DisplayName { get; set; }

        public string Disclaimer { get; set; }
        public string Questions { get; set; }

        [Display(Name = "Active")]
        public bool IsActive { get; set; }

        [Display(Name = "Retired")]
        public bool IsRetired { get; set; }
    }
}
