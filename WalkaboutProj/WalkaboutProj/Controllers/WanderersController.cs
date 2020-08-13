using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using WalkaboutProj.Data;
using WalkaboutProj.Models;

namespace WalkaboutProj.Controllers
{
    public class WanderersController : Controller
    {
        private readonly ApplicationDbContext _context;

        public WanderersController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Wanderers
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.Wanderers.Include(w => w.IdentityUser);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: Wanderers/Details/5
        public IActionResult Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var wanderer = _context.Wanderers.Where(m => m.WandererId == id).FirstOrDefault();
            if (wanderer == null)
            {
                return NotFound();
            }

            return View(wanderer);
        }

        // GET: Wanderers/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Wanderers/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create([Bind("WandererId,IdentityUserId,Username,FirstName,LastName,PhoneNumber,ZipCode,UnitPreference,WeeklyPoints,WeeklyDistance")] Wanderer wanderer)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            wanderer.IdentityUserId = userId;
                _context.Add(wanderer);
                return RedirectToAction(nameof(Index));
        }

        // GET: Wanderers/Edit/5
        public IActionResult Edit()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var wanderer = _context.Wanderers.Where(c => c.IdentityUserId == userId).FirstOrDefault();

            return View(wanderer);
        }

        // POST: Wanderers/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(int id, [Bind("WandererId,IdentityUserId,Username,FirstName,LastName,PhoneNumber,ZipCode,UnitPreference,WeeklyPoints,WeeklyDistance")] Wanderer wanderer)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var wandererToUpdate = _context.Wanderers.Where(c => c.IdentityUserId == userId).SingleOrDefault();
           try
            {
                //all update logic
                _context.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!WandererExists(wandererToUpdate.WandererId))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }
            return RedirectToAction("Index");
        }

        // GET: Wanderers/Delete/5
        public IActionResult Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var wanderer = _context.Wanderers
                .Include(w => w.IdentityUser)
                .FirstOrDefaultAsync(m => m.WandererId == id);
            if (wanderer == null)
            {
                return NotFound();
            }

            return View(wanderer);
        }

        // POST: Wanderers/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(int id)
        {
            var wanderer = _context.Wanderers.FirstOrDefault(s => s.WandererId == id);
            _context.Wanderers.Remove(wanderer);
            _context.SaveChanges();
            return RedirectToAction(nameof(Index));
        }

        private bool WandererExists(int id)
        {
            return _context.Wanderers.Any(e => e.WandererId == id);
        }
    }
}
