using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SeaFoodShop.DataContext.Models
{
    public class ChangePasswordAdminModel
    {
        public string PhoneNumber { get; set; }
        public string NewPassword { get; set; }
    }
    public class ChangePasswordModel
    {
        public string NewPassword { get; set; }
        public string OldPassword { get; set; }
    }
}
