#nullable disable
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Zawody.Data;
using Zawody.Models;

namespace Zawody.Controllers
{
    public class StadionsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public StadionsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Stadions
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.Stadiony.Include(s => s.Address);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: Stadions/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var stadion = await _context.Stadiony
                .Include(s => s.Address)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (stadion == null)
            {
                return NotFound();
            }

            return View(stadion);
        }

        // GET: Stadions/Create
        public IActionResult Create()
        {
            ViewData["AddressId"] = new SelectList(_context.Addresses, "Id", "Id");
            return View();
        }

        // POST: Stadions/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name,Pojemnosc,AddressId")] Stadion stadion)
        {
            if (ModelState.IsValid)
            {
                _context.Add(stadion);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["AddressId"] = new SelectList(_context.Addresses, "Id", "Id", stadion.AddressId);
            return View(stadion);
        }

        // GET: Stadions/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var stadion = await _context.Stadiony.FindAsync(id);
            if (stadion == null)
            {
                return NotFound();
            }
            ViewData["AddressId"] = new SelectList(_context.Addresses, "Id", "Id", stadion.AddressId);
            return View(stadion);
        }

        // POST: Stadions/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,Pojemnosc,AddressId")] Stadion stadion)
        {
            if (id != stadion.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(stadion);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!StadionExists(stadion.Id))
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
            ViewData["AddressId"] = new SelectList(_context.Addresses, "Id", "Id", stadion.AddressId);
            return View(stadion);
        }

        // GET: Stadions/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var stadion = await _context.Stadiony
                .Include(s => s.Address)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (stadion == null)
            {
                return NotFound();
            }

            return View(stadion);
        }

        // POST: Stadions/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var stadion = await _context.Stadiony.FindAsync(id);
            _context.Stadiony.Remove(stadion);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool StadionExists(int id)
        {
            return _context.Stadiony.Any(e => e.Id == id);
        }
    }
}
