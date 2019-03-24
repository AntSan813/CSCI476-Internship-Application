using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;

namespace Internship_Application.Areas.Identity.Data
{
    public partial class AspNetUserTokens 
    {
        
        public string UserId { get; set; }
        public string LoginProvider { get; set; }
        public string Name { get; set; }
        public string Value { get; set; }
        
        public virtual AspNetUsers User { get; set; }
    }
}
