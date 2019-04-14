using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;

namespace Internship_Application.Models
{
    public class QuestionsAndAnswers
    {
        public FormViewModel FormDetails { get; set; }
        public Templates TemplateDetails { get; set; }
        public List<Questions> QuestionList { get; set; }
        public List<Answers> AnswerList { get; set; }
        public List<InputViewModel> InputList { get; set; }
        public SelectList SendBackList { get; set; }
        public int SendBackListId { get; set; }

        public QuestionsAndAnswers()
        {
            var items = new Dictionary<int, string>();
            items.Add(1, "1 - In Process");
            items.Add(2, "2 - Pending-Employer Approval");
            items.Add(3, "3 - Pending-DER Approval (No Input Required)");
            items.Add(4, "4 - Pending-Student Services Approval");
            items.Add(5, "5 - Pending-DER Approval (Faculty Email Required)");
            items.Add(6, "6 - Pending-Instructor Approval");
            items.Add(7, "7 - Pending-DER Approval (Approval Required)");
            SendBackList = new SelectList(items, "Key", "Value");
            
        }
    }
}
