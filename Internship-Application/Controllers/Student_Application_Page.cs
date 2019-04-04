using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Internship_Application.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Internship_Application.Controllers
{
    public class Student_Application_Page : Controller
    {
        private readonly DataContext _context;

        public Student_Application_Page(DataContext context)
        {
            _context = context;
        }
        // GET: /<controller>/
        public IActionResult Index()
        {
            var template = _context.Templates
        .Where(b => b.IsActive == true)
        .FirstOrDefault();

            if (template == null)
            {
                return NotFound();
            }

            TemplateViewModel templateView = new TemplateViewModel { };
            templateView.Questions = JsonConvert.DeserializeObject<List<JsonModel>>(template.Questions);
            templateView.Templates[0] = template;
            return View(templateView);
        }
        // GET: Forms/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Forms/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(IFormCollection collection, Forms forms)
        {
            forms.WuId = collection["wu-id"];
            //first, convert the questions from the active template form to the answers attribute
            //algo:
            //1. go through each question in the active template form
            //2. append a new FormJsonModel for each question in the templates form, to the student's application form
            //  each FormJsonModel will corrlate to the respective template question from its order number, which cannot change if the template is active
            List<FormJsonModel> answers = new List<FormJsonModel> { };
         //***LEFT OFF DOING THIS ALGO***
            //second see if company is already in db
            //algo:
            //1. query db to see if company exists from company name
            //2a. if company is in companies table, then go to step 4
            //2b. else, add company to the companies table 
            //3. add company object to companies table, then pull that object from the table after saving it to get the ID
            //4. forms.CompanyId = company.Id
            Companies company = (Companies)_context.Companies.Where(b => b.CompanyName == collection["company-name"]);
            if (company == null)
            {
                //if not, then add it
                Companies newCompany = new Companies {
                    CompanyName = collection["company-name"],
                    CompanyLocation = collection["company-location"]
                };

                //add new company to db
                _context.Add(newCompany);
                //save changes
                await _context.SaveChangesAsync();

                //now rety at getting the company instance in the db (since we now need the id from it)
                company = (Companies)_context.Companies.Where(b => b.CompanyName == collection["company-name"]);
            }

            //now, tie the company id to the appropriate forms attribute
            forms.CompanyId = company.Id;
            forms.StatusCodeId = 1; //"currently being worked" on status code

            if (ModelState.IsValid)
            {
                _context.Add(forms);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(forms);
        }

    }
}
