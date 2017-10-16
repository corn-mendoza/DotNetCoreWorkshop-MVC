using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using DotNetCoreWorkshop_MVC.Models;

namespace DotNetCoreWorkshop_MVC.Controllers
{
    public class AttendeeController : Controller
    {
        private readonly AttendeeContext _context;

        public AttendeeController(AttendeeContext context)
        {
            _context = context;
        }

        // GET: AttendeeModels
        public async Task<IActionResult> Index()
        {
            return View(await _context.AttendeeModel.ToListAsync());
        }

        // GET: AttendeeModels/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var attendeeModel = await _context.AttendeeModel
                .SingleOrDefaultAsync(m => m.Id == id);
            if (attendeeModel == null)
            {
                return NotFound();
            }

            return View(attendeeModel);
        }

        // GET: AttendeeModels/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: AttendeeModels/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name,Email,Title,Department")] AttendeeModel attendeeModel)
        {
            if (ModelState.IsValid)
            {
                _context.Add(attendeeModel);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(attendeeModel);
        }

        // GET: AttendeeModels/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var attendeeModel = await _context.AttendeeModel.SingleOrDefaultAsync(m => m.Id == id);
            if (attendeeModel == null)
            {
                return NotFound();
            }
            return View(attendeeModel);
        }

        // POST: AttendeeModels/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,Email,Title,Department")] AttendeeModel attendeeModel)
        {
            if (id != attendeeModel.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(attendeeModel);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!AttendeeModelExists(attendeeModel.Id))
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
            return View(attendeeModel);
        }

        // GET: AttendeeModels/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var attendeeModel = await _context.AttendeeModel
                .SingleOrDefaultAsync(m => m.Id == id);
            if (attendeeModel == null)
            {
                return NotFound();
            }

            return View(attendeeModel);
        }

        // POST: AttendeeModels/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var attendeeModel = await _context.AttendeeModel.SingleOrDefaultAsync(m => m.Id == id);
            _context.AttendeeModel.Remove(attendeeModel);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool AttendeeModelExists(int id)
        {
            return _context.AttendeeModel.Any(e => e.Id == id);
        }
    }
}
