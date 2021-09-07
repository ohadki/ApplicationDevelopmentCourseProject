using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ApplicationDevelopmentCourseProject.Models;
using Microsoft.AspNetCore.Http;
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
            List<CartItem> cart;
            if (HttpContext.Session.Get(GetUniqueSessionKey("CartItems")) == null)
            {
                cart = new List<CartItem>();
                HttpContext.Session.SetString(GetUniqueSessionKey("CartItems"), JsonConvert.SerializeObject(cart));
                HttpContext.Session.SetInt32(GetUniqueSessionKey("NumOfCartItems"), 0);
                HttpContext.Session.SetInt32(GetUniqueSessionKey("CartTotal"), 0);
            }
            else
            {
                cart = JsonConvert.DeserializeObject<List<CartItem>>(HttpContext.Session.GetString(GetUniqueSessionKey("CartItems")));
            }
            return View(cart.ToList());
        }

        public ActionResult Buy(int id, bool fromShoppingCartPage)
        {
            List<CartItem> cart;
            Product product = _context.Product.Where(p => p.Id == id).FirstOrDefault();
            
            if (HttpContext.Session.Get(GetUniqueSessionKey("CartItems")) == null)
            {
                cart = new List<CartItem>();
                HttpContext.Session.SetString(GetUniqueSessionKey("CartItems"), JsonConvert.SerializeObject(cart));
            }
            else
            {
                cart = JsonConvert.DeserializeObject<List<CartItem>>(HttpContext.Session.GetString(GetUniqueSessionKey("CartItems")));
            }

            int index = GetProductIndex(id);
            if (index != -1)
            {
                cart[index].Quantity++;
            }
            else
            {
                cart.Add(new CartItem { Product = product, Quantity = 1 });
            }

            HttpContext.Session.SetString(GetUniqueSessionKey("CartItems"), JsonConvert.SerializeObject(cart));
            UpdateNumOfCartItems(true);
            UpdateCartTotal(true, product.Price);

            if(fromShoppingCartPage)
            {
                return RedirectToAction("Index", "Cart");
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
            
        }

        public ActionResult Remove(int id)
        {
            List<CartItem> cart = JsonConvert.DeserializeObject<List<CartItem>>(HttpContext.Session.GetString(GetUniqueSessionKey("CartItems")));
            int index = GetProductIndex(id);
            if (index != -1)
            {
                UpdateNumOfCartItems(false);
                UpdateCartTotal(false, cart[index].Product.Price);

                if (cart[index].Quantity == 1)
                {
                    cart.RemoveAt(index);
                }
                else
                {
                    cart[index].Quantity--;
                }

                HttpContext.Session.SetString(GetUniqueSessionKey("CartItems"), JsonConvert.SerializeObject(cart));
            }
            return RedirectToAction("Index");
        }

        private void UpdateNumOfCartItems(bool isItemAdded)
        {
            if (HttpContext.Session.Get(GetUniqueSessionKey("NumOfCartItems")) == null)
            {
                HttpContext.Session.SetInt32(GetUniqueSessionKey("NumOfCartItems"), 1);
            }
            else
            {
                int NumOfCartItems = (int)HttpContext.Session.GetInt32(GetUniqueSessionKey("NumOfCartItems"));
                if(isItemAdded)
                {
                    NumOfCartItems++;
                }
                else
                {
                    NumOfCartItems--;
                }
                HttpContext.Session.SetInt32(GetUniqueSessionKey("NumOfCartItems"), NumOfCartItems);
            }
        }

        private void UpdateCartTotal(bool isItemAdded, decimal productPrice)
        {
            if (HttpContext.Session.Get(GetUniqueSessionKey("CartTotal")) == null)
            {
                HttpContext.Session.SetInt32(GetUniqueSessionKey("CartTotal"), (int)productPrice);
            }
            else
            {
                int CartTotal = (int)HttpContext.Session.GetInt32(GetUniqueSessionKey("CartTotal"));
                if (isItemAdded)
                {
                    CartTotal += (int)productPrice;
                }
                else
                {
                    CartTotal -= (int)productPrice;
                }
                HttpContext.Session.SetInt32(GetUniqueSessionKey("CartTotal"), CartTotal);
            }
        }

        private string GetUniqueSessionKey(string key)
        {
            return HttpContext.User.Identity.Name.ToString() + key;
        }

        private int GetProductIndex(int id)
        {
            List<CartItem> cart = JsonConvert.DeserializeObject<List<CartItem>>(HttpContext.Session.GetString(GetUniqueSessionKey("CartItems")));
            for (int i = 0; i < cart.Count; i++)
                if (cart[i].Product.Id.Equals(id))
                    return i;
            return -1;
        }
    }
}
