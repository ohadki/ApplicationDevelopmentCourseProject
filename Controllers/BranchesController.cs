using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ApplicationDevelopmentCourseProject.Data;
using ApplicationDevelopmentCourseProject.Models;
using Microsoft.AspNetCore.Http;

namespace ApplicationDevelopmentCourseProject.Controllers
{
    //TODO: HANDLE CRUD VIEW RETURNS
    public class BranchesController : Controller
    {
        private readonly ApplicationDevelopmentCourseProjectContext _context;
        public class ContactViewModel
        {
            public Branch BranchModel { get; set; }
            public List<Branch> Branches { get; set; }
            public List<Contact> Contacts{ get; set; }
            public Contact ContactModel { get; set; }
        }
        public BranchesController(ApplicationDevelopmentCourseProjectContext context)
        {
            _context = context;
        }

        // GET: Branches
        public async Task<IActionResult> Index()
        {
            var branches =  _context.Branch.ToListAsync();
            return View(await branches);
        }

        public IActionResult OrdersFromSpecificBranch(int branchId)
        {
            var tempOrders = (from u in _context.User
                              join ua in _context.UserAddress on u.Id equals ua.UserId
                              join o in _context.Order on u.Id equals o.UserId
                              where o.BranchId == branchId
                          select new
                              {
                                  UserId = u.Id,
                                  Name = u.FirstName + u.LastName,
                                  Address = ua.GetUserAddress(),
                                  Id = o.Id,
                                  Total = o.OrderTotal,
                                  Date = o.OrderPlaced,
                                  ProductString = o.ProductsString,
                                  branch = o.Branch,
                                  BranchID = o.BranchId,
                              }).ToList();
            List<Order> branchOrders = new List<Order>();
            branchOrders = _context.Order.Where(order => order.BranchId == branchId).ToList();
            HttpContext.Session.SetInt32(GetUniqueSessionKey("NumOfOrders"), branchOrders.Count);

            List<Order> orders = new List<Order>();
            foreach (var current in tempOrders)
            {
                Order currentOrder = new Order();
                currentOrder.Id = current.Id;
                currentOrder.ProductsString = current.ProductString;
                currentOrder.OrderPlaced = current.Date;
                currentOrder.OrderTotal = current.Total;
                currentOrder.UserId = current.UserId;
                currentOrder.Branch = current.branch;
                currentOrder.BranchId = current.BranchID;
                orders.Add(currentOrder);
            }

            return View(orders);
        }

        // GET: Branches/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var branch = await _context.Branch
                .FirstOrDefaultAsync(m => m.Id == id);
            if (branch == null)
            {
                return NotFound();
            }

            return View(branch);
        }

        // GET: Branches/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Branches/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,BranchName,Address,XCoordinate,YCoordinate")] Branch branch)
        {
            if (ModelState.IsValid)
            {
                _context.Add(branch);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(branch);
        }

        // GET: Branches/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var branch = await _context.Branch.FindAsync(id);
            if (branch == null)
            {
                return NotFound();
            }
            return View(branch);
        }
        private string GetUniqueSessionKey(string key)
        {
            return HttpContext.User.Identity.Name.ToString() + key;
        }

        // POST: Branches/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,BranchName,Address,XCoordinate,YCoordinate")] Branch branch)
        {
            if (id != branch.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(branch);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!BranchExists(branch.Id))
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
            return View(branch); 
        }

        // GET: Branches/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var branch = await _context.Branch
                .FirstOrDefaultAsync(m => m.Id == id);
            if (branch == null)
            {
                return NotFound();
            }

            return View(branch);
        }

        // POST: Branches/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var branch = await _context.Branch.FindAsync(id);
            _context.Branch.Remove(branch);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool BranchExists(int id)
        {
            return _context.Branch.Any(e => e.Id == id);
        }
    }
}
