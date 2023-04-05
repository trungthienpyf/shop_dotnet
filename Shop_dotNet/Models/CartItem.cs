using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Shop_dotNet.Models
{
    
    public class CartItem
    {
        [Key]
        public int RecordId { get; set; }
        public string CartId { get; set; }
        public int id { get; set; }
        public int Quantity { get; set; } 

        public product product { get; set; }    
    }
}