using SeaFoodShop.DataContext.Models;
using SeaFoodShop.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SeaFoodShop.Repository.Interface
{
    public interface IAccountRespon
    {
        public Task<CustomMessage> SignInAsync(SignInModel model);
        public Task<string> SignUpAsync(SignUpModel model);
        public Task<string> changePasswordAsync(string token, ChangePasswordModel password);
    }
}
