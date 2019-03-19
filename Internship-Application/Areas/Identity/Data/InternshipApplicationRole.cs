using System;
using Microsoft.AspNetCore.Identity;
namespace Internship_Application.Areas.Identity.Data
{
    public class InternshipApplicationRole : IdentityRole
    {
        public string Description { get; set; }
    }
}