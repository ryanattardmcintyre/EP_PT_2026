using Common.Models;
using DataAccess.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Repositories
{
    public class CategoriesRepository
    {
        private ShoppingCartDbContext _context;
        public CategoriesRepository(ShoppingCartDbContext context) {
            _context = context;
        }

        /// <summary>
        /// Reads the entire list of Categories from the database table Categories
        /// </summary>
        /// <returns></returns>
        public IQueryable<Category> GetCategories()
        {
            return _context.Categories.OrderBy(x => x.Order);  
        }

      
    }
}
