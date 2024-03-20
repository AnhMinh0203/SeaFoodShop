using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SeaFoodShop.DataContext.Models;
using SeaFoodShop.Repository.Repositories;
using System.ComponentModel;

namespace SeaFoodShop.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ManagerCustomerController : ControllerBase
    {
        private readonly ManagerCustomerRespon _mCustomerRes;
        public ManagerCustomerController(ManagerCustomerRespon mCustomerRes)
        {
            _mCustomerRes = mCustomerRes;
        }
        [HttpDelete("DeleteCustomer")]
        public async Task<string> DeleteCustomer(string token, string phoneNumber)
        {
            return await _mCustomerRes.deleteCustomerAsync(token, phoneNumber);
        }
        [HttpGet("SearchCustomers")]
        public async Task<List<CustomerInforModel>?> SearchCustomer (string token,string phoneNumber, string status)
        {
            return await _mCustomerRes.searchCustomerAsync(token, phoneNumber, status); 
        }
    }
}
