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
            var forms = _context.Forms.ToList<Forms>();

            if(forms == null)
            {
                return View();
            }

            FormViewModel formView = new FormViewModel { };

            /*for(var i=0; i < formView.Forms.Count; i++)
            {
                forms[i].StudentQuestions = JSON.parse(forms[i].StudentQuestions);
            }*/
            //formView.StudentQuestions = JsonConvert.DeserializeObject<List<JsonModel>>(forms.StudentQuestions);

            formView.Forms = forms;
            return View(formView);
        }
    }
}
