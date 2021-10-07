using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ApplicationDevelopmentCourseProject.Data;
using ApplicationDevelopmentCourseProject.Models;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;

namespace ApplicationDevelopmentCourseProject.Controllers
{
    public class UsersController : Controller
    {
        private readonly ApplicationDevelopmentCourseProjectContext _context;

        public class AdminViewModel
        {
            public List<Branch> Branches { get; set; }
            public List<User> Users { get; set; }
            public List<Product> Products { get; set; }
            public List<Category> Categories { get; set; }
            public List<ProductTag> ProductTags { get; set; }
            public List<Contact> Contacts { get; set; }
            public List<Order> Orders { get; set; }
            public Branch BranchModel { get; set; }
            public User UserModel { get; set; }
            public Product ProductModel { get; set; }
            public ProductTag ProductTagModel { get; set; }
            public Contact ContactModel { get; set; }
            public Category CategoryModel { get; set; }
            public Order OrderModel { get; set; }
        }

        public UsersController(ApplicationDevelopmentCourseProjectContext context)
        {
            _context = context;
        }

        // GET: Users
        public IActionResult Index()
        {
            return View(nameof(AdminPanel));
        }

        public IActionResult Login()
        {
            return View();
        }

        public IActionResult Account()
        {
            return View();
        }

        public IActionResult Register()
        {
            return View();
        }

        [Authorize(Roles ="Admin")]
        public async Task<IActionResult> AdminPanel()
        {
            var adminModel = new AdminViewModel
            {
                Users = await _context.User.ToListAsync(),
                Branches = await _context.Branch.ToListAsync(),
                Products = await _context.Product.ToListAsync(),
                Contacts = await _context.Contact.ToListAsync(),
                Categories = await _context.Category.ToListAsync(),
                Orders = await _context.Order.ToListAsync(),
                ProductTags = await _context.ProductTag.ToListAsync(),
            };
            return View(adminModel);
        }

        // GET: Users1/Details/5
        public async Task<IActionResult> Details(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var user = await _context.User
                .FirstOrDefaultAsync(m => m.Id == id);

            user.Address = _context.UserAddress.Where(a => a.UserId == user.Id).FirstOrDefault();
            if (user == null)
            {
                return NotFound();
            }

            return View(user);
        }
        
        // GET: Users1/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Users1/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Email,Username,Password,Id,FirstName,LastName,AddressLine1,AddressLine2,City,Country,ContactNumber,Type,MemberSince")] User user)
        {
            if (ModelState.IsValid)
            {
                _context.Add(user);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(user);
        }

        // GET: Users/Edit/5
        public async Task<IActionResult> Edit(string id)
        {

            User user = null;

            if (id == null)
            {
                user = await _context.User.Where(user => user.Username == User.Identity.Name.ToString()).FirstOrDefaultAsync();
            }
            else
            {
                user = await _context.User.FindAsync(id);
            }
            user.Address = _context.UserAddress.Where(a => a.UserId == user.Id).FirstOrDefault();

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
        public async Task<IActionResult> Edit(string id, User user)
        {
            if (id != user.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
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
                if (User.IsInRole("Admin"))
                {
                    return RedirectToAction(nameof(AdminPanel));
                }
                else
                {
                    return RedirectToAction(nameof(Account));
                }   
            }
            return View(user);
        }

        // GET: Users/Delete/5
        public async Task<IActionResult> Delete(string id)
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
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            var user = await _context.User.FindAsync(id);
            _context.User.Remove(user);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool UserExists(string id)
        {
            return _context.User.Any(e => e.Id == id);
        }

        public JsonResult CheckValidUser([Bind("Username,Password")] User user)
        {
            string result = "Fail";
            if(user != null)
            {
                User loggedUser = _context.User.Where(x => x.Username == user.Username && x.Password == user.Password).FirstOrDefault();
                if (loggedUser != null)
                {
                    loginUser(loggedUser.Username, loggedUser.Type);
                    result = "Login Succeded";
                }
            }
            return Json(result);
        }

        private async void loginUser(string username, UserType type)
        {
            var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.Name, username),
                    new Claim(ClaimTypes.Role, type.ToString()),
                };

            var claimsIdentity = new ClaimsIdentity(
                claims, CookieAuthenticationDefaults.AuthenticationScheme);

            var authProperties = new AuthenticationProperties
            {
                //ExpiresUtc = DateTimeOffset.UtcNow.AddMinutes(10)
            };

            await HttpContext.SignInAsync(
                CookieAuthenticationDefaults.AuthenticationScheme,
                new ClaimsPrincipal(claimsIdentity),
                authProperties);
        }

        //TODO: MODIFY THIS LOGOUT
        public async Task<IActionResult> Logout()
        {
            // HttpContext.Session.Clear();

            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);

            return RedirectToAction(nameof(Index),"Home");
        }

        public async Task<IActionResult> RegisterUser(User user)
        {
            if (ModelState.IsValid)
            {
                user.Address.UserId = user.Id;
                _context.User.Add(user);
                _context.UserAddress.Add(user.Address);
                await _context.SaveChangesAsync();
                loginUser(user.Username, user.Type);
                return RedirectToAction(nameof(Index),"Home");
            }
            return View(user);
        }
    }
}
