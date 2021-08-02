using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using RealMadridWebApp.Data;
using RealMadridWebApp.Models;

namespace RealMadridWebApp.Controllers
{
    public class UsersController : Controller
    {
        private readonly RealMadridWebAppContext _context;

        public UsersController(RealMadridWebAppContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);

            return RedirectToAction("Login");
        }

        // GET: Users/Login
        public IActionResult Login(string returnUrl)
        {
            ViewData["ReturnUrl"] = returnUrl;
            return View();
        }

        // GET: Users/AccessDenied
        public IActionResult AccessDenied()
        {
            return View();
        }

        // POST: Users/Login
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [AllowAnonymous]
        public async Task<IActionResult> Login([Bind("Id,Username,Password")] User user, string returnUrl)
        {
            
            ModelState.Remove("FirstName");
            ModelState.Remove("PhoneNumber");
            ModelState.Remove("LastName");
            ModelState.Remove("EmailAddress");
            ModelState.Remove("CreationDate");

            if (ModelState.IsValid)
            {
                var q = await _context.User.FirstOrDefaultAsync(u => u.Username.Equals(user.Username) && u.Password.Equals(user.Password));

                if (q != null) 
                {
                    Signin(q);
                    return Redirect(returnUrl == null ? "/" : returnUrl);
                }
                else
                {
                    ViewData["Error"] = "Username and/or password are incorrect.";
                }
            }
            return View(user);
        }

        private async void Signin(User account)
        {
            var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.Name, account.Username),
                    new Claim(ClaimTypes.Role, account.Type.ToString()),
                };

            var claimsIdentity = new ClaimsIdentity(
                claims, CookieAuthenticationDefaults.AuthenticationScheme);

            var authProperties = new AuthenticationProperties
            {
                ExpiresUtc = DateTimeOffset.UtcNow.AddMinutes(10)
            };

            await HttpContext.SignInAsync(
                CookieAuthenticationDefaults.AuthenticationScheme,
                new ClaimsPrincipal(claimsIdentity),
                authProperties);
        }

        // GET: Users/Register
        public IActionResult Register()
        {
            return View();
        }

        // POST: Users/Register
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register([Bind("Id,Username,FirstName,LastName,BirthDate,PhoneNumber,EmailAddress,Password")] User user)
        {
            if (ModelState.IsValid)
            {
                var q = _context.User.FirstOrDefault(u => u.Username == user.Username);
                if (q == null)
                {
                    user.CreationDate = DateTime.Now.Date;
                    _context.Add(user);
                    await _context.SaveChangesAsync();
                    var u = _context.User.FirstOrDefault(u => u.Username == user.Username && u.Password == user.Password);
                    Signin(u);
                    return RedirectToAction(nameof(Index), "Home");
                }
                else
                {
                    ViewData["Error"] = "User already exist! - Choose another username";
                }

            }
            return View(user);
        }

        // GET: Users
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Index()
        {
            return View(await _context.User.ToListAsync());
        }

        // GET: Users
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> FilterUsers(UserType role, string stringRole, string userName, DateTime fromDate, DateTime toDate)
        {
            fromDate = (fromDate == default(DateTime) ? DateTime.MinValue : fromDate);
            toDate = (toDate == default(DateTime) ? DateTime.MaxValue : toDate);

            var users = await _context.User.Where(u => u.CreationDate >= fromDate && u.CreationDate <= toDate).ToListAsync();

            if(stringRole != "No Filter")
            {
                users = users.Where(u => u.Type == role).ToList();
            }
            if (userName != null)
            {
                users = users.Where(u => u.Username.Contains(userName)).ToList();
            }

            return Json(users);
        }

        [Authorize]
        public async Task<IActionResult> GetRolesValue()
        {
            string[] roles = new string[] { UserType.Client.ToString(), UserType.Manager.ToString(), UserType.Admin.ToString() } ;
            return Json(roles);
        }


        // GET: Users/Details/5
        [Authorize]
        public async Task<IActionResult> Details(int? id)
        {
            User user = null;

            if (id != null) // From URL
            {
                var currentUserName = HttpContext.User.Identity.Name;

                var userDB = _context.User.FirstOrDefault(u => u.Username == currentUserName);

                if (userDB.Id != id && !HttpContext.User.IsInRole(UserType.Admin.ToString()))
                {
                    return RedirectToAction(nameof(Index), nameof(Unauthorized));
                }
                 user = await _context.User.Include(u => u.Matches).FirstOrDefaultAsync(m => m.Id == id);
            }

            // From Layout
            else {
                user = await _context.User.Include(u => u.Matches)
                                            .ThenInclude(m => m.Team)
                                          .Include(u => u.Matches)
                                            .ThenInclude(m => m.Competition).FirstOrDefaultAsync(m => m.Username == HttpContext.User.Identity.Name);
            }

            if (user == null) {
                return NotFound();
            }

            return View(user);
        }

        // GET: Users/Edit/5
        [Authorize]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            ViewData["ReadOnly"] = "false";

            var currentUserName = HttpContext.User.Identity.Name;

            var userDB = _context.User.FirstOrDefault(u => u.Username == currentUserName);

            if(userDB.Id != id)
            {
                if (HttpContext.User.IsInRole(UserType.Admin.ToString()))
                {
                    ViewData["ReadOnly"] = "true";
                }
                else
                {
                    return RedirectToAction(nameof(Index), nameof(Unauthorized));
                }
            }
              
            var user = await _context.User.FindAsync(id);
            if (user == null)
            {
                return NotFound();
            }
            return View(user);
        }

        // POST: Users/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Username,FirstName,LastName,BirthDate,PhoneNumber,EmailAddress,Password,Type")] User user)
        {
            ViewData["EditError"] = null;

            if (ModelState.IsValid)
            {
                var EditedUser = await _context.User.AsNoTracking().Where(u => u.Id == id).FirstAsync();

                if(user.Equals(EditedUser))
                {
                    ViewData["EditError"] = "No changes were made";
                }
                else
                {
                    try
                    {
                        _context.Update(user);
                        await _context.SaveChangesAsync();
                    }
                    catch (DbUpdateConcurrencyException)
                    {
                        if (!UserExists(user.Id))
                        {
                            return NotFound();
                        }
                        else
                        {
                            throw;
                        }
                    }
                    string targetController = HttpContext.User.IsInRole(UserType.Admin.ToString()) ? "Users" : "Home";
                    return RedirectToAction(nameof(Index), targetController);
                }
            }
            ViewData["ReadOnly"] = (HttpContext.User.IsInRole(UserType.Admin.ToString()) && user.Username != HttpContext.User.Identity.Name) ? "true" : "false";
            return View(user);
        }

        // GET: Users/Delete/5
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var user = await _context.User
                .FirstOrDefaultAsync(m => m.Id == id);
            if (user == null)
            {
                return NotFound();
            }

            return View(user);
        }

        // POST: Users/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var user = await _context.User.FindAsync(id);
            _context.User.Remove(user);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool UserExists(int id)
        {
            return _context.User.Any(e => e.Id == id);
        }
        public IActionResult NotFound()
        {
            return RedirectToAction(nameof(Index), nameof(NotFound));
        }
    }
}
