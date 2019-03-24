using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;

namespace Internship_Application.Areas.Identity.Data
{
    public partial class AspNetRoleClaims : IdentityRoleClaim<string>
    {
        /*
        public int Id { get; set; }
        public string RoleId { get; set; }
        public string ClaimType { get; set; }
        public string ClaimValue { get; set; }
        */
        public virtual AspNetRoles Role { get; set; }
    }
}
