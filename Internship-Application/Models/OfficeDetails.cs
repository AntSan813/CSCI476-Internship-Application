using System;
using System.Collections.Generic;

namespace Internship_Application.Models
{
    public partial class OfficeDetails
    {
        public int Id { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public DateTime? DeletedAt { get; set; }
        public DateTime DateReceived { get; set; }
        public bool CorrespondenceSentEmployer { get; set; }
        public bool CorrespondenceSentStudent { get; set; }
        public string EstMidPoint { get; set; }
        public bool MeetsClassReq { get; set; }
        public string Classes { get; set; }
        public bool MeetsGpaReq { get; set; }
        public float Gpa { get; set; }
        public bool MeetsPosReq { get; set; }
        public string Pos { get; set; }
        public string Other { get; set; }
    }
}
