using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.EntityFrameworkCore;
using RealMadridWebApp.Data;
using RealMadridWebApp.ExternalServices;
using RealMadridWebApp.Models;
using TweetSharp;


namespace RealMadridWebApp.Controllers
{
    public struct GraphData
    {
        public DateTime FullDate { get; set; }
        public int? HomeGoals { get; set; }
        public int? AwayGoals { get; set; }
    }

    public class MatchesController : Controller {

        private readonly RealMadridWebAppContext _context;

        public MatchesController(RealMadridWebAppContext context) {
            _context = context;
        }

        // GET: Matches
        [Authorize]
        public async Task<IActionResult> Index() {

            var matches = await _context.Match.Include(m => m.Team).Include(m => m.Competition).ToListAsync();

            // Group the queried matches by their scheduled year and month.
            ViewData["GroupedMatches"] = matches.OrderByDescending(m => m.Date)
                                                .GroupBy(m => new MonthGroup{ Year = m.Date.Year, Month = m.Date.ToString("MMMM", new CultureInfo("en-US")) }).ToList();

            ViewData["TeamId"] = new SelectList(_context.Team.Where(t => !t.IsHome), "Id", "Name");
            ViewData["CompetitionId"] = new SelectList(_context.Competition, "Id", "Name");

            ViewData["HomeTeam"] = await _context.Team.Where(t => t.IsHome).FirstOrDefaultAsync();
            return View(matches);
        }

        // GET: Search matches
        [Authorize]
        public async Task<IActionResult> Search(int? teamId, int? competitionId, DateTime? fromDate, DateTime? toDate) {

            fromDate ??= DateTime.MinValue;
            toDate ??= DateTime.MaxValue;

            var matches = await _context.Match.Include(m => m.Team).Include(m => m.Competition)
                                              .Where(m => (teamId == null        || m.TeamId == teamId)
                                                       && (competitionId == null || m.CompetitionId == competitionId)
                                                       && m.Date >= fromDate
                                                       && m.Date <= toDate).ToListAsync();

            var groupedMatches = matches.OrderByDescending(m => m.Date)
                                        .GroupBy(m => new MonthGroup { Year = m.Date.Year, Month = m.Date.ToString("MMMM", new CultureInfo("en-US")) }).ToList();

            return Json(groupedMatches);
        }

        public async Task<IActionResult> GetNextMatch()
        {
            return Json(await _context.Match.Include(m => m.Team).Include(m => m.Competition).Where(m => m.Date >= DateTime.Now).OrderBy(m => m.Date).FirstAsync());
        }


        public async Task<IActionResult> getGraphValues()
        {
            List<GraphData> data = new List<GraphData>();

            var matches = await _context.Match.Include(m => m.Team).Include(m => m.Competition).Where(m => m.Date <= DateTime.Now && m.Date >= DateTime.Now.AddMonths(-5)).OrderBy(m => m.Date).ToListAsync();

            var groupedMatches = matches.OrderByDescending(m => m.Date)
                                                .GroupBy(m => new MonthGroup { Year = m.Date.Year, Month = m.Date.ToString("MMMM", new CultureInfo("en-US")) }).ToList();

            foreach (var grpMatch in groupedMatches)
            {
                GraphData item = new GraphData();
                item.HomeGoals = 0;
                item.AwayGoals = 0;
                foreach (Match match in grpMatch)
                {
                    item.FullDate = (item.FullDate == default(DateTime)) ? new DateTime(match.Date.Year, match.Date.Month, 1) : item.FullDate;

                    if(match.isAwayMatch)
                    {
                        item.AwayGoals += (match.AwayGoals == null)? 0 : match.AwayGoals;
                    }
                    else
                    {
                        item.HomeGoals += (match.HomeGoals == null) ? 0 :match.HomeGoals;
                    }
                    
                }
                data.Add(item);
            }

            return Json(data.ToArray());
        }

        // GET: Matches/Details/5
        [Authorize]
        public async Task<IActionResult> Details(int? id) {

            if (id == null) {
                return NotFound();
            }
            var match = await _context.Match.Include(m => m.Users)
                                            .Include(m => m.Competition)
                                            .Include(m => m.Team).ThenInclude(t => t.Stadium)
                                            .FirstOrDefaultAsync(m => m.Id == id);

            if (match == null) {
                return NotFound();
            }

            var homeTeam = await _context.Team.Include(t => t.Stadium).Where(t => t.IsHome).FirstOrDefaultAsync();

            // Derive the stadium that match will occur at.
            var stadium = (match.isAwayMatch) ? match.Team.Stadium : homeTeam.Stadium;

            ViewData["HomeTeam"] = homeTeam;
            ViewData["Stadium"] = stadium;

            // Calculate the amount of tickets left.
            ViewData["TicketsLeft"] = stadium.Capacity - match.Users.Count;

            var loggedInUser = await _context.User.Where(u => u.Username == HttpContext.User.Identity.Name).FirstOrDefaultAsync();

            ViewData["HasAlreadyPurchased"] = match.Users.Contains(loggedInUser);

            // Collect match history statistics -
            ViewData["MatchesWon"] = await _context.Match.Where(m => m.TeamId == match.TeamId
                                                                  && ((m.isAwayMatch && m.AwayGoals > m.HomeGoals) || (!m.isAwayMatch && m.HomeGoals > m.AwayGoals))).CountAsync();

            ViewData["MatchesLost"] = await _context.Match.Where(m => m.TeamId == match.TeamId
                                                                  && ((m.isAwayMatch && m.AwayGoals < m.HomeGoals) || (!m.isAwayMatch && m.HomeGoals < m.AwayGoals))).CountAsync();

            ViewData["MatchesDrew"] = await _context.Match.Where(m => m.TeamId == match.TeamId
                                                                   && m.Date < DateTime.Now
                                                                   && m.HomeGoals == m.AwayGoals).CountAsync();

            return View(match);
        }

        // GET: Matches/Create
        [Authorize(Roles = "Admin,Manager")]
        public IActionResult Create() {
            ViewData["TeamId"] = new SelectList(_context.Team.Where(t => t.IsHome == false), "Id", "Name");
            ViewData["CompetitionId"] = new SelectList(_context.Competition, "Id", "Name");
            return View();
        }

        // POST: Matches/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin,Manager")]
        public async Task<IActionResult> Create([Bind("Id,TeamId,CompetitionId,isAwayMatch,Date,HomeGoals,AwayGoals")] Match match) {

            if (ModelState.IsValid && await IsMatchValid(match)) {

                _context.Add(match);
                await _context.SaveChangesAsync();

                // If the created match is an upcoming match, post an announcement tweet about it.
                if (match.Date > DateTime.Now) {

                    // Read the newly saved match data, including the data of the corresponding team.
                    Match savedMatch = await _context.Match.Include(m => m.Team).FirstOrDefaultAsync(m => m.Id == match.Id);

                    if (savedMatch != null) {

                        TwitterAPIService.SendTweet($"Real Madrid will match against {savedMatch.Team.Name} on {savedMatch.Date.ToShortDateString()}. Order your tickets now!");
                    }
                }

                return RedirectToAction(nameof(Index));
            }

            ViewData["TeamId"] = new SelectList(_context.Team.Where(t => !t.IsHome), "Id", "Name", match.TeamId);
            ViewData["CompetitionId"] = new SelectList(_context.Competition, "Id", "Name", match.CompetitionId);
            return View(match);
        }

        // GET: Matches/Edit/5
        [Authorize(Roles = "Admin,Manager")]
        public async Task<IActionResult> Edit(int? id) {

            if (id == null) {
                return NotFound();
            }
            var match = await _context.Match.FindAsync(id);
            if (match == null) {
                return NotFound();
            }

            ViewData["TeamId"] = new SelectList(_context.Team.Where(t => !t.IsHome), "Id", "Name", match.TeamId);
            ViewData["CompetitionId"] = new SelectList(_context.Competition, "Id", "Name", match.CompetitionId);
            return View(match);
        }

        // POST: Matches/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin,Manager")]
        public async Task<IActionResult> Edit(int id, [Bind("Id,TeamId,CompetitionId,isAwayMatch,Date,HomeGoals,AwayGoals")] Match match) {

            if (id != match.Id) {
                return NotFound();
            }

            if (ModelState.IsValid && await IsMatchValid(match)) {

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

            ViewData["TeamId"] = new SelectList(_context.Team.Where(t => !t.IsHome), "Id", "Name", match.TeamId);
            ViewData["CompetitionId"] = new SelectList(_context.Competition, "Id", "Name", match.CompetitionId);
            return View(match);
        }

        // Validates the given match and updates the ViewData["Error"] in accordance.
        private async Task<bool> IsMatchValid(Match match) {

            Team givenTeam = await _context.Team.FirstOrDefaultAsync(t => t.Id == match.TeamId);

            // Validate that a score was not supplied if a game has not occured yet, and vice versa.
            if (match.Date > DateTime.Now && (match.HomeGoals != null || match.AwayGoals != null)) {

                ViewData["Error"] = "Cannot specify a score for a match that has not occured yet.";
                return false;

            } else if (match.Date <= DateTime.Now && (match.HomeGoals == null || match.AwayGoals == null)) {

                ViewData["Error"] = "Please specify the score of the match.";
                return false;

            // Validate that the given team ID does not represent our home team.
            } else if (givenTeam.IsHome) {

                ViewData["Error"] = "A match against our home team cannot be created.";
                return false;
            }

            return true;
        }

        // GET: Matches/Delete/5
        [Authorize(Roles = "Admin,Manager")]
        public async Task<IActionResult> Delete(int? id) {

            if (id == null) {
                return NotFound();
            }

            var match = await _context.Match.Include(m => m.Team).Include(m => m.Competition).FirstOrDefaultAsync(m => m.Id == id);

            if (match == null) {
                return NotFound();
            }

            return View(match);
        }

        // POST: Matches/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin,Manager")]
        public async Task<IActionResult> DeleteConfirmed(int id) {

            var match = await _context.Match.FindAsync(id);
            _context.Match.Remove(match);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        // POST: Matches/PurchaseTicket/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public async Task<IActionResult> PurchaseTicket(int id) {

            var match = await _context.Match.Include(m => m.Users).Include(m => m.Team).ThenInclude(t => t.Stadium).Where(m => m.Id == id).FirstAsync();

            if (match != null) {

                var loggedInUser = await _context.User.Where(u => u.Username == HttpContext.User.Identity.Name).FirstOrDefaultAsync();

                // First, validate that the user have not already purchased a ticket, and the match has not occured yet.
                if (match.Users.Contains(loggedInUser)) {

                    TempData["Error"] = "You have already purchased a ticket for this match.";

                } else if (match.Date < DateTime.Now) {

                    TempData["Error"] = "Match has already occured.";

                } else {

                // Validate that there are tickets available - in accordance to the amount of tickets purchased so far,
                // and the amount of seats in the intended stadium.
                Team homeTeam;

                if (match.isAwayMatch) {
                    homeTeam = match.Team;
                } else {
                    homeTeam = await _context.Team.Include(t => t.Stadium).Where(t => t.IsHome).FirstOrDefaultAsync();
                }

                int availableSeats = homeTeam.Stadium.Capacity - match.Users.Count();

                // If there is a ticket available, add the logged in user to the match.
                if (availableSeats > 0) {
                    match.Users.Add(loggedInUser);
                    await _context.SaveChangesAsync();
                    TempData["Success"] = "Ticket purchased successfully";
                } else {
                    TempData["Error"] = "No tickets available.";
                }
            }
            }

            return RedirectToAction(nameof(Details), new { Id = id });
        }

        private bool MatchExists(int id) {
            return _context.Match.Any(e => e.Id == id);
        }
    }
}
