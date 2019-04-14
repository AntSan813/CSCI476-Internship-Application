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
        public async Task<IActionResult> DisplayForm(int? id)
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
            
            List<Answers> answers = JsonConvert.DeserializeObject<List<Answers>>(form.Answers);
            List<Questions> questions = JsonConvert.DeserializeObject<List<Questions>>(template.Questions);
            QuestionsAndAnswers qaList = new QuestionsAndAnswers
            {
                FormDetails = formViewModel,
                TemplateDetails = template,
                QuestionList = questions,
                AnswerList = answers
            };
            return View(qaList);
        }

        [HttpPost]
        public async Task<IActionResult> DisplayForm(QuestionsAndAnswers questionsAndAnswers)
        {
            var form = await _context.Forms.FindAsync(questionsAndAnswers.FormDetails.Id);
            form.StatusCodeId++;
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

    }
}