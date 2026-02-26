using Common.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Utilities
{
    public class BlackFridayCalculation : IPriceCalculation
    {
        public BlackFridayCalculation(IProductsRepository repo)
        {
            _productsRepository = repo;
        }

        public IProductsRepository _productsRepository
        {
            get;
            set;
        }

        public double DiscountPercentage { get; set; } = 0.5;

        public double Calculate(int productId)
        {
            var myProduct = _productsRepository.Get(productId);
            return myProduct.Price * DiscountPercentage; //returns the discount
        }
    }
}
