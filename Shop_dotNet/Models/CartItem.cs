using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Shop_dotNet.Models
{
    [Serializable]
    public class CartItem
    {
        public product product { get; set; }
        public int Quantity { get; set; } 
    }
}