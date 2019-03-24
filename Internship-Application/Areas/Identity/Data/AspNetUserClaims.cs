using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;

namespace Internship_Application.Areas.Identity.Data
{
    public partial class AspNetUserClaims : IdentityUserClaim<string>
 
    {
        /*
        public int Id { get; set; }
        public string UserId { get; set; }
        public string ClaimType { get; set; }
        public string ClaimValue { get; set; }
        */
        public virtual AspNetUsers User { get; set; }
    }
}
