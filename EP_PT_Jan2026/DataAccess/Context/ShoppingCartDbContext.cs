using Common.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Context
{
    //Note: Context class is an abstraction of the database - it is what links the models altogether
    //      It also allows you to implement certain configurations e.g.
    //      - changing the connectionString (even though its not recommended to be done here)
    //      - you can generate it here
    //      - enable lazy loading here
    //      - adding constraints here 

    //IdentityDbContext -> DbContext : but you get an upgrade - it creates the tables that allows us
    //                                 to manage user accounts
    public class ShoppingCartDbContext: IdentityDbContext
    {
        public ShoppingCartDbContext(DbContextOptions<ShoppingCartDbContext> options):  base(options)
        { }

        //tables:
        public DbSet<Product> Products { get; set; } //Products is the table name
        public DbSet<Category> Categories { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderItem> OrderItems { get; set; }
    }
}
