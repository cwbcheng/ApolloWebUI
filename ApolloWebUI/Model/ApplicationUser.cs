using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ApolloWebUI.Model
{
    public class ApplicationUser: IdentityUser
    {
        public string Name { get; set; }

        public List<UserProduct> UserProducts { get; set; }
    }
}
