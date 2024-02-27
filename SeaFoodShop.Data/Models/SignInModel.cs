using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SeaFoodShop.DataContext.Models
{
    public class SignInModel
    {
        public string PhoneNumber { get; set; }
        public string Password { get; set; }
    }
    public class CustomMessage
    {
        public string Message { get; set; }
        public string? Token { get; set; }
    }
}
