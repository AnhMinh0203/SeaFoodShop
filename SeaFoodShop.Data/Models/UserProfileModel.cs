using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SeaFoodShop.DataContext.Models
{
    public class UserProfileModel
    {
        public Guid Id { get; set; }
        public string FullName { get; set; }
        public DateTime Dob { get; set; }
        public int Gender { get; set; }
        public string? PhoneNumber { get; set; }
    }
    public class CustomerInforModel : UserProfileModel
    {
        public string? Address {  get; set; }    
    }

}
