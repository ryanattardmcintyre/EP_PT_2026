using Common.Models;
using DataAccess.Context;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Repositories
{
    public class ProductsRepository
    {

        //Dependency Injection
        //In dependency injection we request the use of an instance of an object which we have to assume its been created in the 
        //beginning (when the application started)
        //How can we request to use it? 
        // - (most popular) via Constructor Injection
        // - Method Injection
        // - Property Injection

        //ProductsRepository = the client 
        //ShoppingCartDbContext = the service
        public ProductsRepository(ShoppingCartDbContext myContext) {
            _myContext = myContext;
        }

        private ShoppingCartDbContext _myContext;

        public void Add(Product product) { 
            
            if(product.Stock >= 0)
            {
                //add the product in the database
                _myContext.Products.Add(product); //this adds the product object into a Products list into the context object in-memory
                
                //when you call the savechanges
                //it prepares and builds an Insert statement with the properties and their values you have in Products
                //it opens a connection with the database
                //executes the INSERT statement

                _myContext.SaveChanges(); //saves permanently the changes that i have in myContext into the actual database "file"
            }
        }

        public void Delete (Product product)
        {
            _myContext.Products.Remove(product);
            _myContext.SaveChanges();
        }

        public void Delete(int id)
        {
            var productToDelete = Get(id);
            if(productToDelete != null)
            {
                Delete(productToDelete);
            }
        }

        public Product? Get(int id) {
            return _myContext.Products.SingleOrDefault(x => x.Id == id); //lambda expression, shorthand notation replacing a foreach loop
        }

        public IQueryable<Product> Get()
        {
            return _myContext.Products; //Select * From Products
        }



    }
}
