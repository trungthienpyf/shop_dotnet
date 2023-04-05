using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Shop_dotNet.Models
{
    
    public class CartItem
    {
      
        public string size { get; set; }
        public int Quantity { get; set; } 

        public product product { get; set; }    
    }
}