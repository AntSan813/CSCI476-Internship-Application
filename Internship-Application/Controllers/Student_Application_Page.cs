using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Internship_Application.Models;
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
    }
}
