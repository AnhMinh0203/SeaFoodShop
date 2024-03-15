using SeaFoodShop.DataContext.Models;
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
    }
}
