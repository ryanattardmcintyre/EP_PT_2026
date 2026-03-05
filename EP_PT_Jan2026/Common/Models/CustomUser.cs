using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Models
{
    public class CustomUser: IdentityUser
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }

        [DefaultValue(false)]
        public bool CreateProductCapability { get; set; }
    }
}
