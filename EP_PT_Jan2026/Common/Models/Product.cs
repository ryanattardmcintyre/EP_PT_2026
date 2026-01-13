using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Models
{
    //Notes:

    //Models will model the database to come
    //All contraints, properties,  data types, auto-incremental property - they have to be defined here


    //Recommendation:
    //Models should be named in singular, Pascal-case (e.g. first letter capital - Product, Category, Order, OrderItem)

    public class Product
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        
        [StringLength(200)]
        public string Name { get; set; }

        [ForeignKey("Category")] //we link the two properties together so at runtime, Category (Navigational Property)
                                 //is loaded with data linked with the value inside the CategoryFK
        public int CategoryFK { get; set; }
        public virtual Category Category { get; set; } //myProduct.Category.Id

        public double Price { get; set; }

        [StringLength(1000)]
        public string Description { get; set; }
        public int Stock { get; set; }
        public string ImagePath { get; set; }

    }
}
