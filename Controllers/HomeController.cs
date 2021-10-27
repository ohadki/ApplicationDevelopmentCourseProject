using ApplicationDevelopmentCourseProject.Data;
using ApplicationDevelopmentCourseProject.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using static ApplicationDevelopmentCourseProject.Controllers.BranchesController;
using Newtonsoft.Json;
using System.Web;

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
            public List<ProductTag> ProductTags { get; set; }
            public Category CategoryModel { get; set; }
            public Product ProductModel { get; set; }
            public ProductTag ProductTagModel { get; set; }
        }

        public HomeController(ILogger<HomeController> logger, ApplicationDevelopmentCourseProjectContext context)
        {
            _logger = logger;
            _context = context;
        }

        public async Task<IActionResult> Index(int? categoryId)
        {
            var productsAndCategoriesViewModel = new ProductsAndCategoriesViewModel
            {
                Categories = await _context.Category.ToListAsync(),
                Products = (categoryId == null) ? await _context.Product.ToListAsync() : await _context.Product.Where(p => p.Category.Id == categoryId).ToListAsync(),
                ProductTags = await _context.ProductTag.ToListAsync(),
            };
            return View(productsAndCategoriesViewModel);
        }

        [Authorize]
        public IActionResult Privacy()
        {
            return View();
        }

        public IActionResult Contact()
        {
            var contactViewModel = new ContactViewModel
            {
                Contacts =  _context.Contact.ToList(),
                Branches =  _context.Branch.ToList()
            };
            return View(contactViewModel);
        }

        public ActionResult Weather()
        {

            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        [HttpPost]
        public String WeatherDetail(string City)
        {

            //Assign API KEY which received from OPENWEATHERMAP.ORG  
            string appId = "f308ba1bb94b36cb85521d1dfd900d93";

            //API path with CITY parameter and other parameters.  
            string url = string.Format("http://api.openweathermap.org/data/2.5/weather?q={0}&units=metric&cnt=1&APPID={1}", City, appId);

            using (WebClient client = new WebClient())
            {
                string json;
                try
                {
                    json = client.DownloadString(url);
                }
                catch (WebException e)
                {
                    Response.StatusCode = (int)HttpStatusCode.NotFound;
                    return "City Not Found";
                }
                

                //********************//  
                //     JSON RECIVED   
                //********************//  
                //{"coord":{ "lon":72.85,"lat":19.01},  
                //"weather":[{"id":711,"main":"Smoke","description":"smoke","icon":"50d"}],  
                //"base":"stations",  
                //"main":{"temp":31.75,"feels_like":31.51,"temp_min":31,"temp_max":32.22,"pressure":1014,"humidity":43},  
                //"visibility":2500,  
                //"wind":{"speed":4.1,"deg":140},  
                //"clouds":{"all":0},  
                //"dt":1578730750,  
                //"sys":{"type":1,"id":9052,"country":"IN","sunrise":1578707041,"sunset":1578746875},  
                //"timezone":19800,  
                //"id":1275339,  
                //"name":"Mumbai",  
                //"cod":200}  

                //Converting to OBJECT from JSON string.  
                RootObject weatherInfo = JsonConvert.DeserializeObject<RootObject>(json);

                //Special VIEWMODEL design to send only required fields not all fields which received from   
                //www.openweathermap.org api  
                ResultViewModel rslt = new ResultViewModel();

                rslt.Country = weatherInfo.sys.country;
                rslt.City = weatherInfo.name;
                rslt.Lat = Convert.ToString(weatherInfo.coord.lat);
                rslt.Lon = Convert.ToString(weatherInfo.coord.lon);
                rslt.Description = weatherInfo.weather[0].description;
                rslt.Humidity = Convert.ToString(weatherInfo.main.humidity);
                rslt.Temp = Convert.ToString(weatherInfo.main.temp);
                rslt.TempFeelsLike = Convert.ToString(weatherInfo.main.feels_like);
                rslt.TempMax = Convert.ToString(weatherInfo.main.temp_max);
                rslt.TempMin = Convert.ToString(weatherInfo.main.temp_min);
                rslt.WeatherIcon = weatherInfo.weather[0].icon;

                //Converting OBJECT to JSON String   
                var jsonstring = JsonConvert.SerializeObject(rslt, Formatting.Indented);

                //Return JSON string.  
                return jsonstring;
            }
        }

        public async Task<IActionResult> SearchJson(string searchString)
        {
            var products = new List<Product>();
            if (!string.IsNullOrEmpty(searchString))
            {
                products = await _context.Product.Where(p => p.Name.Contains(searchString)).OrderByDescending(p => p.Id).ToListAsync();
            }
            else
            {
                products = await _context.Product.ToListAsync();
            }
            return Json(products.ToList());
        }

        public IActionResult SearchByManyParametersJson(string categoriesStr, string productTagsStr, string minPrice, string maxPrice)
        {
            var products = _context.Product.Where(p => true);
            if (minPrice != null)
                products = products.Where(p => p.Price >= Int32.Parse(minPrice));
            if (maxPrice != null)
                products = products.Where(p => p.Price <= Int32.Parse(maxPrice));
            if (categoriesStr != null)
                products = products.Where(p => categoriesStr.Split(",", System.StringSplitOptions.None).Contains(p.Category.Id.ToString()));
            if (productTagsStr != null)
            {
                string[] productTagIdsArray = productTagsStr.Split(",");
                var productTagsList = _context.ProductTag.Where(pt => productTagIdsArray.Contains(pt.Id.ToString())).ToList();
                string[] productTagsArr = new string[productTagsList.Count];
                int index = 0;

                foreach (ProductTag pt in productTagsList)
                {
                    productTagsArr[index] = pt.TagName;
                    index++;
                }

                return Json(products.ToList().Where(p => p.ProductTagsArr().Intersect(productTagsArr).Any()));
            }
            return Json(products.ToList());
        }
    }
}
