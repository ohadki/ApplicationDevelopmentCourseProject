using ApplicationDevelopmentCourseProject.Data;
using ApplicationDevelopmentCourseProject.Models;
using Microsoft.AspNetCore.Authorization;
using ApplicationDevelopmentCourseProject.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
//using System.Data.Entity;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace ApplicationDevelopmentCourseProject.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly ApplicationDevelopmentCourseProjectContext _context;

        public class ProductsAndCategoriesViewModel
        {
            public List<Category> Categories { get; set; }
            public List<Product> Products { get; set; }
            public Category CategoryModel { get; set; }
            public Product ProductModel { get; set; }
        }

        public HomeController(ILogger<HomeController> logger, ApplicationDevelopmentCourseProjectContext context)
        {
            _logger = logger;
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var productsAndCategoriesViewModel = new ProductsAndCategoriesViewModel
            {
                Categories = await _context.Category.ToListAsync(),
                Products = await _context.Product.ToListAsync()
            };
            return View(productsAndCategoriesViewModel);
        }

        [Authorize]
        public IActionResult Privacy()
        {
            return View();
        }

        public IActionResult ContactUs()
        {
            var branches = _context.Branch.ToList();
            return View(branches);
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
