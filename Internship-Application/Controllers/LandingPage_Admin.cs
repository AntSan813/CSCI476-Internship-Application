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

        // GET: Forms
        //public async Task<IActionResult> Index()
        //{


        //    var forms = _context.Forms.ToList<Forms>();

        //    if (forms == null)
        //    {
        //        //TODO: move this to a function

        //        return View();
        //    }
        //    //templateView.StudentQuestions = JsonConvert.DeserializeObject<List<JsonModel>>(template.StudentQuestions);
        //    //templateView.StudentServicesQuestions = JsonConvert.DeserializeObject<List<JsonModel>>(template.StudentServicesQuestions);
        //    //templateView.FacultyQuestions = JsonConvert.DeserializeObject<List<JsonModel>>(template.FacultyQuestions);
        //    //templateView.EmployerQuestions = JsonConvert.DeserializeObject<List<JsonModel>>(template.EmployerQuestions);
        //    //templateView.AdministratorQuestions = JsonConvert.DeserializeObject<List<JsonModel>>(template.AdministratorQuestions);
        //    return View(forms);
            
        //}

        //public async Task<IActionResult> DisplayForm(int? id)
        //{

        //}
        [Authorize(Roles = "Admin")]
        // GET: /<controller>/
        public async Task<IActionResult> Index()
        {
            //{
            //prompt: Templates.Prompt
            //promptValue: forms.PromptValue,
            //}

            //get user form and template that form references by using user id
            //only use active form
            //TODO: change this to take in a list of forms since user can have up to 2
            ////
            var form = await _context.Forms
                .FirstOrDefaultAsync(m => m.Id == 6);
            var template = await _context.Templates
                .FirstOrDefaultAsync(m => m.Id == 6);


            List<QAJsonModel> StudentQuestions = new List<QAJsonModel> { };

            FormJsonModel item = new FormJsonModel { };

            var formSQ = JsonConvert.DeserializeObject<List<FormJsonModel>>(form.StudentQuestions);
            //var formEQ = JsonConvert.DeserializeObject<List<FormJsonModel>>(form.EmployerQuestions);
            //var formFQ = JsonConvert.DeserializeObject<List<FormJsonModel>>(form.FacultyQuestions);
            //var formSSQ = JsonConvert.DeserializeObject<List<FormJsonModel>>(form.StudentServicesQuestions);
            //var formAQ = JsonConvert.DeserializeObject<List<FormJsonModel>>(form.AdministratorQuestions);

            
            var templateSQ = JsonConvert.DeserializeObject<List<JsonModel>>(template.Questions);
            //var templateEQ = JsonConvert.DeserializeObject<List<JsonModel>>(template.EmployerQuestions);
            //var templateFQ = JsonConvert.DeserializeObject<List<JsonModel>>(template.FacultyQuestions);
            //var templateSSQ = JsonConvert.DeserializeObject<List<JsonModel>>(template.StudentServicesQuestions);
            //var templateAQ = JsonConvert.DeserializeObject<List<JsonModel>>(template.AdministratorQuestions);


            for(var i = 0; i < templateSQ.Count; i++)
            {
                var temp = new QAJsonModel
                {
                    Prompt = templateSQ[i].Prompt,
                    PromptValue = formSQ[i].PromptValue
                };
                StudentQuestions.Add(temp);
            }
            //return View(await _context.Forms.ToListAsync());
            return View(StudentQuestions);
        }



    }
}
