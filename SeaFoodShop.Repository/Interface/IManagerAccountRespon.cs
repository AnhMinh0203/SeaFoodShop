using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SeaFoodShop.DataContext.Models;

namespace SeaFoodShop.Repository.Interface
{
    public interface IManagerAccountRespon
    {
        public Task<string> LockAccountAsync(string phoneNumber);
        public Task<string> UnlockAccountAsync(string phoneNumber);
        public Task<List<AccountModel>?> SearchAccountAsync(string token, string phoneNumber, string status);
        public Task<string> changePasswordAdminAsync(string token, ChangePasswordAdminModel password);
    }
}
