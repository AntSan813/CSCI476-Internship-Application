
Taite, Megan Teona-Maria
6:37 PM (0 minutes ago)
to me

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Internship_Application.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;


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
            var forms = _context.Forms.ToList<Forms>();
            if(forms == null)
            {
                return View();
            }
            //templateView.StudentQuestions = JsonConvert.DeserializeObject<List<JsonModel>>(template.StudentQuestions);
            return View(forms);
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

            return View(form);
        }

    }
}