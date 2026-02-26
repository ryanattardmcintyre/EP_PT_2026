using Common.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Utilities
{
    public class LaptopNotification : INotification
    {
        public void Notify(string message)
        {
            using (var sw = System.IO.File.AppendText(@"C:\Users\Ryan\source\repos\EP_PT_2026\EP_PT_Jan2026\Presentation\laptops.log"))
            {
                sw.WriteLine(message);
            }
        }
    }
}
