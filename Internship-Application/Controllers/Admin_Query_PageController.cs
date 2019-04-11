using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Internship_Application.Models;
using System;
using Newtonsoft.Json;
using System.Collections.Generic;
using Microsoft.AspNetCore.Http;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Internship_Application.Controllers
{
    public class Admin_Query_PageController : Controller
    {
        private readonly DataContext _context;

        public Admin_Query_PageController(DataContext context)
        {
            _context = context;
        }

        // GET: /<controller>/ and forms c:
        public IActionResult Index()
        {
            // forms is a list of all forms
            var forms = _context.Forms.ToList<Forms>();

            if (forms == null)
            {
                //TODO: move this to a function

                return View();
            }
            List<FormViewModel> serializedFormList = new List<FormViewModel>();
            foreach (var form in forms)
            {
                Templates template = _context.Templates.Find(form.TemplateId);
                serializedFormList.Add(new FormViewModel
                {
                    Id = form.Id,
                    //Form = JsonConvert.DeserializeObject<List<FormJsonModel>>(form.Answers),
                    //FormName = template.DisplayName,
                    //Disclaimer = template.Disclaimer,
                    StudentName = form.StudentName,
                    //StudentEmail = form.StudentEmail,
                    //FacultyEmail = form.FacultyEmail,
                    //EmployerEmail = form.EmployerEmail,
                    UpdatedAt = form.UpdatedAt,
                    StatusCodesViewModel = _context.StatusCodes.Find(form.StatusCodeId),

                });

            }
            List<Companies> companies = _context.Companies.ToList<Companies>();
            ViewBag.Companies = companies;
            ViewBag.Forms = serializedFormList;
            ViewBag.QueryByCompanyName = 0;


            //formList = forms;

            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        //public IActionResult QueryByCompanyName(IFormCollection formcollection)
        public IActionResult Index(IFormCollection formcollection)
        {
            List<FormViewModel> serializedFormList = new List<FormViewModel>();
            //if statement short circuits if key is not found
            if (formcollection.ContainsKey("CompanyName"))
            {
                ViewBag.QueryByCompanyName = Int32.Parse(formcollection["CompanyId"]);
                //TODO: refactor to have all code in another function and call that function here
                var forms = _context.Forms.Where(m => m.CompanyId == Int32.Parse(formcollection["CompanyId"])).ToList();
      
                foreach (var form in forms)
                {
                    Templates template = _context.Templates.Find(form.TemplateId);
                    serializedFormList.Add(new FormViewModel
                    {
                        Id = form.Id,
                        //Form = JsonConvert.DeserializeObject<List<FormJsonModel>>(form.Answers),
                        //FormName = template.DisplayName,
                        //Disclaimer = template.Disclaimer,
                        StudentName = form.StudentName,
                        //StudentEmail = form.StudentEmail,
                        //FacultyEmail = form.FacultyEmail,
                        //EmployerEmail = form.EmployerEmail,
                        UpdatedAt = form.UpdatedAt,
                        StatusCodesViewModel = _context.StatusCodes.Find(form.StatusCodeId),
                    });
                }

            }
            ViewBag.Companies = _context.Companies.ToList<Companies>();
            ViewBag.Forms = serializedFormList;


           // return View("../Admin_Query_Page/Index");
            return View();
        }

    }
}
