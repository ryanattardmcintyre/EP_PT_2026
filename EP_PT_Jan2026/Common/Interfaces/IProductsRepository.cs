using Castle.Components.DictionaryAdapter.Xml;
using Common.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Interfaces
{
    public interface IProductsRepository
    {
        public void Add(Product product);
        public void Update(Product updatedProduct);
        public void Delete(Product product);
        public void Delete(int id);
        public Product? Get(int id);
        public IQueryable<Product> Get();
    }
}
