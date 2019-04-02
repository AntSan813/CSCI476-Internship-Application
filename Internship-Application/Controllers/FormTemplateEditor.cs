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
            JsonModel item = new JsonModel { };
            List<JsonModel> jsonStr = new List<JsonModel>();
            item.Prompt = "";
            item.InputType = "small-text";
            item.Order = 1;
            item.HelperText = "";
            item.DateSigned = "";
            item.Signed = false;
            item.Options = new List<string>{ };

            //default empty array of questions for each section
            templates.IsRetired = false;
            templates.Questions = "[]";

            jsonStr = JsonConvert.DeserializeObject<List<JsonModel>>(templates.Questions);
            jsonStr.Add(item);

            templates.Questions = JsonConvert.SerializeObject(jsonStr);
            
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
            templateView.Templates = new List<Templates>();
            templateView.Templates.Add(template);

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
        public async Task<IActionResult> EditAsync(int id, IFormCollection collection, [Bind("Questions,Templates")] TemplateViewModel templateView)
        {
            Console.WriteLine(id);
            Console.WriteLine(templateView.Templates.First().Id);

            //List<Templates> templates = new List<Templates>();
            //templates.Add(template);
            //Console.WriteLine(templateView.Templates.GetType());
            //Console.WriteLine(template.GetType());
            if (id != templateView.Templates[0].Id)
            {
                return NotFound();
            }

            if (templateView.Templates.First().IsRetired == false)
            {
                //TODO: change this to return the previous page and a flash message saying that you cannot edit the form since a form has been made from it
                return Create();
            }
            // TemplateViewModel templateView = new TemplateViewModel { };
            //templateView.Templates = new List<Templates>();

            List<JsonModel> jsonStr = new List<JsonModel>();

            //generate first json
            JsonModel item = new JsonModel { };
            item.Prompt = collection["prompt"];
            item.InputType = collection["input-type"];
            // item.Order = Int32.Parse(collection["order"]);
            item.HelperText = collection["helperText"];
            //item.Options = collection["options"].ToList<string>();

            //foreach(JsonModel question in jsonStr){
            //    if(question.Order > item.Order)
            //    {
            //        item.Order = question.Order + 1;
            //    }
            //}

            if (collection["input-type"] == "signature")
            {
                //get form data
                item.DateSigned = "";
                item.Signed = false;
            }

            //TODO: separate following iff stmts to be a function that returns the serialized json by taking in the collection.
            if (collection["person"] == "student")
            {
                Console.WriteLine(templateView.Templates.First().Questions);

                jsonStr = JsonConvert.DeserializeObject<List<JsonModel>>(templateView.Templates.First().Questions);
                jsonStr.Add(item);
                string serializedJson = JsonConvert.SerializeObject(jsonStr);
                templateView.Templates.First().Questions = serializedJson;
            }
           // templateView.Template
           // templateView.Templates[0].AdministratorQuestions = JsonConvert.SerializeObject(templateView.AdministratorQuestions);
        

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
