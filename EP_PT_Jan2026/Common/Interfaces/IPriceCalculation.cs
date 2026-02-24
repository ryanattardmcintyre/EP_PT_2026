using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Interfaces
{
    public interface IPriceCalculation
    {
        public IProductsRepository _productsRepository { get; set; }
        public double Calculate(int productId);

    }
}
