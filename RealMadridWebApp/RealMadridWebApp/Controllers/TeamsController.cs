using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using RealMadridWebApp.Data;
using RealMadridWebApp.Models;

namespace RealMadridWebApp.Controllers
{
    public class TeamsController : Controller
    {
        private readonly RealMadridWebAppContext _context;

        public TeamsController(RealMadridWebAppContext context) {
            _context = context;
        }

        // GET: Teams
        [Authorize(Roles = "Admin,Manager")]
        public async Task<IActionResult> Index() {
            var realMadridWebAppContext = _context.Team.Include(t => t.Stadium);
            return View(await realMadridWebAppContext.ToListAsync());
        }

        // GET: Teams/Details/5
        [Authorize(Roles = "Admin,Manager")]
        public async Task<IActionResult> Details(int? id) {

            if (id == null) {
                return NotFound();
            }
            var team = await _context.Team.Include(t => t.Stadium).FirstOrDefaultAsync(m => m.Id == id);
            if (team == null) {
                return NotFound();
            }
            return View(team);
        }

        // GET: Teams/Create
        [Authorize(Roles = "Admin,Manager")]
        public IActionResult Create() {
            ViewData["StadiumId"] = new SelectList(_context.Stadium, "Id", "Name");
            return View();
        }

        // POST: Teams/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin,Manager")]
        public async Task<IActionResult> Create([Bind("Id,Name,StadiumId,ImagePath")] Team team) {

            if (ModelState.IsValid) {
                _context.Add(team);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            ViewData["StadiumId"] = new SelectList(_context.Stadium, "Id", "Name", team.StadiumId);
            return View(team);
        }

        // GET: Teams/Edit/5
        [Authorize(Roles = "Admin,Manager")]
        public async Task<IActionResult> Edit(int? id) {

            if (id == null) {
                return NotFound();
            }

            var team = await _context.Team.FindAsync(id);
            if (team == null) {
                return NotFound();
            }

            ViewData["StadiumId"] = new SelectList(_context.Stadium, "Id", "Name", team.StadiumId);
            return View(team);
        }

        // POST: Teams/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin,Manager")]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,StadiumId,ImagePath")] Team team) {

            if (id != team.Id) {
                return NotFound();
            }

            if (ModelState.IsValid) {

                try {

                    _context.Update(team);
                    await _context.SaveChangesAsync();

                } catch (DbUpdateConcurrencyException) {

                    if (!TeamExists(team.Id)) {
                        return NotFound();
                    } else {
                        throw;
                    }
                }

                return RedirectToAction(nameof(Index));
            }
            ViewData["StadiumId"] = new SelectList(_context.Stadium, "Id", "Name", team.StadiumId);
            return View(team);
        }

        // GET: Teams/Delete/5
        [Authorize(Roles = "Admin,Manager")]
        public async Task<IActionResult> Delete(int? id) {

            if (id == null) {
                return NotFound();
            }

            var team = await _context.Team.Include(t => t.Stadium).FirstOrDefaultAsync(m => m.Id == id);

            if (team == null) {
                return NotFound();
            }

            return View(team);
        }

        // POST: Teams/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin,Manager")]
        public async Task<IActionResult> DeleteConfirmed(int id) {

            var team = await _context.Team.FindAsync(id);
            _context.Team.Remove(team);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool TeamExists(int id) {
            return _context.Team.Any(e => e.Id == id);
        }
    }
}
