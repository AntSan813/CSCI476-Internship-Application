using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Internship_Application.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;


// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Internship_Application.Controllers
{
    public class LandingPage_Admin : Controller
    {
        private readonly DataContext _context;

        public LandingPage_Admin(DataContext context)
        {
            _context = context;
        }

        [Authorize(Roles = "Admin")]
        // GET: /<controller>/
        public async Task<IActionResult> Index()
        {
            //var forms = _context.Forms.ToList<Forms>();
            var formViewModels = _context.Forms.Select(x => new FormViewModel {
                Id = x.Id,
                StudentName = x.StudentName,
                UpdatedAt = x.UpdatedAt,
                StudentEmail = x.StudentEmail,
                EmployerEmail = x.EmployerEmail,
                FacultyEmail = x.FacultyEmail,
                StatusCodesViewModel = new StatusCodesViewModel
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

            //templateView.StudentQuestions = JsonConvert.DeserializeObject<List<JsonModel>>(template.StudentQuestions);
            return View(formViewModels);
           /* var form = await _context.Forms
                .FirstOrDefaultAsync();
            var template = await _context.Templates
                .FirstOrDefaultAsync();


            List<FormJsonModel> StudentQuestions = new List<FormJsonModel> { };

            FormJsonModel item = new FormJsonModel { };

            //var formSQ = JsonConvert.DeserializeObject<List<FormJsonModel>>(form.StudentQuestions);
            
            //var templateSQ = JsonConvert.DeserializeObject<List<JsonModel>>(template.Questions);

            for(var i = 0; i < templateSQ.Count; i++)
            {
                var temp = new FormJsonModel
                {
                    Prompt = templateSQ[i].Prompt,
                   // PromptValue = formSQ[i].PromptValue
                };
                Questions.Add(temp);
            }
            //return View(await _context.Forms.ToListAsync());
            return View(Questions);*/
        }

        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> EditForm(int? id)
        {
            var form = await _context.Forms
               .FirstOrDefaultAsync(m => m.Id == id);

            return View(form);
        }

        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DisplayForm(int? id)
        {
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
                StatusCodesViewModel = new StatusCodesViewModel
                {
                    Id = form.StatusCodeId,
                    StatusCode = _context.StatusCodes.FirstOrDefault(s => s.Id == form.StatusCodeId).StatusCode,
                    Details = _context.StatusCodes.FirstOrDefault(s => s.Id == form.StatusCodeId).Details
                }
            };

            //form.Answers = JsonConvert.DeserializeObject<List<JsonModel>>(form.Answers);
            List<Answers> answers = JsonConvert.DeserializeObject<List<Answers>>(form.Answers);
            List<Questions> questions = JsonConvert.DeserializeObject<List<Questions>>(template.Questions);
            QuestionsAndAnswers qaList = new QuestionsAndAnswers
            {
                FormDetails = formViewModel,
                TemplateDetails = template,
                QuestionList = questions,
                AnswerList = answers
            };
            //form.Answers = json.ToString();
            return View(qaList);
        }

    }
}