using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;

namespace Internship_Application.Areas.Identity.Data
{
    public partial class AspNetUserRoles 
    {
        public string UserId { get; set; }
        public string RoleId { get; set; }
        
        public virtual AspNetRoles Role { get; set; }
        public virtual AspNetUsers User { get; set; }
    }
}
