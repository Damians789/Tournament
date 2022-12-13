#nullable disable
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
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
using Microsoft.AspNetCore.Identity;

namespace Zawody.Controllers
{
    [Authorize]
    public class PlayersController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IAuthorizationService _authorizationService;
        private readonly IUserContextService _userContextService;

        public PlayersController(ApplicationDbContext context, IUserContextService userContextService, IAuthorizationService authorizationService)
        {
            _context = context;
            _userContextService = userContextService;
            _authorizationService = authorizationService;

        }

        // GET: Players
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

            var players = from p in _context.Players select p;

            if (!String.IsNullOrEmpty(searchString))
            {
                players = players.Where(p => p.LastName.Contains(searchString)
                                       || p.FirstName.Contains(searchString)).Include(p => p.Team);
            }

            players = sortOrder switch
            {
                "name_desc" => players.OrderByDescending(s => s.LastName),
                "Date" => players.OrderBy(s => s.Pozycja),
                "date_desc" => players.OrderByDescending(s => s.Pozycja),
                _ => players.OrderBy(s => s.LastName),
            };
            int pageSize = 3;
            /*var applicationDbContext = _context.Players.Include(p => p.Team);*/
            /*return View(await applicationDbContext.ToListAsync());*/
            return View(await PaginatedList<Player>.CreateAsync(players.AsNoTracking(), pageNumber ?? 1, pageSize));
        }

        // GET: Players/Details/5
        [AllowAnonymous]
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var player = await _context.Players
                .Include(p => p.Team)
                .FirstOrDefaultAsync(m => m.ID == id);
            if (player == null)
            {
                return NotFound();
            }

            return View(player);
        }

        // GET: Players/Create
        [Authorize(Roles = "SuperAdmin, Trener")]
        public IActionResult Create()
        {
            ViewData["TeamID"] = new SelectList(_context.Teams, "Id", "Name");
            return View();
        }

        // POST: Players/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("TeamID,Pozycja,ID,LastName,FirstName,DateOfBirth")] Player player)
        {
            if (ModelState.IsValid)
            {
                player.CreatedById = _userContextService.GetUserId;
                _context.Add(player);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["TeamID"] = new SelectList(_context.Teams, "Id", "Name", player.TeamID);
            return View(player);
        }

        // GET: Players/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var player = await _context.Players.FindAsync(id);
            if (player == null)
            {
                return NotFound();
            }

            var authorizationResult = _authorizationService.AuthorizeAsync(_userContextService.User, player,
                    new ResourceOperationRequirement(ResourceOperation.Update)).Result;

            if (!authorizationResult.Succeeded)
            {
                throw new ForbidException();
            }

            ViewData["TeamID"] = new SelectList(_context.Teams, "Id", "Name", player.TeamID);
            return View(player);
        }

        // POST: Players/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("TeamID,Pozycja,ID,LastName,FirstName,DateOfBirth")] Player player)
        {
            if (id != player.ID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(player);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!PlayerExists(player.ID))
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
            ViewData["TeamID"] = new SelectList(_context.Teams, "Id", "Name", player.TeamID);
            return View(player);
        }

        // GET: Players/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var player = await _context.Players
                .Include(p => p.Team)
                .FirstOrDefaultAsync(m => m.ID == id);


            if (player == null)
            {
                return NotFound();
            }

            var authorizationResult = _authorizationService.AuthorizeAsync(_userContextService.User, player,
               new ResourceOperationRequirement(ResourceOperation.Delete)).Result;

            if (!authorizationResult.Succeeded)
            {
                throw new ForbidException();
            }

            return View(player);
        }

        // POST: Players/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var player = await _context.Players.FindAsync(id);

            _context.Players.Remove(player);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool PlayerExists(int id)
        {
            return _context.Players.Any(e => e.ID == id);
        }
    }
}
