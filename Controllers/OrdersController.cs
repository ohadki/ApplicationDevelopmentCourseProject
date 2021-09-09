using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ApplicationDevelopmentCourseProject.Data;
using ApplicationDevelopmentCourseProject.Models;
using Newtonsoft.Json;
using Microsoft.AspNetCore.Http;
using System.Globalization;

namespace ApplicationDevelopmentCourseProject.Controllers
{
    public class OrdersController : Controller
    {
        private readonly ApplicationDevelopmentCourseProjectContext _context;

        public OrdersController(ApplicationDevelopmentCourseProjectContext context)
        {
            _context = context;
        }

        // GET: Orders
        public IActionResult Index()
        {
            List<CartItem> productsList = JsonConvert.DeserializeObject<List<CartItem>>(HttpContext.Session.GetString(GetUniqueSessionKey("CartItems")));
            if (productsList == null)
            {
                productsList = new List<CartItem>();
                HttpContext.Session.SetString("CartItems", JsonConvert.SerializeObject(productsList));
                HttpContext.Session.SetInt32("NumOfCartItems", 0);
                HttpContext.Session.SetInt32("CartTotal", 0);
            }

            return View(productsList.ToList());
        }

        // GET: Orders/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var order = await _context.Order
                .FirstOrDefaultAsync(m => m.Id == id);
            if (order == null)
            {
                return NotFound();
            }

            return View(order);
        }

        // GET: Orders/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Orders/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,OrderTotal,OrderPlaced,UserId")] Order order)
        {
            if (ModelState.IsValid)
            {
                _context.Add(order);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(order);
        }

        // GET: Orders/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var order = await _context.Order.FindAsync(id);
            if (order == null)
            {
                return NotFound();
            }
            return View(order);
        }

        // POST: Orders/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,OrderTotal,OrderPlaced,UserId")] Order order)
        {
            if (id != order.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(order);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!OrderExists(order.Id))
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
            return View(order);
        }

        // GET: Orders/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var order = await _context.Order
                .FirstOrDefaultAsync(m => m.Id == id);
            if (order == null)
            {
                return NotFound();
            }

            return View(order);
        }

        // POST: Orders/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var order = await _context.Order.FindAsync(id);
            _context.Order.Remove(order);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool OrderExists(int id)
        {
            return _context.Order.Any(e => e.Id == id);
        }
        private string GetUniqueSessionKey(string key)
        {
            return HttpContext.User.Identity.Name.ToString() + key;
        }

        public async Task<IActionResult> PurchaseOrder()
        {
            try
            {
                List<CartItem> productsList = JsonConvert.DeserializeObject<List<CartItem>>(HttpContext.Session.GetString(GetUniqueSessionKey("CartItems")));
                Order order = new Order();
                order.UserId = HttpContext.Session.GetString("UserId");
                //order.Products = productsList;
                decimal orderTotal = 0;
                foreach (var product in productsList)
                {
                    orderTotal += ((product.Quantity) * product.Product.Price);
                }
                order.OrderTotal = orderTotal;
                order.OrderPlaced = DateTime.Now;

                var updateMontlySales = _context.MonthlySales.SingleOrDefault(x => x.Month == CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(order.OrderPlaced.Month) && x.Year == order.OrderPlaced.Year.ToString());
                if(updateMontlySales == null)
                {
                    MonthlySales ms = new MonthlySales();
                    ms.Month = CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(order.OrderPlaced.Month);
                    ms.Year = order.OrderPlaced.Year.ToString();
                    ms.Sum = orderTotal;
                    _context.Add(ms);
                }
                else
                {
                    updateMontlySales.Sum += orderTotal;
                }
                _context.Add(order);
                await _context.SaveChangesAsync();

                return RedirectToAction("Index", "Orders");
            }
            catch (Exception)
            {
                return RedirectToAction("Index", "Home");
            }
        }

        public IActionResult MonthlySalesStats()
        {
            var data = new JsonResult(_context.MonthlySales.ToList());
            return data;
        }
    }
}
