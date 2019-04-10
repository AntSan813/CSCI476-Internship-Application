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
            //FormViewModel formView = new FormViewModel { };
            //FormViewModel.Questions = JsonConvert.DeserializeObject<List<JsonModel>>(forms.Questions);
            List<Forms> formList = new List<Forms>(forms);
            //formList = forms;

            return View(formList);
        }
    }
}
