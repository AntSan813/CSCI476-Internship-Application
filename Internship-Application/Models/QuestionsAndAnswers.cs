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
    }
}
