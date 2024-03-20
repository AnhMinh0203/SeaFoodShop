using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SeaFoodShop.DataContext.Models;
using SeaFoodShop.Repository.Interface;
using SeaFoodShop.Repository.Repositories;

namespace SeaFoodShop.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ManagerAccountController : ControllerBase
    {
        private readonly ManagerAccountRespon _managerAccountRespon;
        public ManagerAccountController(ManagerAccountRespon managerAccountRespon)
        {
            _managerAccountRespon = managerAccountRespon;
        }
        [HttpPut("LockAccount")]
        public async Task<string> lockAccount(string phoneNumber)
        {
            return await _managerAccountRespon.LockAccountAsync(phoneNumber);
        }
        [HttpPut("UnLockAccount")]
        public async Task<string> unLockAccount(string phoneNumber)
        {
            return await _managerAccountRespon.UnlockAccountAsync(phoneNumber);
        }
        [HttpGet("SearchAccount")]
        public async Task<List<AccountModel>?> searchAccount(string token, string phoneNumber, string status)
        {
            return await _managerAccountRespon.SearchAccountAsync(token, phoneNumber, status);
        }

        [HttpPut("ChangePasswordAdmin")]
        public async Task<string> changePasswordAdmin(string token, ChangePasswordAdminModel password)
        {
            return await _managerAccountRespon.changePasswordAdminAsync(token, password);
        }
    }
}
