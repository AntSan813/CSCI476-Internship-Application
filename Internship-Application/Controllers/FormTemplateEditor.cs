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
            var templates = _context.Templates.ToList<Templates>();

            if (templates == null)
            {
                //TODO: move this to a function

                return View();
            }
            ViewBag.Templates = templates;
            return View();
            //return View(templates);
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
        public async Task<IActionResult> Create(/*[Bind("IsActive,TemplateName,DisplayName,Questions,Disclaimer")] */ Templates template)
        {
            //Required questions in the form: student name, Winthrop id, employer email, and faculty of record email

            //START OF ADDING DEFAULT QUESTIONS
            //try to copy from active template
            Templates temp = _context.Templates
                   .First(m => m.IsActive == true);
            if (temp != null)
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
                return RedirectToAction("Index", "Admin_TemplatesOverviewPage");
            }
            var errors = ModelState.Select(x => x.Value.Errors)
                           .Where(y => y.Count > 0)
                           .ToList();

            return View(template);
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

            if(template == null)
            {
                return NotFound();
            }

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
        public IActionResult Edit(int id, IFormCollection collection, [Bind("Templates,Questions")] TemplateViewModel templateView)
        {

            Console.WriteLine(ViewBag);      
            if (id != templateView.Templates[0].Id)
            {
                return NotFound();
            }


            List<JsonModel> questionModel = JsonConvert.DeserializeObject<List<JsonModel>>(templateView.Templates[0].Questions);
            var order = questionModel.Count+1;
            if((Int32.Parse(collection["order"]) < order) && (Int32.Parse(collection["order"]) > 0))
            { //pretty much if the user entered a valid order number, then let the order be that
                order = Int32.Parse(collection["order"]);
            }
            
            //TODO: Add checks to make sure data was inputted correctly before storing the information
            //E.g. add a check to see if the order is valid (order must be within 1 and len(questions) + 1. 
            questionModel.Insert(order-1,new JsonModel
            {
                Prompt = collection["prompt"],
                InputType = collection["input-type"],
                Order = order,
                HelperText = collection["helper-text"],
                DateSigned = "",
                Signed = false,
                Options = new List<string> { },
                Required = false, //TODO: change this to take in a field from the collection
                Person = collection["person"] //TODO: change this to take in a field from the collection,
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
