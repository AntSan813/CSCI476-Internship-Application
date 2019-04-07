using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Internship_Application.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Internship_Application.Controllers
{
    public class Student_Application_Page : Controller
    {
        private readonly DataContext _context;
        //private readonly UserManager<IdentityUser> _userManager;
        //private Task<IdentityUser> GetCurrentUserAsync() => _userManager.GetUserAsync(HttpContext.User);

        public Student_Application_Page(DataContext context)
        {
            _context = context;
        }
        // GET: /<controller>/
        public async Task<IActionResult> Index()
        {
            //first, get user data
            //var user = await GetCurrentUserAsync();
            //var userId = user?.Id;
            string studentEmail = User.Identity.Name;

            //second, grab all forms where the emails match
            var forms = await _context.Forms.FirstOrDefaultAsync(b => b.StudentEmail == studentEmail);
            if (forms == null)
            {
                return View(new List<Forms> { });
            }
            return View(forms);

            //FormViewModel formView = new FormViewModel {  };

            ////third, check if any forms were pulled
            //if (forms == null)
            //{
            //    //if no forms exist, literally do nothing. 
            //    //all we want to do it state no forms were found and that the user has
            //    //to create one 
            //}
            //else
            //{
            //    //if there is a form, then we want to update our formView
            //    formView.Forms = new List<Forms>(forms);
            //}


            //var template = _context.Templates
            //    .Where(b => b.IsActive == true)
            //    .FirstOrDefault();

            //if (template == null)
            //{
            //    return NotFound();
            //}

            //TemplateViewModel templateView = new TemplateViewModel { };
            //templateView.Questions = JsonConvert.DeserializeObject<List<JsonModel>>(template.Questions);
            //templateView.Templates[0] = template;
            //return View(formView);
        }


        // GET: Forms/Edit
        //new forms go here
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int? id)
        {
            FormViewModel formView = new FormViewModel { };

            //goes in the the following if statement 
            if (id == null)
            {
                //first, get user data
                //var user = await GetCurrentUserAsync();
                //var userId = user?.Id;
                string studentEmail = User.Identity.Name;

                Forms newForm = new Forms { };
                newForm.Answers = "[]";
                newForm.StudentEmail = studentEmail;


                //first, get most active template from db
                Templates template = _context.Templates
                    .First(m => m.IsActive == true);

                newForm.TemplateId = template.Id;

                //then, get the list of questions from that template
                List<JsonModel> templateQuestions = new List<JsonModel>(JsonConvert.DeserializeObject<List<JsonModel>>(template.Questions));
                List<FormJsonModel> formItems = new List<FormJsonModel>();

                foreach (var item in templateQuestions)
                {
                    formItems.Add(new FormJsonModel
                    {
                        //question info
                        Order = item.Order,
                        Prompt = item.Prompt,
                        PromptValue = "",
                        HelperText = item.HelperText,
                        InputType = item.InputType,

                        //type vars
                        DateSigned = item.DateSigned,
                        Options = item.Options,
                        Signed = item.Signed,
                        Required = item.Required,

                        //who can edit question
                        Person = item.Person
                    });
                }

                formView.Form = new List<FormJsonModel>(formItems);
                newForm.Answers = JsonConvert.SerializeObject(formItems);

                //also get the disclaimer and template name
                formView.Disclaimer = template.Disclaimer;
                formView.FormName = template.DisplayName;

                if (ModelState.IsValid)
                {
                    _context.Add(newForm);
                    await _context.SaveChangesAsync();
                    // return RedirectToAction(nameof(Index));
                }

            }
            
            return View(formView);
        }
        

        //// GET: Forms/Edit/5
        //public async Task<IActionResult> Edit(int? id)
        //{
        //    if (id == null)
        //    {
        //        return NotFound();
        //    }

        //    //first, get the current users form from the db
        //    var forms = await _context.Forms.FindAsync(id);
        //    FormViewModel formView = new FormViewModel { };
            

        //    if (forms == null)
        //    {
        //        return NotFound();
        //    }
        //    return View(forms);
        //}

        // POST: Forms/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public async Task<IActionResult> Edit(int id, Forms forms)
        //{
        //    if (id != forms.Id)
        //    {
        //        return NotFound();
        //    }

        //    if (ModelState.IsValid)
        //    {
        //        try
        //        {
        //            _context.Update(forms);
        //            await _context.SaveChangesAsync();
        //        }
        //        catch (DbUpdateConcurrencyException)
        //        {
        //            //if (!FormsExists(forms.Id))
        //            //{
        //            //    return NotFound();
        //            //}
        //            //else
        //            //{
        //            //    throw;
        //            //}
        //        }
        //        return RedirectToAction(nameof(Index));
        //    }
        //    return View(forms);
        //}

        //// POST: Forms/Create
        //// To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        //// more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public async Task<IActionResult> Create(IFormCollection collection, Forms forms)
        //{
        //    forms.WuId = collection["wu-id"];
        //    //first, convert the questions from the active template form to the answers attribute
        //    //algo:
        //    //1. go through each question in the active template form
        //    //2. append a new FormJsonModel for each question in the templates form, to the student's application form
        //    //  each FormJsonModel will corrlate to the respective template question from its order number, which cannot change if the template is active
        //    List<FormJsonModel> answers = new List<FormJsonModel> { };
        //    //***LEFT OFF DOING THIS ALGO***
        //    //second see if company is already in db
        //    //algo:
        //    //1. query db to see if company exists from company name
        //    //2a. if company is in companies table, then go to step 4
        //    //2b. else, add company to the companies table 
        //    //3. add company object to companies table, then pull that object from the table after saving it to get the ID
        //    //4. forms.CompanyId = company.Id
        //    Companies company = (Companies)_context.Companies.Where(b => b.CompanyName == collection["company-name"]);
        //    if (company == null)
        //    {
        //        //if not, then add it
        //        Companies newCompany = new Companies
        //        {
        //            CompanyName = collection["company-name"],
        //            CompanyLocation = collection["company-location"]
        //        };

        //        //add new company to db
        //        _context.Add(newCompany);
        //        //save changes
        //        await _context.SaveChangesAsync();

        //        //now rety at getting the company instance in the db (since we now need the id from it)
        //        company = (Companies)_context.Companies.Where(b => b.CompanyName == collection["company-name"]);
        //    }

        //    //now, tie the company id to the appropriate forms attribute
        //    forms.CompanyId = company.Id;
        //    forms.StatusCodeId = 1; //"currently being worked" on status code

        //    if (ModelState.IsValid)
        //    {
        //        _context.Add(forms);
        //        await _context.SaveChangesAsync();
        //        return RedirectToAction(nameof(Index));
        //    }
        //    return View(forms);
        //}
    }
}
