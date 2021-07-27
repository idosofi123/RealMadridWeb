using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using RealMadridWebApp.Data;
using RealMadridWebApp.ExternalServices;
using RealMadridWebApp.Models;

namespace RealMadridWebApp.Controllers
{
    public class PlayersController : Controller
    {
        private readonly RealMadridWebAppContext _context;



        public PlayersController(RealMadridWebAppContext context)
        {
            _context = context;
        }

        // GET: Players
        public async Task<IActionResult> Index()
        {
            var players = await _context.Player.Include(p => p.BirthCountry).Include(p => p.Position).ToListAsync();
            var groupedPlayersByPosition = players.OrderByDescending(p => p.PositionId).GroupBy(p => p.Position).Select(
                n => new PositionGroup { Key = n.Key, Players = n.Key.Players, count = n.Count() }).ToList();


            ViewData["countries"] = new SelectList(_context.Country, nameof(Country.CountryID), nameof(Country.CountryName));
            ViewData["GroupedPlayers"] = groupedPlayersByPosition;

            return View();
        }

        public async Task<IActionResult> Search(Foot? prefferedFoot, int[]? country, int minAge, int maxAge)
        {
            DateTime today = DateTime.Today;
            DateTime minDate = today.AddYears(-minAge);
            DateTime maxDate = today.AddYears(-maxAge);
            var players = await _context.Player.Include(p => p.BirthCountry).Include(p => p.Position)
                                                .Where(p =>  (country.Length == 0 || country.Contains(p.BirthCountryId) ) &&
                                                            ( prefferedFoot == null || p.PreferedFoot == prefferedFoot) &&
                                                            ( p.BirthDate <= minDate && p.BirthDate >= maxDate)).ToListAsync();

            /*var groupedPlayersByPosition = players.OrderByDescending(p=> p.PositionId).GroupBy(p => new PositionGroup { PositionId = p.Position.Id, PositionName = p.Position.PositionName }).ToList();*/
            var groupedPlayersByPosition = players.OrderByDescending(p => p.PositionId).GroupBy(p => p.Position).Select(
                n => new PositionGroup { Key = n.Key, Players = n.Key.Players, count = n.Count() }).ToList();

            return Json(groupedPlayersByPosition);

        }

        // GET: Players/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var player = await _context.Player
                .Include(p => p.BirthCountry)
                .Include(p => p.Position)
                .FirstOrDefaultAsync(m => m.PlayerId == id);
            if (player == null)
            {
                return NotFound();
            }

            return View(player);
        }

        // GET: Players/Create
        public IActionResult Create()
        {
            ViewData["BirthCountryId"] = new SelectList(_context.Set<Country>(), "CountryID", nameof(Country.CountryName));
            ViewData["PositionId"] = new SelectList(_context.Set<Position>(), "Id", nameof(Position.PositionName));
            return View();
        }

        // POST: Players/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("PlayerId,FirstName,LastName,ShirtNumber,ImagePath,PositionId,BirthDate,PreferedFoot,CountryId,BirthCountryId,Height,Weight")] Player player)
        {
            ModelState.Remove(nameof(Player.Position));
            ModelState.Remove(nameof(Player.BirthCountry));
            if (ModelState.IsValid)
            {
                _context.Add(player);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));          
            }
            ViewData["BirthCountryId"] = new SelectList(_context.Set<Country>(), "CountryID", nameof(Country.CountryName), player.BirthCountryId);
            ViewData["PositionId"] = new SelectList(_context.Set<Position>(), "Id", nameof(Position.PositionName), player.PositionId);
            return View(player);
        }

        // GET: Players/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var player = await _context.Player.FindAsync(id);
            if (player == null)
            {
                return NotFound();
            }
            ViewData["BirthCountryId"] = new SelectList(_context.Set<Country>(), "CountryID", nameof(Country.CountryName), player.BirthCountryId);
            ViewData["PositionId"] = new SelectList(_context.Set<Position>(), "Id", nameof(Position.PositionName), player.PositionId);
            return View(player);
        }

        // POST: Players/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("PlayerId,FirstName,LastName,ShirtNumber,ImagePath,PositionId,BirthDate,PreferedFoot,CountryId,BirthCountryId,Height,Weight")] Player player)
        {
            if (id != player.PlayerId)
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
                    if (!PlayerExists(player.PlayerId))
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
            ViewData["BirthCountryId"] = new SelectList(_context.Set<Country>(), "CountryID", nameof(Country.CountryName), player.BirthCountryId);
            ViewData["PositionId"] = new SelectList(_context.Set<Position>(), "Id", nameof(Position.PositionName), player.PositionId);
            return View(player);
        }

        // GET: Players/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var player = await _context.Player
                .Include(p => p.BirthCountry)
                .Include(p => p.Position)
                .FirstOrDefaultAsync(m => m.PlayerId == id);
            if (player == null)
            {
                return NotFound();
            }

            return View(player);
        }

        // POST: Players/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var player = await _context.Player.FindAsync(id);
            _context.Player.Remove(player);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool PlayerExists(int id)
        {
            return _context.Player.Any(e => e.PlayerId == id);
        }
    }
}
