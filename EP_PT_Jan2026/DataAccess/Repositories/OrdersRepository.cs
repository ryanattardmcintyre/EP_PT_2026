using Common.Interfaces;
using Common.Models;
using DataAccess.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Repositories
{
    public  class OrdersRepository: IOrdersRepository
    {
        private ShoppingCartDbContext _shoppingCartDbContext;
        private IProductsRepository _productsRepository;
        public OrdersRepository(ShoppingCartDbContext context, IProductsRepository pr) {

            _shoppingCartDbContext = context;
            _productsRepository = pr;
        }

        public Order GetOrder(Guid orderId)
        {
            return _shoppingCartDbContext.Orders.SingleOrDefault(x => x.Id == orderId);
        }

        private void AddOrderItem(OrderItem oi)
        {
            _shoppingCartDbContext.OrderItems.Add(oi);
        }

        private void AddOrder(Order order)
        {
            _shoppingCartDbContext.Orders.Add(order);
        }

        public void Checkout(List<OrderItem> orderItems, string username)
        {
            _shoppingCartDbContext.Database.AutoTransactionBehavior 
                = Microsoft.EntityFrameworkCore.AutoTransactionBehavior.Always;

            Order myOrder = new Order();
            myOrder.Username = username;
            myOrder.DataPlaced = DateTime.Now;
            myOrder.FinalPrice = 0;
            myOrder.Id = Guid.NewGuid();

            foreach (OrderItem oi in orderItems)
            {
                var evaluatedProduct = _productsRepository.Get(oi.ProductFK);
                if (oi.Quantity > evaluatedProduct.Stock)
                {
                    throw new Exception("Not enough stock for product " + oi.ProductFK);
                }
                else
                {
                    evaluatedProduct.Stock -= oi.Quantity;
                }
                myOrder.FinalPrice += (oi.Price);
                oi.OrderFK = myOrder.Id;
                AddOrderItem(oi);
            }

            AddOrder(myOrder);
 
            _shoppingCartDbContext.SaveChanges();
        }

    }
}
