using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Models
{
    public class OrderItem
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [ForeignKey("Product")]
        public int ProductFK { get; set; }
        public virtual Product Product { get; set; } //Navigational Property, virtual is needed for LazyLoading to work without issues

        public int Quantity { get; set; }

        public double Price { get; set; }

        [ForeignKey("Order")]
        public Guid OrderFK { get; set; }
        public virtual Order Order {get;set;}
    }
}
