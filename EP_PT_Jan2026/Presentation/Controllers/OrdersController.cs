using Common.Interfaces;
using Common.Models;
using DataAccess.Repositories;
using Microsoft.AspNetCore.Mvc;
using Presentation.Models;

namespace Presentation.Controllers
{
    public class OrdersController : Controller
    {
        private OrdersCacheRepository _ordersCacheRepository;
        private OrdersRepository _ordersDbRepository;

        private IPriceCalculation _vatCalculation;
        private IPriceCalculation _blackFridayCalculation;

        public OrdersController(
        [FromKeyedServices("db")] IOrdersRepository ordersDbRepository,
        [FromKeyedServices("cache")] IOrdersRepository ordersCacheRepository,

        [FromKeyedServices("vat")] IPriceCalculation vatCalculation,
        [FromKeyedServices("blackFriday")] IPriceCalculation blackFridayCalculation
            )
        {
            _ordersCacheRepository = (OrdersCacheRepository) ordersCacheRepository;
            _ordersDbRepository = (OrdersRepository) ordersDbRepository;

            _vatCalculation = vatCalculation;
            _blackFridayCalculation = blackFridayCalculation;
        }

        public IActionResult Index([FromServices] IProductsRepository productsRepository)
        {
            string username = "anonymous";

            //read the products already in cart/cache

            var list  = _ordersCacheRepository.Get(username);

            //work out the latest prices with discounts

            List<ShoppingCartViewModel> myModel = new List<ShoppingCartViewModel>();

            foreach(var oi in list)
            {
                oi.Product = productsRepository.Get(oi.ProductFK);
                myModel.Add(new ShoppingCartViewModel()
                {
                    OrderItem = oi,
                    VAT = (_vatCalculation.Calculate(oi.ProductFK)) * oi.Quantity,
                    Discount = _blackFridayCalculation.Calculate(oi.ProductFK) * oi.Quantity
                });
            }

            //and preview on screen
            return View(myModel);
        }

        public IActionResult Commit()
        {
            //read from cache
            //save in db
            string username = "anonymous";

            //read the products already in cart/cache

            var list = _ordersCacheRepository.Get(username);

            //work out the latest prices with discounts

            foreach (var oi in list)
            {
                oi.Price -= _blackFridayCalculation.Calculate(oi.ProductFK) * oi.Quantity;
                oi.Price += (_vatCalculation.Calculate(oi.ProductFK)) * oi.Quantity;
            }

            _ordersDbRepository.Checkout(list, username);
            TempData["success"] = "Order was placed successfully";
            _ordersCacheRepository.Checkout(new List<OrderItem>(), username);

            return RedirectToAction("Index", "Products");

        }
    }
}
