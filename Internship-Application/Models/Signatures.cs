using System;
using System.Collections.Generic;

namespace Internship_Application.Models
{
    public partial class Signatures
    {
        public int Id { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public DateTime? DeletedAt { get; set; }
        public bool StudentSigned { get; set; }
        public bool EmployerSigned { get; set; }
        public bool FacultySigned { get; set; }
        public bool StudentServicesSigned { get; set; }
        public bool DirectorSigned { get; set; }
        public DateTime? StudentSignDate { get; set; }
        public DateTime? EmployerSignDate { get; set; }
        public DateTime? FacultySignDate { get; set; }
        public DateTime? StudentServicesSignDate { get; set; }
        public DateTime? DirectorSignDate { get; set; }
    }
}
