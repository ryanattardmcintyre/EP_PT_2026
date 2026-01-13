using Microsoft.AspNetCore.Mvc;

namespace Presentation.Controllers
{
    public class ProductsController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        //Note: Ways of passing data from View => Controller
        //1. Parameters
        //2. Models
        //3. Context.Forms[...]

        public IActionResult Search(string keyword)
        {
            //search in the database for products starting with keyword


            //Note: Ways of passing data from Controller => View
            //1. ViewBag => dynamic object, whatever i store inside doesn't survive a redirection
            //2. TempData => it survives 1 redirection
            //3. Models
            //4. Cookies
            //5. ViewData
            //6. Session
            //....

            

            ViewBag.Message = "No products were found with this " + keyword;


            //YOU control where the user is redirected after the method completes
            return View("Index");
            //by default => View() it will seek a View called same as the action name i.e. Products\Search.cshtml
            //to redirect the user to a different-named view we use return View("nameOfTheOtherView");


        }


    }
}
