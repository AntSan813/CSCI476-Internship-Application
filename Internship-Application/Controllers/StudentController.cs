using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Internship_Application.Controllers
{

    [Authorize(Roles = "Student")]//only allow if student role
    public class StudentController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}