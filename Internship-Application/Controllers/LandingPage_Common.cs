using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Internship_Application.Models;
using Microsoft.AspNetCore.Authorization;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Internship_Application.Controllers
{
    public class LandingPage_Common : Controller
    {
        private readonly DataContext _context;

        public LandingPage_Common(DataContext context)
        {
            _context = context;
        }
        // GET: /<controller>/
        [Authorize(Roles = "Student")]
        public async Task<IActionResult> Index()
        {
            return View(await _context.Forms.ToListAsync());
        }
    }
}
