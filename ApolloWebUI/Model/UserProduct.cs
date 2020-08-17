using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ApolloWebUI.Model
{
    public class UserProduct
    {
        public string UserId { get; set; }
        public ApplicationUser User { get; set; }
        public string ProductId { get; set; }
        public Product Product { get; set; }
    }
}
