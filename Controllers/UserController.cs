using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ApplicationDevelopmentCourseProject.Models;

namespace ApplicationDevelopmentCourseProject.Controllers
{
    public class UserController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        public JsonResult CheckValidUser(User model)
        {
            string result = "Fail";
            var DataItem = "Test"; //TODO: LINQ
            if (DataItem != null)
            {
                //TODO: Handle valid user
            }
            return Json(result);
        }
    }
}
