using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using RealMadridWebApp.Data;
using RealMadridWebApp.Models;

namespace RealMadridWebApp.Controllers {

    public class StadiumController : Controller {

        private readonly RealMadridWebAppContext _context;

        public StadiumController(RealMadridWebAppContext context) {
            _context = context;
        }

        // GET: Stadium
        public async Task<IActionResult> Index() {
            return View(await _context.Stadium.ToListAsync());
        }

        // GET: Stadium/Details/5
        public async Task<IActionResult> Details(int? id) {

            if (id == null) {
                return NotFound();
            }

            var stadium = await _context.Stadium.FirstOrDefaultAsync(m => m.Id == id);

            if (stadium == null) {
                return NotFound();
            }

            return View(stadium);
        }

        // GET: Stadium/Create
        public IActionResult Create() {
            return View();
        }

        // POST: Stadium/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name,Capacity")] Stadium stadium) {

            if (ModelState.IsValid) {
                _context.Add(stadium);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            return View(stadium);
        }

        // GET: Stadium/Edit/5
        public async Task<IActionResult> Edit(int? id) {

            if (id == null) {
                return NotFound();
            }

            var stadium = await _context.Stadium.FindAsync(id);

            if (stadium == null) {
                return NotFound();
            }

            return View(stadium);
        }

        // POST: Stadium/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,Capacity")] Stadium stadium) {

            if (id != stadium.Id) {
                return NotFound();
            }

            if (ModelState.IsValid) {

                try {
                    _context.Update(stadium);
                    await _context.SaveChangesAsync();

                } catch (DbUpdateConcurrencyException) {

                    if (!StadiumExists(stadium.Id)) {
                        return NotFound();
                    } else {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }

            return View(stadium);
        }

        // GET: Stadium/Delete/5
        public async Task<IActionResult> Delete(int? id) {

            if (id == null) {
                return NotFound();
            }

            var stadium = await _context.Stadium.FirstOrDefaultAsync(m => m.Id == id);

            if (stadium == null) {
                return NotFound();
            }

            return View(stadium);
        }

        // POST: Stadium/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id) {

            var stadium = await _context.Stadium.FindAsync(id);
            _context.Stadium.Remove(stadium);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        private bool StadiumExists(int id) {
            return _context.Stadium.Any(e => e.Id == id);
        }
    }
}
