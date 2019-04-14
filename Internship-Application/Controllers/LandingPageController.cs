using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Internship_Application.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;


namespace Internship_Application.Controllers
{
    public class LandingPageController : Controller
    {
        private readonly DataContext _context;

        public LandingPageController(DataContext context)
        {
            _context = context;
        }
        public async Task<IActionResult> Index()
        {
            var formViewModels = _context.Forms.Select(x => new FormViewModel {
                Id = x.Id,
                StudentName = x.StudentName,
                UpdatedAt = x.UpdatedAt,
                StudentEmail = x.StudentEmail,
                EmployerEmail = x.EmployerEmail,
                FacultyEmail = x.FacultyEmail,
                StatusCodesViewModel = new StatusCodes
                {
                    Id = x.StatusCodeId,
                    StatusCode = _context.StatusCodes.FirstOrDefault(s => s.Id == x.StatusCodeId).StatusCode,
                    Details = _context.StatusCodes.FirstOrDefault(s => s.Id == x.StatusCodeId).Details
                }
            }).ToList();

            if (formViewModels == null)
            {
                return View();
            }

            return View(formViewModels);
        }

        public async Task<IActionResult> EditForm(int? id)
        {
            var form = await _context.Forms
               .FirstOrDefaultAsync(m => m.Id == id);

            return View(form);
        }

        [HttpGet]
        public async Task<IActionResult> DisplayForm(int? id)
        {
            if(id == null)
            {
                //insert form
                string studentEmail = User.Identity.Name;

                Forms newForm = new Forms { };
                newForm.Answers = "[]";
                newForm.StudentEmail = studentEmail;
                newForm.StatusCodeId = 1;

                Templates temp = _context.Templates
                    .First(m => m.IsActive == true);

                newForm.TemplateId = temp.Id;

                if (ModelState.IsValid)
                {
                    _context.Add(newForm);
                    await _context.SaveChangesAsync();
                    id = newForm.Id;
                }
            }

             var form = await _context.Forms
                .FirstOrDefaultAsync(m => m.Id == id);
            var template = _context.Templates
                .FirstOrDefault(m => m.Id == form.TemplateId);

            var formViewModel = new FormViewModel
            {
                Id = form.Id,
                StudentName = form.StudentName,
                UpdatedAt = form.UpdatedAt,
                StudentEmail = form.StudentEmail,
                EmployerEmail = form.EmployerEmail,
                FacultyEmail = form.FacultyEmail,
                StatusCodesViewModel = new StatusCodes
                {
                    Id = form.StatusCodeId,
                    StatusCode = _context.StatusCodes.FirstOrDefault(s => s.Id == form.StatusCodeId).StatusCode,
                    Details = _context.StatusCodes.FirstOrDefault(s => s.Id == form.StatusCodeId).Details
                }
            };
            /*template.Questions = template.Questions.TrimStart('\"');
            template.Questions = template.Questions.TrimEnd('\"');
            template.Questions = template.Questions.Replace("\\", "");*/
            List<Answers> answers = JsonConvert.DeserializeObject<List<Answers>>(form.Answers);
            List<Questions> questions = JsonConvert.DeserializeObject<List<Questions>>(template.Questions);
            List<InputViewModel> inputs = new List<InputViewModel>();

            //
            string role = "";
            if (User.IsInRole("Admin"))
            {
                role = "Admin";
            }
            else if (User.IsInRole("Student"))
            {
                role = "Student";
            }
            else if (User.IsInRole("FacultyOfRec"))
            {
                role = "FacultyOfRec";
            }
            else if (User.IsInRole("Employer"))
            {
                role = "Employer";
            }
            else if (User.IsInRole("StudentServices"))
            {
                role = "Student Services";
            }

            Boolean correctRoleSubmit = false;
            //if all required questions met and status code is for them
            if (role == "Admin")
            {
                if (formViewModel.StatusCodesViewModel.Id == 3 || formViewModel.StatusCodesViewModel.Id == 5 || formViewModel.StatusCodesViewModel.Id == 7)
                {
                    correctRoleSubmit = true;
                }
            }
            else if (role == "Student")
            {
                if (formViewModel.StatusCodesViewModel.Id == 1)
                {
                    correctRoleSubmit = true;
                }
            }
            else if (role == "FacultyOfRec")
            {
                if (formViewModel.StatusCodesViewModel.Id == 6)
                {
                    correctRoleSubmit = true;
                }
            }
            else if (role == "StudentServices")
            {
                if (formViewModel.StatusCodesViewModel.Id == 4)
                {
                    correctRoleSubmit = true;
                }
            }
            else if (role == "Employer")
            {
                if (formViewModel.StatusCodesViewModel.Id == 2)
                {
                    correctRoleSubmit = true;
                }
            }
            //
            foreach(var q in questions)
            {

                foreach(var a in answers)
                {
                    if(q.Order == a.Order)
                    {
                        //isRequired by role?
                        bool isReq = false;
                        bool isDisabled = true;
                        if(q.Role == role && correctRoleSubmit)
                        {
                            isDisabled = false;
                        }
                        if(q.Required && q.Role == role)
                        {
                            isReq = true;
                        }
                        inputs.Add(new InputViewModel
                        {
                            DisplayOrder = Convert.ToInt32(q.Order),
                            Order = Convert.ToInt32(q.Order),
                            Prompt = q.Prompt,
                            InputType = q.InputType,
                            HelperText = q.HelperText,
                            Options = q.Options,
                            Signed = q.Signed,
                            Required = q.Required,
                            ProcessQuestion = q.ProcessQuestion,
                            Role = q.Role,
                            Value = a.Value,
                            DateSigned = a.DateSigned,
                            isRequired = isReq,
                            isDisabled = isDisabled
                        });
                    }
                }
            }
            QuestionsAndAnswers qaList = new QuestionsAndAnswers
            {
                FormDetails = formViewModel,
                TemplateDetails = template,
                QuestionList = questions,
                AnswerList = answers,
                InputList = inputs
            };
            return View(qaList);
        }

        [HttpPost]
        public async Task<IActionResult> DisplayForm(QuestionsAndAnswers questionsAndAnswers, string submit)
        {
            var form = await _context.Forms.FindAsync(questionsAndAnswers.FormDetails.Id);
            var userInput = questionsAndAnswers.InputList;

            //
            string userRole = getRole();
            bool canSubmit = getCorrRoleSubmit(userRole, questionsAndAnswers.FormDetails.StatusCodesViewModel.Id);
            var reqCount = 0;
            string studentName = "", wuId = "", studentEmail = "", facultyEmail = "", employerEmail = "";
            bool answeredAll = true;
            foreach(var item in userInput)
            {
                /*if(userRole == item.Role && item.isRequired) {
                    reqCount++;
                }*/
                if(item.isRequired && (item.Value == null || item.Value.Length <= 0))
                {
                    answeredAll = false;
                }
                if(item.ProcessQuestion)
                {
                    switch (item.Prompt)
                    {
                        case "Intern Name":
                            form.StudentName = item.Value;
                            break;
                        case "Student Id Number":
                            form.WuId = item.Value;
                            break;
                        case "Email":
                            form.StudentEmail = item.Value;
                            break;
                        case "Employer Email":
                            form.EmployerEmail = item.Value;
                            break;
                        case "Instructor of Record Email":
                            form.FacultyEmail = item.Value;
                            break;
                        default:
                            break;
                        
                    }
                }
            }
            //

            string answerString = "[";
            foreach(var item in userInput)
            {
                answerString = answerString + "{\"Order\":\"" + item.Order + "\", \"Value\":\"" + item.Value + "\", \"DateSigned\":\"" + item.DateSigned + "\"},";  
            }
            answerString = answerString + "]";
            switch (submit)
            {
                case "Submit":
                    if (answeredAll)
                    {
                        form.StatusCodeId++;
                        form.Answers = answerString;
                        form.UpdatedAt = DateTime.Now;
                        await _context.SaveChangesAsync();
                        return RedirectToAction("Index", "Receipt_Submission_Page", new { id = questionsAndAnswers.FormDetails.Id });
                    }
                    else if (form.StatusCodeId == 3)
                    {
                        form.StatusCodeId++;
                        form.Answers = answerString;
                        form.UpdatedAt = DateTime.Now;
                        await _context.SaveChangesAsync();
                        return RedirectToAction("Index", "Receipt_Submission_Page", new { id = questionsAndAnswers.FormDetails.Id });
                    }
                    else if(form.StatusCodeId == 5 && form.FacultyEmail != null)
                    {
                        form.StatusCodeId++;
                        form.Answers = answerString;
                        form.UpdatedAt = DateTime.Now;
                        await _context.SaveChangesAsync();
                        return RedirectToAction("Index", "Receipt_Submission_Page", new { id = questionsAndAnswers.FormDetails.Id });
                    }
                    else
                    {
                        form.Answers = answerString;
                        await _context.SaveChangesAsync();
                        return RedirectToAction(nameof(DisplayForm));
                    }
                case "Accept":
                    form.StatusCodeId = 8;
                    form.UpdatedAt = DateTime.Now;
                    break;
                case "Decline":
                    form.StatusCodeId = 9;
                    form.UpdatedAt = DateTime.Now;
                    break;
                case "Withdraw":
                    form.StatusCodeId = 10;
                    form.UpdatedAt = DateTime.Now;
                    break;
                case "SendBack":
                    form.UpdatedAt = DateTime.Now;
                    break;
                case "Save":
                    form.Answers = answerString;
                    form.UpdatedAt = DateTime.Now;
                    await _context.SaveChangesAsync();
                    break;
                case "Back To List":
                    return RedirectToAction(nameof(Index));
                default:
                    throw new Exception();
            }
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        public string getRole()
        {
            string role = "";
            if (User.IsInRole("Admin"))
            {
                role = "Admin";
            }
            else if (User.IsInRole("Student"))
            {
                role = "Student";
            }
            else if (User.IsInRole("FacultyOfRec"))
            {
                role = "FacultyOfRec";
            }
            else if (User.IsInRole("Employer"))
            {
                role = "Employer";
            }
            else if (User.IsInRole("StudentServices"))
            {
                role = "Student Services";
            }
            return role;
        }

        public bool getCorrRoleSubmit(string role, int statId)
        {
            Boolean correctRoleSubmit = false;
            //if all required questions met and status code is for them
            if (role == "Admin")
            {
                if (statId == 3 || statId == 5 || statId == 7)
                {
                    correctRoleSubmit = true;
                }
            }
            else if (role == "Student")
            {
                if (statId == 1)
                {
                    correctRoleSubmit = true;
                }
            }
            else if (role == "FacultyOfRec")
            {
                if (statId == 6)
                {
                    correctRoleSubmit = true;
                }
            }
            else if (role == "StudentServices")
            {
                if (statId == 4)
                {
                    correctRoleSubmit = true;
                }
            }
            else if (role == "Employer")
            {
                if (statId == 2)
                {
                    correctRoleSubmit = true;
                }
            }
            return correctRoleSubmit;
        }

    }
}