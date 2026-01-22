using Common.Models;

namespace Presentation.Models
{
    //ViewModels are used to present on screen a selection of database data
    //           are also a way how to transport data from the Controller to the View or vice-versa

    //Data 1 => Product
    //Data 2 => List<Category>
    //Data 3 => List<Status> //we don't have this
    public class ProductsCreateViewModel
    {
        public Product Product { get; set; }
        public List<Category> Categories { get; set; }
    }
}
