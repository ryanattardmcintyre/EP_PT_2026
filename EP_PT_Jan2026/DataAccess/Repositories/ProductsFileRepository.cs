using Common.Interfaces;
using Common.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Repositories
{
    public class ProductsFileRepository : IProductsRepository
    {

        private string _path;
        public ProductsFileRepository(string path) {
            _path = path;
        }
        public void Add(Product product)
        {
            //JSON format
            var myExistentListOfProducts = Get().ToList();

            //if you want to save the actual image as a string
            //read the image from product.ImagePath in memory
            //Convert file residing in memory into base64
            //replace the content of ImagePath with the base64 string


            myExistentListOfProducts.Add(product);

            string myProducts = JsonConvert.SerializeObject(myExistentListOfProducts);

            System.IO.File.WriteAllText(_path, myProducts);
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
            if (System.IO.File.Exists(_path))
            { 
                    string jsonString = System.IO.File.ReadAllText(_path);
                    if (String.IsNullOrEmpty(jsonString)) return new List<Product>().AsQueryable();
                    List<Product> myProducts = JsonConvert.DeserializeObject<List<Product>>(jsonString);
                    return myProducts.AsQueryable();
            }
            else
            {
                return new List<Product>().AsQueryable();
            }
        }

        public void Update(Product updatedProduct)
        {
            throw new NotImplementedException();
        }
    }
}
