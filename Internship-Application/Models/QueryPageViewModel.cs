using System;
using System.Collections.Generic;

namespace Internship_Application.Models
{
    public partial class QueryPageViewModel
    {
        public int PreviousCompanyNameQuery { get; set; }
        public int PreviousCompanyLocationQuery { get; set; }
        public int PreviousYearQuery { get; set; }
        public int PreviousSemesterQuery { get; set; }
        public int PreviousPaidQuery { get; set; }
        public int PreviousUnpaidQuery { get; set; }
        public int PreviousStatusCodeQuery { get; set; }
        public List<int> QueriedFormIds { get; set; }
    }
}
