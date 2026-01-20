using Common.Models;
using DataAccess.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace Presentation.Controllers
{
    public class ProductsController : Controller
    {
        //private ProductsRepository _productsRepository;
        //public ProductsController(ProductsRepository pr) //requesting an instance of type PRODUCTSREPOSITORY
        //{
        //    _productsRepository = pr;
        //}

        public IActionResult Index()
        {
            return View();
        }

        //roles of
        //Controllers : business logic related to the ui
        //Repository : business logic related to the database

        //actions that can be coded in a controller and NOT in a repository
        //1. request > data 
        //2. parse the data
        //3. sanitize the data 
        //4. save the data
        //5. you might need to re-shape the data in such a way that the user understands it better
        //6. you have to decide where to redirect the user
        //7. response < data

        [HttpGet]
        public IActionResult Create() //this is triggered upon the user clicks the link Create
        {
            return View(); //mvc will return a view from the folder VIEW which should be located under a subfolder called Products with the name Create()
        }

        [HttpPost]
        public IActionResult Submit(Product p, [FromServices] ProductsRepository _productsRepository)
        {
            //Add the product keyed in by the user into the database
            //note: no LINQ code
            _productsRepository.Add(p);
            TempData["success"] = "Product was added successfully";


            return View("Index");

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
