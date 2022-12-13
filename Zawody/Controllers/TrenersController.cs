#nullable disable
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Zawody.Authorization;
using Zawody.Data;
using Zawody.Exceptions;
using Zawody.Models;
using Zawody.Services;

namespace Zawody.Controllers
{
    [Authorize]
    public class TrenersController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IAuthorizationService _authorizationService;
        private readonly IUserContextService _userContextService;

        public TrenersController(ApplicationDbContext context, IUserContextService userContextService, IAuthorizationService authorizationService)
        {
            _context = context;
            _userContextService = userContextService;
            _authorizationService = authorizationService;
        }

        // GET: Treners
        [AllowAnonymous]
        public async Task<IActionResult> Index(string sortOrder, string currentFilter, string searchString, int? pageNumber)
        {
            ViewData["CurrentSort"] = sortOrder;
            ViewData["NameSortParm"] = String.IsNullOrEmpty(sortOrder) ? "name_desc" : "";
            ViewData["DateSortParm"] = sortOrder == "Date" ? "date_desc" : "Date";

            if (searchString != null)
            {
                pageNumber = 1;
            }
            else
            {
                searchString = currentFilter;
            }

            ViewData["CurrentFilter"] = searchString;

            var treners = from t in _context.Trenerzy select t;

            if (!String.IsNullOrEmpty(searchString))
            {
                treners = treners.Where(t => t.LastName.Contains(searchString)
                                       || t.FirstName.Contains(searchString)).Include(t => t.Team);
            }

            treners = sortOrder switch
            {
                "name_desc" => treners.OrderByDescending(t => t.LastName),
                "Date" => treners.OrderBy(t => t.HireDate),
                "date_desc" => treners.OrderByDescending(t => t.HireDate),
                _ => treners.OrderBy(t => t.LastName),
            };
            int pageSize = 3;
            /*return View(await _context.Trenerzy.ToListAsync());*/
            return View(await PaginatedList<Trener>.CreateAsync(treners.AsNoTracking(), pageNumber ?? 1, pageSize));
        }

        // GET: Treners/Details/5
        [AllowAnonymous]
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var trener = await _context.Trenerzy
                .FirstOrDefaultAsync(m => m.ID == id);
            if (trener == null)
            {
                return NotFound();
            }

            return View(trener);
        }

        // GET: Treners/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Treners/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("HireDate,ID,LastName,FirstName,DateOfBirth")] Trener trener)
        {
            if (ModelState.IsValid)
            {
                trener.CreatedById = _userContextService.GetUserId;
                _context.Add(trener);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(trener);
        }

        // GET: Treners/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var trener = await _context.Trenerzy.FindAsync(id);
            if (trener == null)
            {
                return NotFound();
            }

            var authorizationResult = _authorizationService.AuthorizeAsync(_userContextService.User, trener,
                    new ResourceOperationRequirement(ResourceOperation.Update)).Result;

            if (!authorizationResult.Succeeded)
            {
                throw new ForbidException();
            }

            return View(trener);
        }

        // POST: Treners/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("HireDate,ID,LastName,FirstName,DateOfBirth")] Trener trener)
        {
            if (id != trener.ID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(trener);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!TrenerExists(trener.ID))
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
            return View(trener);
        }

        // GET: Treners/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var trener = await _context.Trenerzy
                .FirstOrDefaultAsync(m => m.ID == id);
            if (trener == null)
            {
                return NotFound();
            }

            var authorizationResult = _authorizationService.AuthorizeAsync(_userContextService.User, trener,
                    new ResourceOperationRequirement(ResourceOperation.Update)).Result;

            if (!authorizationResult.Succeeded)
            {
                throw new ForbidException();
            }


            return View(trener);
        }

        // POST: Treners/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var trener = await _context.Trenerzy.FindAsync(id);
            _context.Trenerzy.Remove(trener);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool TrenerExists(int id)
        {
            return _context.Trenerzy.Any(e => e.ID == id);
        }
    }
}
