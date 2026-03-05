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


    //Validators:
    //Required, StringLength, Range, Compare , RegularExpression
    public class Product
    {

        //[Compare("ConfirmPassword", ErrorMessage ="" )]
        //public string Password { get; set; }
        //public string ConfirmPassword { get; set; }


        public Product()
        { Stock = 1; }

        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [StringLength(250, ErrorMessage ="Cannot exceed 250 characters")]
        [Required(AllowEmptyStrings =false, ErrorMessage ="Product name is empty")]
        [RegularExpression("^[a-zA-Z0-9 ]+$", ErrorMessage ="Product names must be made up of letters, numbers and spaces")]
        public string Name { get; set; }

        [ForeignKey("Category")] //we link the two properties together so at runtime, Category (Navigational Property)
                                 //is loaded with data linked with the value inside the CategoryFK
        public int CategoryFK { get; set; }
        public virtual Category Category { get; set; } //myProduct.Category.Id

        [Required(AllowEmptyStrings = false, ErrorMessage = "Product price is empty")]
        [Range(0, 100000, ErrorMessage ="Prices may vary between 0 and 100000. if more contact admin@gmail.com")]
        public double Price { get; set; }

        [StringLength(1000)]
        public string? Description { get; set; }

        [Range(0, 100000, ErrorMessage = "Stock amount may vary between 0 and 1000000. if more contact admin@gmail.com")]
        public int Stock { get; set; }

        public string? ImagePath { get; set; }

        public bool Discount { get; set; }

    }
}
