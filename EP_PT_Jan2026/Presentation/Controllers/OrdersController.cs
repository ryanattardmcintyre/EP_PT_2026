using Common.Interfaces;
using Common.Models;
using DataAccess.Factory;
using DataAccess.Repositories;
using Microsoft.AspNetCore.Authorization;
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

        [Authorize] //it enforces that the user is logged in (hence non-anonymous) prior to accessing this method
        public IActionResult Index([FromServices] IProductsRepository productsRepository)
        { 
            
            string username = User.Identity.Name; //this will give me the email address

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

        [Authorize]
        public IActionResult Commit([FromServices] NotificationFactory factory, [FromServices] IProductsRepository productsRepository)
        {

            //read from cache
            //save in db
            string username = User.Identity.Name;

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

            foreach (var oi in list)
            {
                var product = productsRepository.Get(oi.ProductFK);
                INotification notification =  factory.Create(product.CategoryFK);
                notification.Notify($"Product {product.Name} has been bought");
            }

            _ordersCacheRepository.ClearCache(username);

            return RedirectToAction("Index", "Products");

        }


        public void BulkImport(string jsonString)
        {
            //deserialization of jsonString into a list of List<IItemValidating>
            //foreach (var item in listOfItemValidating)
            //{

            //}

        }

    }
}
