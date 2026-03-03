using Common.Models;
using DataAccess.Repositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Diagnostics;
using Presentation.Models;
using Common.Interfaces;

namespace Presentation.Controllers
{
    public class ProductsController : Controller
    {
        private CategoriesRepository _categoriesRepository;
        private IProductsRepository _productsRepository;
        private IOrdersRepository _ordersRepository;
        public ProductsController(CategoriesRepository categoriesRepository, 
            IProductsRepository productsRepository
            , [FromKeyedServices("cache")] IOrdersRepository ordersRepository) //requesting an instance of type PRODUCTSREPOSITORY
        {
             

            _categoriesRepository = categoriesRepository;
            _productsRepository = productsRepository;
            _ordersRepository = ordersRepository;
        }

        public IActionResult Index(int page = 1, int pageSize = 6)
        {
            //the term models is used for object types that transport data to/from views to/from the controller


            if (TempData["keyword"] != null || TempData["category"] != null)
            {
                //it means that next was pressed in search mode
                string keyword = TempData["keyword"] != null ? TempData["keyword"].ToString() : "";
                int category = TempData["category"] != null ? Convert.ToInt32(TempData["category"]) : -1;


                return Search(keyword,category, page, pageSize);
            }
            
            //info 2: list of products
            var list = _productsRepository.Get().Skip((page - 1)*pageSize).Take(pageSize);
            ViewBag.CurrentPage = page;
            ViewBag.TotalItemsFetched = list.Count();
            ViewBag.PageSize = pageSize;

            //info 1: list of categories
            var myPreparedSqlOfCategories = _categoriesRepository.GetCategories();
            ProductsListViewModel myModel = new ProductsListViewModel();
            myModel.Products = list.ToList();

            myModel.Categories = myPreparedSqlOfCategories.ToList();

            return View(myModel); //model: IQueryable<Product>
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
            var myPreparedSqlOfCategories = _categoriesRepository.GetCategories();

            ProductsCreateViewModel myModel = new ProductsCreateViewModel();
            myModel.Categories = myPreparedSqlOfCategories.ToList(); //opens connection - gets data - closes connection
            //myModel.Product = new Product();

            return View(myModel); //mvc will return a view from the folder VIEW which should be located under a subfolder called Products with the name Create()
        }


        //to explain:

        //what happens when there's an error ...redirection!
        //validations
        //revise registration of CategoriesRepository....

        [HttpPost]
        public IActionResult Submit(ProductsCreateViewModel p, [FromServices] IWebHostEnvironment host)
        {
            //Add the product keyed in by the user into the database
            //note: no LINQ code
            try
            {
                if (p.ImageFile != null)
                {
                    string uniqueFilename = Guid.NewGuid().ToString() + System.IO.Path.GetExtension(p.ImageFile.FileName);

                    //we save the physical file using the absolute path //C:\Users\Ryan\source\repos\EP_PT_2026\EP_PT_Jan2026\Presentation\wwwroot
                    string absolutePath = host.WebRootPath + "//images//" + uniqueFilename;

                    using (var fs = new FileStream(absolutePath, FileMode.CreateNew, FileAccess.Write))
                    {
                        p.ImageFile.CopyTo(fs);
                    }

                    //relative Path is used to render images in the browser
                    string relativePath = @"\images\" + uniqueFilename;
                    p.Product.ImagePath = relativePath;
                }

                _productsRepository.Add(p.Product);
                //...
                TempData["success"] = "Product was added successfully";
                
                //return View("Index"); //redirecting the user server-side
                return RedirectToAction("Index"); 
                //client-side redirection //https://localhost:xxxx/Products/Index ViewBag won't work; but TempData will
            }
            catch (Exception ex) 
            {
                //log ex.Message
                TempData["error"] = "Product failed to be added";

                return View("Create");
            }

        }


        //Note: Ways of passing data from View => Controller
        //1. Parameters
        //2. Models
        //3. Context.Forms[...]

        
        public IActionResult Search(string keyword, int category, int page = 1, int pageSize = 6)
        {
            //search in the database for products starting with keyword

            //Note: Ways of passing data from Controller => View
            //1. ViewBag => dynamic object, whatever i store inside doesn't survive a redirection
            //2. TempData => it survives 1 redirection
            //3. Models
            //4. Cookies
            //5. ViewData
            //6. Session

            //Notes on deferred execution (i.e. using the IQueryable)
            //Get() => 1st call
            //Where() => 2nd call
            //OrderBy() => 3rd call

            //because of IQueryable()
            //after 1st call => Select * From Products
            //after 2nd call => Select * From Products Where Name like '%keyword%' or Description Like '%description%'
            //after 3rd call => Select * From Products Where Name like '%keyword%' or Description Like '%description%' order by Name asc

            if (keyword == null) keyword = "";

            var list = _productsRepository.Get().Where(p => p.Name.Contains(keyword)
                                                 || p.Description.Contains(keyword)
                                                  );

            if (category > 0) list = list.Where(x => x.CategoryFK == category);
              
            list = list.OrderBy(p=> p.Name).Skip((page - 1) * pageSize).Take(pageSize);

            ViewBag.CurrentPage = page;
            ViewBag.TotalItemsFetched = list.Count();
            ViewBag.PageSize = pageSize;

            //YOU control where the user is redirected after the method completes

            //info 1: list of categories
            var myPreparedSqlOfCategories = _categoriesRepository.GetCategories();
            ProductsListViewModel myModel = new ProductsListViewModel();
            myModel.Products = list.ToList();

            myModel.Categories = myPreparedSqlOfCategories.ToList();

            
            TempData["keyword"] = keyword;
            TempData["category"] = category;

            return View("Index", myModel);
            //by default => View() it will seek a View called same as the action name i.e. Products\Search.cshtml
            //to redirect the user to a different-named view we use return View("nameOfTheOtherView");


        }

        public IActionResult Details(int id)
        {
            var product = _productsRepository.Get(id);
         
            if (product == null)
                {
                TempData["error"] = "Product does not exist";
                return RedirectToAction("Index"); //it will redirect the end user to the action (above) called Index
                }
            else return View(product);

        }


        [HttpGet]
        public IActionResult Edit(int? productId)
        {
            if(productId == null)
            {
                TempData["error"] = "Invalid request";
                return RedirectToAction("Index");
            }
            
            else
            {
                var originalProduct = _productsRepository.Get(productId.Value);
                if (originalProduct == null) {
                    TempData["error"] = "Product does not exist";
                    return RedirectToAction("Index");
                }
                else
                {
                    var myPreparedSqlOfCategories = _categoriesRepository.GetCategories();

                    ProductsCreateViewModel myModel = new ProductsCreateViewModel();
                    myModel.Categories = myPreparedSqlOfCategories.ToList();
                    myModel.Product = originalProduct;
                    
                    return View(myModel); //means => it is going to look for a page named Edit.cshtml
                }
            }

        }

        [HttpPost]
        public IActionResult Update(ProductsCreateViewModel p, [FromServices] IWebHostEnvironment host)
        {
            try
            {
                if (p.ImageFile != null)
                {
                    string uniqueFilename = Guid.NewGuid().ToString() + System.IO.Path.GetExtension(p.ImageFile.FileName);

                    string absolutePath = host.WebRootPath + "//images//" + uniqueFilename;

                    using (var fs = new FileStream(absolutePath, FileMode.CreateNew, FileAccess.Write))
                    {
                        p.ImageFile.CopyTo(fs);
                    }

                    string relativePath = @"\images\" + uniqueFilename;
                    p.Product.ImagePath = relativePath;


                    //delete the old image
                    var myOldProduct = _productsRepository.Get(p.Product.Id);
                    string absolutePathOfTheImageToDelete = host.WebRootPath + myOldProduct.ImagePath;
                    if(System.IO.File.Exists(absolutePathOfTheImageToDelete) || String.IsNullOrEmpty(myOldProduct.ImagePath) == false )
                        System.IO.File.Delete(absolutePathOfTheImageToDelete);
                }

                _productsRepository.Update(p.Product);

                TempData["success"] = "Product was updated successfully";

                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                TempData["error"] = "Product failed to be added. Check your details"; 
                return RedirectToAction("Edit", new { productId = p.Product.Id });
            }

        }


         //AddToCart OR Delete
        public IActionResult Checkout(List<OrderItem> productsToBuy, string buttonChoice)
        {
            string username = User.Identity.Name;

            if (buttonChoice.ToLower() == "delete")
            {
                foreach (var oi in productsToBuy)
                {
                    if (oi.ProductFK != 0)
                    {
                        _productsRepository.Delete(oi.ProductFK);
                    }
                }
                TempData["success"] = "Product(s) deleted successfully";

            }
            else    //AddToCart
            {
                try
                {
                    List<OrderItem> productsConfirmed = new List<OrderItem>();
                    foreach (var oi in productsToBuy)
                    {
                        if (oi.ProductFK != 0 && oi.Quantity > 0)
                        {
                            productsConfirmed.Add(oi);
                        }
                    }

                    _ordersRepository.Checkout(productsConfirmed, username); //<< added in cache!

                    TempData["success"] = "Order was placed successfully";
                }
                catch (Exception ex)
                {
                    if (ex.Message.Contains("Not enough"))
                    {
                        TempData["error"] = ex.Message;
                    }
                    else
                        TempData["error"] = "Order was not placed. Try again later";
                }
            }
            return RedirectToAction("Index");

        }


       

    }
}
