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
    public class LandingPageController : Controller
    {
        private readonly DataContext _context;

        public LandingPageController(DataContext context)
        {
            _context = context;
        }

        //[Authorize(Roles = "Admin")]
        public async Task<IActionResult> Index()
        {
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