using Common.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Utilities
{
    public class VatCalculation : IPriceCalculation
    {
       
        public VatCalculation(IProductsRepository repo) {
            _productsRepository = repo;
        }

        public IProductsRepository _productsRepository { 
            get; 
            set; }

        public double Calculate(int productId)
        {
            var product = _productsRepository.Get(productId);
            return product.Price * .18;
        }

    }
}
