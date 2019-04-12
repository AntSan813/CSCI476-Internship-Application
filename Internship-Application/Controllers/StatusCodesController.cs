using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Internship_Application.Models;

namespace Internship_Application.Controllers
{
    public class StatusCodesController : Controller
    {
        private readonly DataContext _context;

        public StatusCodesController(DataContext context)
        {
            _context = context;
        }

        // GET: StatusCodes
        public async Task<IActionResult> Index()
        {
            return View(await _context.StatusCodes.ToListAsync());
        }

        // GET: StatusCodes/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var statusCodes = await _context.StatusCodes
                .FirstOrDefaultAsync(m => m.Id == id);
            if (statusCodes == null)
            {
                return NotFound();
            }

            return View(statusCodes);
        }

        // GET: StatusCodes/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: StatusCodes/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,StatusCode,Details")] StatusCodes statusCodes)
        {
            if (ModelState.IsValid)
            {
                _context.Add(statusCodes);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(statusCodes);
        }

        // GET: StatusCodes/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var statusCodes = await _context.StatusCodes.FindAsync(id);
            if (statusCodes == null)
            {
                return NotFound();
            }
            return View(statusCodes);
        }

        // POST: StatusCodes/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,StatusCode,Details")] StatusCodes statusCodes)
        {
            if (id != statusCodes.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(statusCodes);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!StatusCodesExists(statusCodes.Id))
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
            return View(statusCodes);
        }

        // GET: StatusCodes/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var statusCodes = await _context.StatusCodes
                .FirstOrDefaultAsync(m => m.Id == id);
            if (statusCodes == null)
            {
                return NotFound();
            }

            return View(statusCodes);
        }

        // POST: StatusCodes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var statusCodes = await _context.StatusCodes.FindAsync(id);
            _context.StatusCodes.Remove(statusCodes);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool StatusCodesExists(int id)
        {
            return _context.StatusCodes.Any(e => e.Id == id);
        }
    }
}
