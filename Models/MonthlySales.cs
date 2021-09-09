using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ApplicationDevelopmentCourseProject.Models
{
    public class MonthlySales
    {
        [Key]
        public int Id { get; set; }
        public string Month { get; set; } = DateTime.Now.ToString("MM");
        public string Year { get; set; } = DateTime.Now.Year.ToString("YYYY");
        public decimal Sum { get; set; } = 0;
    }
}
