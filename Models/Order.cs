using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ApplicationDevelopmentCourseProject.Models
{
    public class Order
    {
        [Key]
        public int Id { get; set; }

        public decimal OrderTotal { get; set; }

        public DateTime OrderPlaced { get; set; }

        public IEnumerable<CartItem> Products { get; set; }

        public string UserId { get; set; }
    }
}
