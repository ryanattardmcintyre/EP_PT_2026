using Common.Interfaces;
using DataAccess.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Factory
{
    public class NotificationFactory
    {
        public INotification Create(int categoryId)
        {
            if (categoryId == 1)
                return new MobileNotification();
            else return new LaptopNotification();
        }
    }
}
