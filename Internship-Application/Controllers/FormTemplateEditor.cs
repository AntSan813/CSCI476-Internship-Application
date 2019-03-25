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
        private readonly CSCI476Context _context;

        public FormTemplateEditor(CSCI476Context context)
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

            var template = _context.Templates
                    .Where(b => b.IsActive == true)
                    .FirstOrDefault();

            if (template == null)
            {
                return NotFound();
            }

            return View(template);
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
        public async Task<IActionResult> Create(IFormCollection collection, [Bind("Id,CreatedAt,UpdatedAt,DeletedAt,Name,StudentQuestions,EmployerQuestions,FacultyQuestions,StudentServicesQuestions,AdministratorQuestions,IsActive")] Templates templates)
        {
            List<JsonModel> jsonStr = new List<JsonModel>();
            // Console.WriteLine(collection["prompt"] + collection["input-type"] + collection["helper-text"] + collection["order"]);

            //generate first json
            JsonModel item = new JsonModel { };
            item.Prompt = collection["prompt"];
            item.InputType = collection["input-type"];
            item.Order = collection["order"];
            item.HelperText = collection["helperText"];

            if (collection["input-type"] == "signature")
            {
                //get form data
                item.DatedSigned = "";
                item.Signed = false;

            }

            //Console.WriteLine(item);

            //default empty array of questions for each section
            templates.StudentQuestions = "[]";
            templates.EmployerQuestions = "[]";
            templates.FacultyQuestions = "[]";
            templates.StudentServicesQuestions = "[]";
            templates.AdministratorQuestions = "[]";

            //TODO: separate following iff stmts to be a function that returns the serialized json by taking in the collection.
            if (collection["person"] == "student")
            {
                jsonStr = JsonConvert.DeserializeObject<List<JsonModel>>(templates.StudentQuestions);
                jsonStr.Add(item);
                string serializedJson = JsonConvert.SerializeObject(jsonStr);
                templates.StudentQuestions = serializedJson;
            }
            else if (collection["person"] == "employer")
            {
                jsonStr = JsonConvert.DeserializeObject<List<JsonModel>>(templates.EmployerQuestions);
                jsonStr.Add(item);
                templates.EmployerQuestions = JsonConvert.SerializeObject(jsonStr);
            }
            else if (collection["person"] == "faculty")
            {
                jsonStr = JsonConvert.DeserializeObject<List<JsonModel>>(templates.FacultyQuestions);
                jsonStr.Add(item);
                templates.FacultyQuestions = JsonConvert.SerializeObject(jsonStr);
            }
            else if (collection["person"] == "student-services")
            {
                jsonStr = JsonConvert.DeserializeObject<List<JsonModel>>(templates.StudentServicesQuestions);
                jsonStr.Add(item);
                templates.StudentServicesQuestions = JsonConvert.SerializeObject(jsonStr);
            }
            else if (collection["person"] == "admin")
            {
                jsonStr = JsonConvert.DeserializeObject<List<JsonModel>>(templates.AdministratorQuestions);
                jsonStr.Add(item);
                templates.AdministratorQuestions = JsonConvert.SerializeObject(jsonStr);
            }
            //Console.WriteLine(jsonStr);

            if (ModelState.IsValid)
            {
                _context.Add(templates);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(templates);
        }

        // GET: Templates/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var templates = await _context.Templates.FindAsync(id);
            if (templates == null)
            {
                return NotFound();
            }
            return View(templates);
        }

        // POST: Templates/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, IFormCollection collection, [Bind("Id,CreatedAt,UpdatedAt,DeletedAt,Name,StudentQuestions,EmployerQuestions,FacultyQuestions,StudentServicesQuestions,AdministratorQuestions,IsActive")] Templates templates)
        {
            if (id != templates.Id)
            {
                return NotFound();
            }

            List<JsonModel> jsonStr = new List<JsonModel>();

            //generate first json
            JsonModel item = new JsonModel { };
            item.Prompt = collection["prompt"];
            item.InputType = collection["input-type"];
            item.Order = collection["order"];
            item.HelperText = collection["helperText"];

            if (collection["input-type"] == "signature")
            {
                //get form data
                item.DatedSigned = "";
                item.Signed = false;

            }

            //TODO: separate following iff stmts to be a function that returns the serialized json by taking in the collection.
            if (collection["person"] == "student")
            {
                jsonStr = JsonConvert.DeserializeObject<List<JsonModel>>(templates.StudentQuestions);
                jsonStr.Add(item);
                string serializedJson = JsonConvert.SerializeObject(jsonStr);
                templates.StudentQuestions = serializedJson;
            }
            else if (collection["person"] == "employer")
            {
                jsonStr = JsonConvert.DeserializeObject<List<JsonModel>>(templates.EmployerQuestions);
                jsonStr.Add(item);
                templates.EmployerQuestions = JsonConvert.SerializeObject(jsonStr);
            }
            else if (collection["person"] == "faculty")
            {
                jsonStr = JsonConvert.DeserializeObject<List<JsonModel>>(templates.FacultyQuestions);
                jsonStr.Add(item);
                templates.FacultyQuestions = JsonConvert.SerializeObject(jsonStr);
            }
            else if (collection["person"] == "student-services")
            {
                jsonStr = JsonConvert.DeserializeObject<List<JsonModel>>(templates.StudentServicesQuestions);
                jsonStr.Add(item);
                templates.StudentServicesQuestions = JsonConvert.SerializeObject(jsonStr);
            }
            else if (collection["person"] == "admin")
            {
                jsonStr = JsonConvert.DeserializeObject<List<JsonModel>>(templates.AdministratorQuestions);
                jsonStr.Add(item);
                templates.AdministratorQuestions = JsonConvert.SerializeObject(jsonStr);
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
            _context.Templates.Remove(templates);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool TemplatesExists(int id)
        {
            return _context.Templates.Any(e => e.Id == id);
        }
    }
}
