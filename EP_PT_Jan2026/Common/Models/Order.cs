using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Models
{

    // Order  
    // - 8C1AFCF9-18BE-4543-A850-575645B87BDD 1 1000
    // - 8C1AFCF9-18BE-4543-A850-575645B87BDD 2 300


    public class Order
    {
        [Key]
        public Guid Id { get; set; }
        public string Username { get; set; }
        public DateTime DataPlaced { get; set; }
        public double FinalPrice { get; set; }
        public IQueryable<OrderItem> OrderItems { get; set; }
    }
}
