﻿using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using RealMadridWebApp.Data;
using RealMadridWebApp.Models;
using TweetSharp;

namespace RealMadridWebApp.Controllers
{
    public class MatchesController : Controller {

        private readonly RealMadridWebAppContext _context;

        public MatchesController(RealMadridWebAppContext context) {
            _context = context;
        }

        // GET: Matches
        public async Task<IActionResult> Index() {

            var matches = await _context.Match.Include(m => m.Team).ToListAsync();

            // Group the queried matches by their scheduled year and month.
            ViewData["GroupedMatches"] = matches.OrderByDescending(m => m.Date)
                                                .GroupBy(m => new MonthGroup{ Year = m.Date.Year, Month = m.Date.ToString("MMMM", new CultureInfo("en-US")) }).ToList();

            return View(matches);
        }

        // GET: Matches/Details/5
        public async Task<IActionResult> Details(int? id) {

            if (id == null) {
                return NotFound();
            }

            var match = await _context.Match.Include(m => m.Team).FirstOrDefaultAsync(m => m.Id == id);

            if (match == null) {
                return NotFound();
            }

            return View(match);
        }

        // GET: Matches/Create
        public IActionResult Create() {
            ViewData["TeamId"] = new SelectList(_context.Team.Where(t => t.IsHome == false), "Id", "Name");
            return View();
        }

        // POST: Matches/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,TeamId,isAwayMatch,Date,HomeGoals,AwayGoals")] Match match) {

            if (ModelState.IsValid) {

                // Validate that the given team ID does not represent our home team.
                Team givenTeam = await _context.Team.FirstOrDefaultAsync(t => t.Id == match.TeamId);

                if (givenTeam.IsHome == false) {

                    _context.Add(match);
                    await _context.SaveChangesAsync();

                    // If the created match is an upcoming match, post an announcement tweet about it.
                    if (match.Date > DateTime.Now) {

                        // Connect to the service and authenticate.
                        var service = new TwitterService(Keys.TweeterAPIKey, Keys.TweeterAPISecretKey);
                        service.AuthenticateWith(Keys.TweeterToken, Keys.TweeterSecretToken);

                        // Read the newly saved match data, including the data of the corresponding team.
                        Match savedMatch = await _context.Match.Include(m => m.Team).FirstOrDefaultAsync(m => m.Id == match.Id);

                        if (savedMatch != null) {

                            // Post the tweet.
                            service.SendTweet(new SendTweetOptions {
                                Status = $"Real Madrid will match against {savedMatch.Team.Name} on {savedMatch.Date.ToShortDateString()}. Order your tickets now!"
                            });
                        }
                    }

                    return RedirectToAction(nameof(Index));

                } else {
                    ViewData["Error"] = "A match against our home team cannot be created.";
                }
            }

            ViewData["TeamId"] = new SelectList(_context.Team, "Id", "Name", match.TeamId);
            return View(match);
        }

        // GET: Matches/Edit/5
        public async Task<IActionResult> Edit(int? id) {

            if (id == null) {
                return NotFound();
            }

            var match = await _context.Match.FindAsync(id);

            if (match == null) {
                return NotFound();
            }

            ViewData["TeamId"] = new SelectList(_context.Team, "Id", "Name", match.TeamId);
            return View(match);
        }

        // POST: Matches/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,TeamId,isAwayMatch,Date,HomeGoals,AwayGoals")] Match match) {

            if (id != match.Id) {
                return NotFound();
            }

            if (ModelState.IsValid) {

                try {

                    _context.Update(match);
                    await _context.SaveChangesAsync();

                } catch (DbUpdateConcurrencyException) {
                    
                    if (!MatchExists(match.Id)) {
                        return NotFound();
                    } else {
                        throw;
                    }
                }

                return RedirectToAction(nameof(Index));
            }

            ViewData["TeamId"] = new SelectList(_context.Team, "Id", "Name", match.TeamId);
            return View(match);
        }

        // GET: Matches/Delete/5
        public async Task<IActionResult> Delete(int? id) {

            if (id == null) {
                return NotFound();
            }

            var match = await _context.Match.Include(m => m.Team).FirstOrDefaultAsync(m => m.Id == id);

            if (match == null) {
                return NotFound();
            }

            return View(match);
        }

        // POST: Matches/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id) {

            var match = await _context.Match.FindAsync(id);
            _context.Match.Remove(match);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool MatchExists(int id) {
            return _context.Match.Any(e => e.Id == id);
        }
    }
}
