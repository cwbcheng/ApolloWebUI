using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace ApolloWebUI.Model
{
    [Table("Products")]
    public class Product
    {
        [Key]
        [MaxLength(20)]
        public string Id { get; set; }

        [MaxLength(20)]
        [Required]
        public string ProductName { get; set; }

        public bool IsDisable { get; set; }

        public List<UserProduct> UserProducts { get; set; }
    }
}
