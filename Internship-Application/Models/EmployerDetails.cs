using System;
using System.Collections.Generic;

namespace Internship_Application.Models
{
    public partial class EmployerDetails
    {
        public int Id { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public DateTime? DeletedAt { get; set; }
        public string SupervisorName { get; set; }
        public string SupervisorTitle { get; set; }
        public string SupervisorPhone { get; set; }
        public string SupervisorEmail { get; set; }
        public string QuestionsInternRole { get; set; }
        public string QuestionsInternProjects { get; set; }
        public string QuestionsInternOutcome { get; set; }
        public string QuestionsComments { get; set; }
        public string OrgName { get; set; }
        public string BusinessLiscense { get; set; }
        public string StateIssued { get; set; }
        public string Address { get; set; }
        public bool SiteVisitAvailable { get; set; }
        public DateTime InternshipStartDate { get; set; }
        public DateTime InternshipEndDate { get; set; }
        public int InternshipNumWeeks { get; set; }
        public float InternshipHoursPerWeek { get; set; }
        public bool InternshipIsPaid { get; set; }
        public string IntershipPaidAmount { get; set; }
        public string IntershipAdditionalCompensation { get; set; }
    }
}
