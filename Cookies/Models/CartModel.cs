using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Cookies.Models
{
    public class CartModel
    {
        [Key]
        public string ID { get; set; }

        [Column(TypeName = "nvarchar(450)")]
        public string ProductID { get; set; }

        [Column(TypeName = "nvarchar(450)")]
        public string UserID { get; set; }

        public int Quantity { get; set; }
    }
}
