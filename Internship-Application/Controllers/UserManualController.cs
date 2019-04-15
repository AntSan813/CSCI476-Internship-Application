using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Internship_Application.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System.Diagnostics;
using Microsoft.AspNetCore.Authorization;
using System.Net.Mail;


namespace Internship_Application.Controllers
{
    public class UserManualController : Controller
    {   
        public async Task<IActionResult> Index()
        { 
            return View();
        }
    }
}
