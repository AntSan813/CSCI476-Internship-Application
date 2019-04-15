using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Internship_Application.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;

namespace Internship_Application.Controllers
{
    [Authorize(Roles = "Admin")]//only let the user on this page if they have the admin role
    public class Admin_TemplatesOverviewPageController : Controller
    {
        private readonly DataContext _context;

        public Admin_TemplatesOverviewPageController(DataContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var templateList = _context.Templates.Select(x => new Templates
            {
                Id = x.Id,
                TemplateName = x.TemplateName,
                UpdatedAt = x.UpdatedAt,
                CreatedAt = x.CreatedAt,
                RetiredAt = x.RetiredAt,
                IsActive = x.IsActive,
                IsRetired = x.IsRetired,
            }).ToList();

            if (templateList == null)
            {
                return View();
            }

            return View(templateList);
        }

        public async Task<IActionResult> EditForm(int? id)
        {
            var form = await _context.Forms
               .FirstOrDefaultAsync(m => m.Id == id);

            return View(form);
        }

        [HttpGet]
        public async Task<IActionResult> DisplayTemplate(int? id)
        {
            if (id == null)
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

            var template = await _context.Templates
               .FirstOrDefaultAsync(m => m.Id == id);
            template = _context.Templates
                .FirstOrDefault(m => m.Id == template.Id);
                
            /*template.Questions = template.Questions.TrimStart('\"');
            template.Questions = template.Questions.TrimEnd('\"');
            template.Questions = template.Questions.Replace("\\", "");*/

            List<Questions> questions = JsonConvert.DeserializeObject<List<Questions>>(template.Questions);
            QuestionsAndAnswers qList = new QuestionsAndAnswers
            {
                TemplateDetails = template,
                QuestionList = questions,
            };
            return View(qList);
        }

        [HttpPost]
        public async Task<IActionResult> DisplayTemplate(QuestionsAndAnswers questionsAndAnswers, string submit)
        {
            var template = await _context.Templates.FindAsync(questionsAndAnswers.TemplateDetails.Id);

            var templateList = _context.Templates.Select(x => new Templates
            {
                Id = x.Id,
                TemplateName = x.TemplateName,
                UpdatedAt = x.UpdatedAt,
                CreatedAt = x.CreatedAt,
                RetiredAt = x.RetiredAt,
                IsActive = x.IsActive,
                IsRetired = x.IsRetired,
            }).ToList();

            var formViewModels = _context.Forms.Select(x => new FormViewModel
            {
                Id = x.Id,
                StudentName = x.StudentName,
                UpdatedAt = x.UpdatedAt,
                StudentEmail = x.StudentEmail,
                EmployerEmail = x.EmployerEmail,
                FacultyEmail = x.FacultyEmail,
                TemplateId = x.TemplateId,
                StatusCodesViewModel = new StatusCodes
                {
                    Id = x.StatusCodeId,
                    StatusCode = _context.StatusCodes.FirstOrDefault(s => s.Id == x.StatusCodeId).StatusCode,
                    Details = _context.StatusCodes.FirstOrDefault(s => s.Id == x.StatusCodeId).Details
                }
            }).ToList();

            switch (submit)
            {
                case "Make Template Active":
                    //can only have ONE active template at a time
                    var countActives = 0;
                    foreach (var item in templateList)
                    {
                        if(item.IsActive == true)
                        {
                            //found an active template
                            countActives++;
                        }
                        else
                        {
                            countActives = countActives + 0;
                        }
                    }

                    if(countActives > 0)
                    {
                        //already an active template
                        template.IsActive = false;
                        template.IsRetired = false;
                    }
                    else
                    {
                        template.IsActive = true;
                        template.IsRetired = false;
                    }
                    break;
                case "Retire Template":
                    //cannot retire a template if there is any student still using it. 
                    var usingTemplateCount = 0;
                    var list = new List<dynamic>();
                    foreach (var item in formViewModels)
                    {
                        if (item.TemplateId == template.Id)
                        {
                            //currently looking at a form that references the id of the template about to be retired, if it is any status code other than 8, 9, or 10, it cannot be retired
                            if ((item.StatusCodesViewModel.Id == 8) || (item.StatusCodesViewModel.Id == 9) || (item.StatusCodesViewModel.Id == 10))
                            {
                                //doesn't stop the form from being retired
                                usingTemplateCount = usingTemplateCount + 0;
                            }
                            else
                            {
                                //store student email to let her know who is using the template
                                usingTemplateCount++;
                                list.Add(item.StudentEmail);
                            }
                        }
                        //if any record in the forms database is referencing the id of the template that is trying to be retired, show admin the emails of those users and prevent template from being retired
                    }

                    if(usingTemplateCount > 0)
                    {
                        template.IsActive = true; //must stay active
                        template.IsRetired = false; //cannot be retired
                    }
                    else
                    {
                        template.IsActive = false;
                        template.IsRetired = true; //can retire form
                        template.RetiredAt = DateTime.Now; //updates when form was retired
                    }
                    break;

                case "Move Template to In-Progress":
                    template.IsActive = false;
                    template.IsRetired = false;
                    template.RetiredAt = null;
                    break;

                default:
                    throw new Exception();
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        // GET: Templates/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Templates/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateTemplate(Templates template)
        {
            //try to copy from active template
            Templates temp = _context.Templates
                   .First(m => m.IsActive == true);
            if(temp != null)
            {
                //List<Questions> qList = JsonConvert.DeserializeObject<List<Questions>>(temp.Questions);
                template.Questions = temp.Questions;
            }
            else
            {
                //Required questions in the form: student name, Winthrop id, employer email, and faculty of record email

                //START OF ADDING DEFAULT QUESTIONS
                List<Questions> qList = new List<Questions> { };
                qList.Add(new Questions
                {
                    Order = "1",
                    Prompt = "Intern Name",
                    InputType = "text",
                    HelperText = "",
                    Options = "",
                    Required = true,
                    ProcessQuestion = true,
                    Role = "Student"
                });
                qList.Add(new Questions
                {
                    Order = "2",
                    Prompt = "Email",
                    InputType = "text",
                    HelperText = "",
                    Options = "",
                    Required = true,
                    ProcessQuestion = true,
                    Role = "Student"
                });
                qList.Add(new Questions
                {
                    Order = "3",
                    Prompt = "Student ID Number",
                    InputType = "text",
                    HelperText = "",
                    Options = "",
                    Required = true,
                    ProcessQuestion = true,
                    Role = "Student"
                });
                qList.Add(new Questions
                {
                    Order = "4",
                    Prompt = "Employer Email",
                    InputType = "text",
                    HelperText = "",
                    Options = "",
                    Required = true,
                    ProcessQuestion = true,
                    Role = "Student"
                });
                qList.Add(new Questions
                {
                    Order = "5",
                    Prompt = "Instructor of Record Email",
                    InputType = "text",
                    HelperText = "",
                    Options = "",
                    Required = true,
                    ProcessQuestion = true,
                    Role = "Admin"
                });
                qList.Add(new Questions
                {
                    Order = "6",
                    Prompt = "Organization Name",
                    InputType = "text",
                    HelperText = "",
                    Options = "",
                    Required = true,
                    ProcessQuestion = true,
                    Role = "Employer"
                });
                qList.Add(new Questions
                {
                    Order = "7",
                    Prompt = "Paid",
                    InputType = "radio",
                    HelperText = "",
                    Options = "",
                    Required = true,
                    ProcessQuestion = true,
                    Role = "Employer"
                });
                qList.Add(new Questions
                {
                    Order = "8",
                    Prompt = "Physical Address",
                    InputType = "text",
                    HelperText = "",
                    Options = "",
                    Required = true,
                    ProcessQuestion = true,
                    Role = "Employer"
                });
                template.Questions = JsonConvert.SerializeObject(qList);
            }

            if (ModelState.IsValid)
            {
                _context.Add(template);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            var errors = ModelState.Select(x => x.Value.Errors)
                           .Where(y => y.Count > 0)
                           .ToList();

            return View(template);
        }
    }
}