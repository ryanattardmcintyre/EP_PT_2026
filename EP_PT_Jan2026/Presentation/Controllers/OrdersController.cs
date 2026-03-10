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
        private IOrdersRepository _ordersDbRepository;

        private IPriceCalculation _vatCalculation;
        private IPriceCalculation _blackFridayCalculation;

        private ILogger<OrdersController> _logger;
        public OrdersController(
        [FromKeyedServices("logging")] IOrdersRepository ordersDbRepository,
        [FromKeyedServices("cache")] IOrdersRepository ordersCacheRepository,

        [FromKeyedServices("vat")] IPriceCalculation vatCalculation,
        [FromKeyedServices("blackFriday")] IPriceCalculation blackFridayCalculation,

        ILogger<OrdersController> logger
            )
        {
            _ordersCacheRepository = (OrdersCacheRepository) ordersCacheRepository;
            _ordersDbRepository =  ordersDbRepository;

            _vatCalculation = vatCalculation;
            _blackFridayCalculation = blackFridayCalculation;

            _logger = logger;
        }

        [Authorize] //it enforces that the user is logged in (hence non-anonymous) prior to accessing this method
        public IActionResult Index([FromServices] IProductsRepository productsRepository)
        {
            try
            {
                string username = User.Identity.Name; //this will give me the email address
                _logger.LogInformation($"{username} has accessed the shopping cart");
                //read the products already in cart/cache

                var list = _ordersCacheRepository.Get(username);
                _logger.LogInformation($"{username} retrieved {list.Count} items from the shopping cart");
                //work out the latest prices with discounts

                List<ShoppingCartViewModel> myModel = new List<ShoppingCartViewModel>();

                foreach (var oi in list)
                {
                    oi.Product = productsRepository.Get(oi.ProductFK);
                    myModel.Add(new ShoppingCartViewModel()
                    {
                        OrderItem = oi,
                        VAT = (_vatCalculation.Calculate(oi.ProductFK)) * oi.Quantity,
                        Discount = _blackFridayCalculation.Calculate(oi.ProductFK) * oi.Quantity
                    });
                    _logger.LogWarning($"VAT for {oi.ProductFK} with qty {oi.Quantity} was calcuated to be {(_vatCalculation.Calculate(oi.ProductFK)) * oi.Quantity} ");
                    _logger.LogWarning($"Discount for {oi.ProductFK} with qty {oi.Quantity} was calcuated to be {_blackFridayCalculation.Calculate(oi.ProductFK) * oi.Quantity} ");
                }
                return View(myModel);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message, null);
                TempData["error"] = "Could not load your cart. We've taken note of the error. Try again later";
                return RedirectToAction("Index", "Products");
            }
            
         
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
