using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Factory
{
    public class ImportItemFactory
    {
        public List<IItemValidating> Create(string json)
        {
            List<IItemValidating> myList = new List<IItemValidating>();

            //deserialize the json string
            //if the type == MenuItem
            //add to myList new MenuItem(){...}
            //else
            //add to myList new Restaurant(){...}

            return myList;
               
        }
    }


    public interface IItemValidating
    {
        public string GetValidators();
        public string GetCardPartial();
    }

    public class Restaurant : IItemValidating
    {
        public string GetCardPartial()
        {
            return "RestaurantPartial.cshtml";
        }

        public string GetValidators()
        {
            return "admin@site.com";
        }


        public string Title { get; set; }
    }

    public class MenuItem : IItemValidating
    {

        public string RestaurantOwner { get; set; }
        public string GetCardPartial()
        {
            return "MenuItemPartial.cshtml";
        }

        public string GetValidators()
        {
            return RestaurantOwner;
        }
    }
}
