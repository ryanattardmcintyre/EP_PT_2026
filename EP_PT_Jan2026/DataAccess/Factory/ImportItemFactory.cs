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
    { }

    public class Restaurant:IItemValidating
    { }

    public class MenuItem:IItemValidating
    { }
}
