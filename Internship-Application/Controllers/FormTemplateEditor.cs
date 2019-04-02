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
        public async Task<IActionResult> Create( [Bind("Id,CreatedAt,UpdatedAt,DeletedAt,StudentQuestions,EmployerQuestions,FacultyQuestions,StudentServicesQuestions,AdministratorQuestions,IsActive,IsModifiable,TemplateName,FormTitle,Disclaimer")] Templates templates)
        {
            ////generate default json
            //default empty array of questions for each section
            templates.IsModifiable = true;
            templates.StudentQuestions = "[]";
            templates.EmployerQuestions = "[]";
            templates.FacultyQuestions = "[]";
            templates.StudentServicesQuestions = "[]";
            templates.AdministratorQuestions = "[]";

            List<JsonModel> emptyJson = JsonConvert.DeserializeObject<List<JsonModel>>(templates.AdministratorQuestions);
            JsonModel item = new JsonModel { };
            item.Prompt = "";
            item.InputType = "small-text";
            item.Order = 1;
            item.HelperText = "";
            item.DateSigned = "";
            item.Signed = false;
            item.Options = new List<string> { };
            emptyJson.Add(item);

            templates.AdministratorQuestions = JsonConvert.SerializeObject(emptyJson);
            templates.FacultyQuestions = JsonConvert.SerializeObject(emptyJson);
            templates.StudentServicesQuestions = JsonConvert.SerializeObject(emptyJson);
            
            //START OF ADDING DEFAULT QUESTIONS FOR STUDENT QUESTION
            List<JsonModel> studentJson = JsonConvert.DeserializeObject<List<JsonModel>>(templates.StudentQuestions);
            studentJson.Add(new JsonModel
            {
                Prompt = "Name",
                InputType = "small-text",
                Order = 1,
                HelperText = "",
                DateSigned = "",
                Signed = false,
                Options = new List<string> { }
            });

            studentJson.Add(new JsonModel
            {
                Prompt = "Class enrolled in",
                InputType = "small-text",
                Order = 2,
                HelperText = "",
                DateSigned = "",
                Signed = false,
                Options = new List<string> { }
            });

            studentJson.Add(new JsonModel
            {
                Prompt = "Major",
                InputType = "small-text",
                Order = 3,
                HelperText = "",
                DateSigned = "",
                Signed = false,
                Options = new List<string> { }
            });
            templates.StudentQuestions = JsonConvert.SerializeObject(studentJson);


            //START OF ADDING DEFAULT QUESTIONS FOR EMPLOYER QUESTION
            List<JsonModel> employerJson = JsonConvert.DeserializeObject<List<JsonModel>>(templates.EmployerQuestions);
            employerJson.Add(new JsonModel
            {
                Prompt = "Company name",
                InputType = "small-text",
                Order = 1,
                HelperText = "",
                DateSigned = "",
                Signed = false,
                Options = new List<string> { }
            });


            employerJson.Add(new JsonModel
            {
                Prompt = "Paid",
                InputType = "signature",
                Order = 2,
                HelperText = "",
                DateSigned = "",
                Signed = false,
                Options = new List<string> { }
            });

            employerJson.Add(new JsonModel
            {
                Prompt = "Salary",
                InputType = "money",
                Order = 3,
                HelperText = "",
                DateSigned = "",
                Signed = false,
                Options = new List<string> { }
            });
        

            employerJson.Add(new JsonModel
            {
                Prompt = "Company location",
                InputType = "small-text",
                Order = 4,
                HelperText = "",
                DateSigned = "",
                Signed = false,
                Options = new List<string> { }
            });


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
            //Console.WriteLine(id);
            //Console.WriteLine(template.Id);
            //Console.WriteLine(template.UpdatedAt);
            //Console.WriteLine(template.CreatedAt);
            //Console.WriteLine(template.FormTitle);
            //Console.WriteLine(template.Disclaimer);
            //Console.WriteLine(template.TemplateName);
            //Console.WriteLine(template.StudentQuestions);
            //Console.WriteLine(template.FacultyQuestions);
            //Console.WriteLine(template.EmployerQuestions);
            //Console.WriteLine(template.AdministratorQuestions);
            //Console.WriteLine(template.StudentServicesQuestions);
            //Console.WriteLine(template.IsActive);
            //Console.WriteLine(template.IsModifiable);
            //List<Templates> templates = new List<Templates>();
            //templates.Add(template);
            //Console.WriteLine(templateView.Templates.GetType());
            //Console.WriteLine(template.GetType());

            TemplateViewModel templateView = new TemplateViewModel { };
            templateView.Templates = new List<Templates> { };
            templateView.Templates.Add(template);

            if (templateView.Templates.First() == null)
            {
                return NotFound();
            }


            templateView.StudentQuestions = new List<JsonModel>(JsonConvert.DeserializeObject<List<JsonModel>>(template.StudentQuestions));
            templateView.FacultyQuestions = new List<JsonModel>(JsonConvert.DeserializeObject<List<JsonModel>>(template.FacultyQuestions));
            templateView.StudentServicesQuestions = new List<JsonModel>(JsonConvert.DeserializeObject<List<JsonModel>>(template.StudentServicesQuestions));
            templateView.EmployerQuestions = new List<JsonModel>(JsonConvert.DeserializeObject<List<JsonModel>>(template.EmployerQuestions));
            templateView.AdministratorQuestions = new List<JsonModel>(JsonConvert.DeserializeObject<List<JsonModel>>(template.AdministratorQuestions));
            Console.WriteLine(templateView.Templates.First().Id);

            return View(templateView);
        }

        // POST: Templates/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, IFormCollection collection, [Bind("StudentQuestions,EmployerQuestions,FacultyQuestions,StudentServicesQuestions,AdministratorQuestions,Templates")] TemplateViewModel templateView)
        {
         //   Console.WriteLine(id);
         //   Console.WriteLine(templateView.Templates.First().Id);
         //   Console.WriteLine(templateView.StudentQuestions.First().InputType);
         //   Console.WriteLine(templateView.FacultyQuestions.First().Order);
         //   Console.WriteLine(templateView.EmployerQuestions.First().Order);
         //   Console.WriteLine(templateView.StudentServicesQuestions.First().Order);

            if (id != templateView.Templates[0].Id)
            {
                return NotFound();
            }
            if (templateView.Templates[0].IsModifiable == false){
                //TODO: change this to return the previous page and a flash message saying that you cannot edit the form since a form has been made from it
                return Create();
            }

            
            List<JsonModel> jsonStr = new List<JsonModel>();

            //generate first json
            JsonModel item = new JsonModel { };
            item.Prompt = collection["prompt"];
            item.InputType = collection["input-type"];
            item.HelperText = collection["helperText"];

            if (collection["input-type"] == "signature")
            {
                //get form data
                item.DateSigned = "";
                item.Signed = false;
            }
            
            //TODO: separate following iff stmts to be a function that returns the serialized json by taking in the collection.
            if (collection["person"] == "student")
            {
                item.Order = templateView.StudentQuestions.Count + 1;
                templateView.StudentQuestions.Add(item);
                templateView.Templates[0].StudentQuestions = JsonConvert.SerializeObject(templateView.StudentQuestions);
            }
            else if (collection["person"] == "employer")
            {
                item.Order = templateView.EmployerQuestions.Count + 1;
                templateView.EmployerQuestions.Add(item);
                templateView.Templates[0].EmployerQuestions = JsonConvert.SerializeObject(templateView.EmployerQuestions);
            }
            else if (collection["person"] == "faculty")
            {
                item.Order = templateView.FacultyQuestions.Count + 1;
                templateView.FacultyQuestions.Add(item);
                templateView.Templates[0].FacultyQuestions = JsonConvert.SerializeObject(templateView.FacultyQuestions);
            }
            else if (collection["person"] == "student-services")
            {
                item.Order = templateView.StudentServicesQuestions.Count + 1;
                templateView.StudentServicesQuestions.Add(item);
                templateView.Templates[0].StudentServicesQuestions = JsonConvert.SerializeObject(templateView.StudentServicesQuestions);
            }
            else if (collection["person"] == "admin")
            {
                item.Order = templateView.AdministratorQuestions.Count + 1;
                templateView.AdministratorQuestions.Add(item);
                templateView.Templates[0].AdministratorQuestions = JsonConvert.SerializeObject(templateView.AdministratorQuestions);
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(templateView.Templates[0]);
                    await _context.SaveChangesAsync();
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
                return RedirectToAction(nameof(Index));
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
            var form = await _context.Forms
                .FirstOrDefaultAsync(m => m.TemplateId == templates.Id);
            if(form == null) //meaning no form refereneces this template
            {
                _context.Templates.Remove(templates);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            else
            {
                templates.IsActive = false;
                templates.IsModifiable = false;
                templates.DeletedAt = DateTime.Now;
            }

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



            if (templates == null)
            {
                return NotFound();
            }
            return View(templates);
        }
    }
}
