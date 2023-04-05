    using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Shop_dotNet.Models
{
    public class OrderViewModel { 
    [Required(ErrorMessage = "Tên người nhận không được để trống")]
    public string name_receive { get; set; }
    [Required(ErrorMessage = "Số điện thoại không được để trống")]
    public string phone_receive { get; set; }
    [Required(ErrorMessage = "Địa chỉ không được để trống")]
    public string address_receive { get; set; }
    public string Email { get; set; }
    public string Note { get; set; }
    public int TypePayment { get; set; }
    public int price { get; set; }
    }
}