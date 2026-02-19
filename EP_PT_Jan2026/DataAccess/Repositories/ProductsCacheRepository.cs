using Common.Interfaces;
using Common.Models;
using Microsoft.Extensions.Caching.Memory;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Repositories
{
    //Framework services, e.g. IWebHostEnvironment, IMemoryCache
    //Application Services e.g. ProductsDbRepository, OrdersRepository, etc

    public class ProductsCacheRepository : IProductsRepository
    {
        private IMemoryCache _db;

        public ProductsCacheRepository(IMemoryCache db)
        {
            _db = db;
        }

        public void Add(Product product)
        {
            var myProducts = Get().ToList();
            myProducts.Add(product);

            string myProductsStr = JsonConvert.SerializeObject(myProducts);

            _db.Set("products", myProductsStr);

        }

        public void Delete(Product product)
        {
            throw new NotImplementedException();
        }

        public void Delete(int id)
        {
            throw new NotImplementedException();
        }

        public Product? Get(int id)
        {
            throw new NotImplementedException();
        }

        public IQueryable<Product> Get()
        {
            List<Product> products = new List<Product>();

            string productsStr = "";
            bool exists = _db.TryGetValue("products", out productsStr);

            if (!exists) return products.AsQueryable();

            List<Product> myProducts = JsonConvert.DeserializeObject<List<Product>>(productsStr);
            return myProducts.AsQueryable();
        }

        public void Update(Product updatedProduct)
        {
            throw new NotImplementedException();
        }
    }
}
