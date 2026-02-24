using Common.Interfaces;
using Common.Models;
using Microsoft.Extensions.Caching.Memory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Repositories
{
    public class OrdersCacheRepository : IOrdersRepository
    {
        private IMemoryCache _db;

        public OrdersCacheRepository(IMemoryCache db)
        {
            _db = db;
        }

        public void Checkout(List<OrderItem> orderItems, string username)
        {
            _db.Set<List<OrderItem>>(username, orderItems);

        }

        public List<OrderItem> Get(string username)
        {
            return _db.Get<List<OrderItem>>(username);
        }
    }
}
