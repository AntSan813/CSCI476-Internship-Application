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
    public class SignaturesController : Controller
    {
        private readonly CSCI476Context _context;

        public SignaturesController(CSCI476Context context)
        {
            _context = context;
        }

        // GET: Signatures
        public async Task<IActionResult> Index()
        {
            return View(await _context.Signatures.ToListAsync());
        }

        // GET: Signatures/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var signatures = await _context.Signatures
                .FirstOrDefaultAsync(m => m.Id == id);
            if (signatures == null)
            {
                return NotFound();
            }

            return View(signatures);
        }

        // GET: Signatures/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Signatures/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,CreatedAt,UpdatedAt,DeletedAt,StudentSigned,EmployerSigned,FacultySigned,StudentServicesSigned,DirectorSigned,StudentSignDate,EmployerSignDate,FacultySignDate,StudentServicesSignDate,DirectorSignDate")] Signatures signatures)
        {
            if (ModelState.IsValid)
            {
                _context.Add(signatures);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(signatures);
        }

        // GET: Signatures/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var signatures = await _context.Signatures.FindAsync(id);
            if (signatures == null)
            {
                return NotFound();
            }
            return View(signatures);
        }

        // POST: Signatures/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,CreatedAt,UpdatedAt,DeletedAt,StudentSigned,EmployerSigned,FacultySigned,StudentServicesSigned,DirectorSigned,StudentSignDate,EmployerSignDate,FacultySignDate,StudentServicesSignDate,DirectorSignDate")] Signatures signatures)
        {
            if (id != signatures.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(signatures);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!SignaturesExists(signatures.Id))
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
            return View(signatures);
        }

        // GET: Signatures/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var signatures = await _context.Signatures
                .FirstOrDefaultAsync(m => m.Id == id);
            if (signatures == null)
            {
                return NotFound();
            }

            return View(signatures);
        }

        // POST: Signatures/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var signatures = await _context.Signatures.FindAsync(id);
            _context.Signatures.Remove(signatures);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool SignaturesExists(int id)
        {
            return _context.Signatures.Any(e => e.Id == id);
        }
    }
}
