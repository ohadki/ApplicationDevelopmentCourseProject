using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ApplicationDevelopmentCourseProject.Models
{
    public class Branch
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Location { get; set; }
        public string ImageUrl { get; set; }
        //TODO: Think if we want to show the product for each branch.
        public IEnumerable<Product> Products { get; set; }
    }
}
