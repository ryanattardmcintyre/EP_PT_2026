using Common.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Utilities
{
    public class MobileNotification : INotification
    {
        public void Notify(string message)
        {
             Console.WriteLine(message); //alternatively send sms or send email
        }
    }
}
