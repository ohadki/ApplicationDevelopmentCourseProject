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
            //Clear the cart after purchase.
            List<CartItem> cart = new List<CartItem>();
            HttpContext.Session.SetString(GetUniqueSessionKey("CartItems"), JsonConvert.SerializeObject(cart));
            HttpContext.Session.SetInt32(GetUniqueSessionKey("NumOfCartItems"), 0);
            HttpContext.Session.SetInt32(GetUniqueSessionKey("CartTotal"), 0);

            return View(productsList.ToList());
        }

        public IActionResult UserOrders()
        {
            List<Order> userOrders = new List<Order>();
            var userId = _context.User.Where(user => user.Username == User.Identity.Name.ToString()).FirstOrDefault().Id;
            userOrders = _context.Order.Where(order => order.UserId == userId).ToList();
            HttpContext.Session.SetInt32(GetUniqueSessionKey("NumOfOrders"), userOrders.Count);

            List<List<CartItem>> productsList = new List<List<CartItem>>();

            foreach(Order order in userOrders)
            {
                List<CartItem> orderItems = new List<CartItem>();
                orderItems = ConvertStringToProductList(order.ProductsString);
                productsList.Add(orderItems);
            }
            ViewBag.ProductsList = productsList;

            return View(userOrders.ToList());
        }

        public IActionResult OrderDetails(int orderId)
        {
            var userId = HttpContext.Session.GetString("UserId");
            List<CartItem> productsList = new List<CartItem>();
            var order = _context.Order.Where(x => x.Id == orderId).SingleOrDefault();

            if(order == null)
            {
                return View("UserOrders");
            }

            productsList=(ConvertStringToProductList(order.ProductsString));
            ViewBag.OrderId = orderId;
            ViewBag.OrderTotal = order.OrderTotal;
            ViewBag.OrderDate = order.OrderPlaced;
            return View(productsList);
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

        public string ConvertProductListToString(List<CartItem> productsList)
        {
            string list = "";

            foreach (var product in productsList)
            {
                list = list + product.Id.ToString() + "," + product.Quantity.ToString() +
                    "," + product.Product.Id.ToString() + "," + product.Product.Name +
                    "," + product.Product.CategoryId + ","+ product.Product.Description + 
                    "," + product.Product.Image + "," + product.Product.Price.ToString();
                list = list + "|";
            }

            return list;
        }

        public List<CartItem> ConvertStringToProductList(string productString)
        {
            List<CartItem> list = new List<CartItem>();
            String[] strlist = productString.Split("|");

            foreach (var product in strlist)
            {
                if (product != "")
                {
                    String[] productDetails = product.Split(",");
                    CartItem current = new CartItem();
                    Product currentProduct = new Product();
                    current.Id = Convert.ToInt32(productDetails[0]);
                    current.Quantity = Convert.ToInt32(productDetails[1]);
                    currentProduct.Id = Convert.ToInt32(productDetails[2]);
                    currentProduct.Name = productDetails[3];
                    currentProduct.CategoryId = Convert.ToInt32(productDetails[4]);
                    currentProduct.Description = productDetails[5];
                    currentProduct.Image = productDetails[6];
                    currentProduct.Price = Convert.ToDecimal(productDetails[7]);
                    current.Product = currentProduct;

                    list.Add(current);
                }
            }

            return list;
        }

        public async Task<IActionResult> PurchaseOrder()
        {
            try
            {
                List<CartItem> productsList = JsonConvert.DeserializeObject<List<CartItem>>(HttpContext.Session.GetString(GetUniqueSessionKey("CartItems")));
                Order order = new Order();
                order.UserId = _context.User.Where(user => user.Username == User.Identity.Name.ToString()).FirstOrDefault().Id;
                string productsString = ConvertProductListToString(productsList);
                order.ProductsString = productsString;

                decimal orderTotal = 0;
                foreach (var product in productsList)
                {
                    orderTotal += ((product.Quantity) * product.Product.Price);
                    var productCategoryCtx = _context.Category.SingleOrDefault(x => x.Id == product.Product.CategoryId);
                    productCategoryCtx.SoldProductsCount += product.Quantity;

                    //Update product quantity
                    var curProduct = _context.Product.SingleOrDefault(x => x.Id == product.Product.Id);
                    if(curProduct.Quantity - product.Quantity < 0)
                    {
                        throw new InvalidOperationException("Quantity cannot be negative");
                    }
                    curProduct.Quantity -= product.Quantity;
                }
                order.OrderTotal = orderTotal;
                order.OrderPlaced = DateTime.Now;

                _context.Add(order);
                await _context.SaveChangesAsync();

                return RedirectToAction("Index", "Orders");
            }
            catch (Exception e)
            {
                return RedirectToAction("Index", "Home");
            }
        }

        public IActionResult MonthlySalesStats()
        {
            var data = new JsonResult(
                _context.Order.AsEnumerable().GroupBy(o => new { o.OrderPlaced.Month, o.OrderPlaced.Year })
                .Select(
                    g => new
                    {
                        sum = g.Sum(s => s.OrderTotal),
                        month = g.Key.Month + "/" + g.Key.Year + "- " + g.Sum(s => s.OrderTotal).ToString() + "₪",
                    })
                .ToList());
            return data;
        }
    }
}
