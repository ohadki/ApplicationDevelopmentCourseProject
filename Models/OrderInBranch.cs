using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ApplicationDevelopmentCourseProject.Models
{
    public class OrderInBranch
    {
        public int orderId;
        public decimal orderTotal;
        public DateTime orderPlaced;
        public string name;// name of the user who ordered.
        public string address; // Address of the user that comeet the order.
    }
}
