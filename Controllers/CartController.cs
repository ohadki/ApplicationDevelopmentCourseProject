using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ApplicationDevelopmentCourseProject.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using ApplicationDevelopmentCourseProject.Data;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using Newtonsoft.Json;
using Microsoft.AspNetCore.Authorization;

namespace ApplicationDevelopmentCourseProject.Controllers
{
    [Authorize]
    public class CartController : Controller
    {
        private readonly ApplicationDevelopmentCourseProjectContext _context;
        public CartController(ApplicationDevelopmentCourseProjectContext context)
        {
            _context = context;
        }
        public IActionResult Index()
        {
            return View();
        }

        public ActionResult Buy(int id)
        {
            Product product = new Product();
            if (HttpContext.Session.Get("cart") == null)
            {
                List<CartItem> cart = new List<CartItem>();
                cart.Add(new CartItem { Product = _context.Product.Where(p => p.Id == id).FirstOrDefault(), Quantity = 1 });
                HttpContext.Session.SetString("cart", JsonConvert.SerializeObject(cart));
            }
            else
            {
                List<CartItem> cart = JsonConvert.DeserializeObject<List<CartItem>>(HttpContext.Session.GetString("cart"));
                int index = GetProductIndex(id);
                if (index != -1)
                {
                    cart[index].Quantity++;
                }
                else
                {
                    cart.Add(new CartItem { Product = (Product)_context.Product.Where(p => p.Id == id), Quantity = 1 });
                }
                HttpContext.Session.SetString("cart", JsonConvert.SerializeObject(cart));
            }
            return RedirectToAction("Index", "Home");
        }

        public ActionResult Remove(int id)
        {
            List<CartItem> cart = JsonConvert.DeserializeObject<List<CartItem>>(HttpContext.Session.GetString("cart"));
            int index = GetProductIndex(id);
            cart.RemoveAt(index);
            HttpContext.Session.SetString("cart", JsonConvert.SerializeObject(cart));
            return RedirectToAction("Index");
        }
        private int GetProductIndex(int id)
        {
            List<CartItem> cart = JsonConvert.DeserializeObject<List<CartItem>>(HttpContext.Session.GetString("cart"));
            for (int i = 0; i < cart.Count; i++)
                if (cart[i].Product.Id.Equals(id))
                    return i;
            return -1;
        }
    }
}
