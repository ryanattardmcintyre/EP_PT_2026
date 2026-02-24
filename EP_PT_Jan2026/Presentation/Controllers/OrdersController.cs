using Common.Interfaces;
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
        [FromKeyedServices("db")] OrdersRepository ordersDbRepository,
        [FromKeyedServices("cache")] OrdersCacheRepository ordersCacheRepository,
        [FromKeyedServices("vat")] IPriceCalculation vatCalculation,
        [FromKeyedServices("blackFriday")] IPriceCalculation blackFridayCalculation
            )
        {
            _ordersCacheRepository = ordersCacheRepository;
            _ordersDbRepository = ordersDbRepository;

            _vatCalculation = vatCalculation;
            _blackFridayCalculation = blackFridayCalculation;
        }

        public IActionResult Index()
        {
            string username = "anonymous";

            //read the products already in cart/cache

            var list  = _ordersCacheRepository.Get(username);

            //work out the latest prices with discounts

            List<ShoppingCartViewModel> myModel = new List<ShoppingCartViewModel>();

            foreach(var oi in list)
            {
                myModel.Add(new ShoppingCartViewModel()
                {
                    OrderItem = oi,
                    VAT = (_vatCalculation.Calculate(oi.ProductFK)) * oi.Quantity,
                    Discount = _blackFridayCalculation.Calculate(oi.ProductFK) * oi.Quantity
                });
            }

            //and preview on screen
            return View(list);
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
                oi.Price += (_vatCalculation.Calculate(oi.ProductFK)) * oi.Quantity;
                oi.Price -= _blackFridayCalculation.Calculate(oi.ProductFK) * oi.Quantity;
            }

            _ordersDbRepository.Checkout(list, username);

            return RedirectToAction("Index", "Products");

        }
    }
}
