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
    public class OfficeDetailsController : Controller
    {
        private readonly CSCI476Context _context;

        public OfficeDetailsController(CSCI476Context context)
        {
            _context = context;
        }

        // GET: OfficeDetails
        public async Task<IActionResult> Index()
        {
            return View(await _context.OfficeDetails.ToListAsync());
        }

        // GET: OfficeDetails/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var officeDetails = await _context.OfficeDetails
                .FirstOrDefaultAsync(m => m.Id == id);
            if (officeDetails == null)
            {
                return NotFound();
            }

            return View(officeDetails);
        }

        // GET: OfficeDetails/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: OfficeDetails/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,CreatedAt,UpdatedAt,DeletedAt,DateReceived,CorrespondenceSentEmployer,CorrespondenceSentStudent,EstMidPoint,MeetsClassReq,Classes,MeetsGpaReq,Gpa,MeetsPosReq,Pos,Other")] OfficeDetails officeDetails)
        {
            if (ModelState.IsValid)
            {
                _context.Add(officeDetails);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(officeDetails);
        }

        // GET: OfficeDetails/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var officeDetails = await _context.OfficeDetails.FindAsync(id);
            if (officeDetails == null)
            {
                return NotFound();
            }
            return View(officeDetails);
        }

        // POST: OfficeDetails/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,CreatedAt,UpdatedAt,DeletedAt,DateReceived,CorrespondenceSentEmployer,CorrespondenceSentStudent,EstMidPoint,MeetsClassReq,Classes,MeetsGpaReq,Gpa,MeetsPosReq,Pos,Other")] OfficeDetails officeDetails)
        {
            if (id != officeDetails.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(officeDetails);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!OfficeDetailsExists(officeDetails.Id))
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
            return View(officeDetails);
        }

        // GET: OfficeDetails/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var officeDetails = await _context.OfficeDetails
                .FirstOrDefaultAsync(m => m.Id == id);
            if (officeDetails == null)
            {
                return NotFound();
            }

            return View(officeDetails);
        }

        // POST: OfficeDetails/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var officeDetails = await _context.OfficeDetails.FindAsync(id);
            _context.OfficeDetails.Remove(officeDetails);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool OfficeDetailsExists(int id)
        {
            return _context.OfficeDetails.Any(e => e.Id == id);
        }
    }
}
