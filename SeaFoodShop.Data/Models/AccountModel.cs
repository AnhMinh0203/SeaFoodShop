using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SeaFoodShop.DataContext.Models
{
    public class AccountModel 
    {
        public Guid Id { get; set; } 
        public string FullName { get; set; }
        public string PhoneNumber { get; set; }
        public DateTime Dob { get; set; }
        public string Gender { get; set; }
        protected int Status { get; set; }
    }

    public class SignUpModel : AccountModel
    {

        public string Password { get; set; }
        public string RepeatPassword { get; set; }
    }

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
