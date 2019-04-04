using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Internship_Application.Models;
using System;
using Newtonsoft.Json;
using System.Collections.Generic;
using Microsoft.AspNetCore.Http;

namespace Internship_Application.Controllers
{
    public class FormTemplateEditor : Controller
    {
        private readonly DataContext _context;

        public FormTemplateEditor(DataContext context)
        {
            _context = context;
        }

        // GET: Templates
        public IActionResult Index()
        {
            //if (id == null)
            // {
            //    return NotFound();
            //}

            var template = _context.Templates.ToList<Templates>();

            if (template == null)
            {
                //TODO: move this to a function

                return View();
            }
            TemplateViewModel templateView = new TemplateViewModel { };
            //templateView.StudentQuestions = JsonConvert.DeserializeObject<List<JsonModel>>(template[0].StudentQuestions);
            //templateView.StudentServicesQuestions = JsonConvert.DeserializeObject<List<JsonModel>>(template.StudentServicesQuestions);
            //templateView.FacultyQuestions = JsonConvert.DeserializeObject<List<JsonModel>>(template.FacultyQuestions);
            //templateView.EmployerQuestions = JsonConvert.DeserializeObject<List<JsonModel>>(template.EmployerQuestions);
            //templateView.AdministratorQuestions = JsonConvert.DeserializeObject<List<JsonModel>>(template.AdministratorQuestions);
            templateView.Templates = template;
            return View(templateView);
        }



        // GET: Templates/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var templates = await _context.Templates
                .FirstOrDefaultAsync(m => m.Id == id);
            if (templates == null)
            {
                return NotFound();
            }

            return View(templates);
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
        public async Task<IActionResult> Create(/*[Bind("IsActive,TemplateName,DisplayName,Questions,Disclaimer")] */ Templates templates)
        {
            //Required questions in the form: student name, Winthrop id, employer email, and faculty of record email

            //START OF ADDING DEFAULT QUESTIONS
            List<JsonModel> questions = new List<JsonModel> { };
            questions.Add(new JsonModel
            {
                Prompt = "Student Name",
                InputType = "small-text",
                Order = 1,
                HelperText = "",
                DateSigned = "",
                Signed = false,
                Options = new List<string> { },
                Required = true,
                Person = "student",
            });

            questions.Add(new JsonModel
            {
                Prompt = "Class Name",
                InputType = "small-text",
                Order = 2,
                HelperText = "E.g. CSCI 491",
                DateSigned = "",
                Signed = false,
                Options = new List<string> { },
                Required = true,
                Person = "student",
            });

            questions.Add(new JsonModel
            {
                Prompt = "Employer Email",
                InputType = "small-text",
                Order = 3,
                HelperText = "",
                DateSigned = "",
                Signed = false,
                Options = new List<string> { },
                Required = true,
                Person = "employer",
            });

            questions.Add(new JsonModel
            {
                Prompt = "Company Name",
                InputType = "small-text",
                Order = 4,
                HelperText = "",
                DateSigned = "",
                Signed = false,
                Options = new List<string> { },
                Required = true,
                Person = "employer",
            });

            questions.Add(new JsonModel
            {
                Prompt = "Company Location",
                InputType = "small-text",
                Order = 5,
                HelperText = "",
                DateSigned = "",
                Signed = false,
                Options = new List<string> { },
                Required = true,
                Person = "faculty-of-record",
            });

            questions.Add(new JsonModel
            {
                Prompt = "Paid",
                InputType = "signature",
                Order = 6,
                HelperText = "",
                DateSigned = "",
                Signed = false,
                Options = new List<string> { },
                Required = true,
                Person = "employer",
            });

            questions.Add(new JsonModel
            {
                Prompt = "Salary",
                InputType = "money",
                Order = 7,
                HelperText = "",
                DateSigned = "",
                Signed = false,
                Options = new List<string> { },
                Required = false, //false in the case of no paid internship
                Person = "employer",
            });

            questions.Add(new JsonModel
            {
                Prompt = "Faculty of Record Email",
                InputType = "small-text",
                Order = 8,
                HelperText = "",
                DateSigned = "",
                Signed = false,
                Options = new List<string> { },
                Required = true,
                Person = "faculty-of-record",
            });

            templates.Questions = JsonConvert.SerializeObject(questions);

            if (ModelState.IsValid)
            {
                _context.Add(templates);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            var errors = ModelState.Select(x => x.Value.Errors)
                           .Where(y => y.Count > 0)
                           .ToList();

            return View(templates);
        }

        // GET: Templates/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var template = await _context.Templates
                .FirstOrDefaultAsync(m => m.Id == id);

            TemplateViewModel templateView = new TemplateViewModel { };
            templateView.Templates = new List<Templates> {template};

            if (templateView.Templates.First() == null)
            {
                return NotFound();
            }
            
            templateView.Questions = JsonConvert.DeserializeObject<List<JsonModel>>(template.Questions);

            return View(templateView);
        }

        // POST: Templates/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, IFormCollection collection, [Bind("Questions,Templates")] TemplateViewModel templateView)
        {

            if (id != templateView.Templates[0].Id)
            {
                return NotFound();
            }

            //if (templateView.Templates[0].IsRetired == true)
            //{
            //    //TODO: change this to return the previous page and a flash message saying that you cannot edit the form since a form has been made from it
            //    return Create();
            //}



            //***LEFT OFF FIGURING OUT WHY templateView.Templates[0].Questions IS NULL****
            List<JsonModel> questionModel = JsonConvert.DeserializeObject<List<JsonModel>>(templateView.Templates[0].Questions);
            var order = questionModel.Count + 1;
            //the following may be useful later
            //foreach (JsonModel question in questionModel)
            //{
            //    if (question.Order > order)
            //    {
            //        order = question.Order + 1;
            //    }
            //}
            //TODO: Add checks to make sure data was inputted correctly before storing the information
            //E.g. add a check to see if the order is valid (order must be within 1 and len(questions) + 1. 
            questionModel.Add(new JsonModel
            {
                Prompt = collection["prompt"],
                InputType = collection["input-type"],
                Order = order, //Int32.Parse(collection["order"]),
                HelperText = collection["helper-text"],
                DateSigned = "",
                Signed = false,
                Options = new List<string> { },
                Required = false, //TODO: change this to take in a field from the collection
                Person = "student" //TODO: change this to take in a field from the collection,
            });

            templateView.Templates[0].Questions = JsonConvert.SerializeObject(questionModel);
            templateView.Questions = questionModel;

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Templates.Update(templateView.Templates[0]);
                    
                     _context.SaveChanges();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!TemplatesExists(templateView.Templates[0].Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                //return RedirectToAction(nameof(Index));
                return View(templateView);

            }

            return View(templateView);
        }

        // GET: Templates/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var templates = await _context.Templates
                .FirstOrDefaultAsync(m => m.Id == id);
            if (templates == null)
            {
                return NotFound();
            }

            return View(templates);
        }

        // POST: Templates/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var templates = await _context.Templates.FindAsync(id);
            //_context.Templates.Remove(templates);
            //await _context.SaveChangesAsync();

            //TODO: Change this to only set the DeleteAt field iff there is a reference to this template in the Forms table
            //meaning, that if someone made a application with this template, we have to keep the template in the database until that application
            //gets deleted after 5 years. 
            //Only let admin delete iff no other apps use this template. 
            templates.IsActive = false;
            templates.IsRetired = true;
            templates.RetiredAt = DateTime.Now;
            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(templates);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!TemplatesExists(templates.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            
            return View(templates);
        }

        private bool TemplatesExists(int id)
        {
            return _context.Templates.Any(e => e.Id == id);
        }

        // GET: Templates/Edit/5
        public async Task<IActionResult> EditJson(int id)
        {

            var templates = await _context.Templates.FirstOrDefaultAsync(m => m.Id == id);

            Console.WriteLine(templates.Questions);

            if (templates == null)
            {
                return NotFound();
            }
            return View(templates);
        }
    }
}
