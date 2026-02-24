using Common.Models;

namespace Presentation.Models
{
    public class ShoppingCartViewModel
    {
        public OrderItem OrderItem { get; set; }
        public double VAT { get; set; }
        public double Discount { get; set; }
    }
}
