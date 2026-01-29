using Common.Models;

namespace Presentation.Models
{
    public class ProductsListViewModel
    {
        public List<Product> Products { get; set; }
        public List<Category> Categories { get; set; }
    }
}
