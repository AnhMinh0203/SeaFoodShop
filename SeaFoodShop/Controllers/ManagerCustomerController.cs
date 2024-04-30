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
        public async Task<ListCustomerInforModel?> SearchCustomer (string token,string textInput, int pageIndex, int pageSize)
        {
            return await _mCustomerRes.searchCustomerAsync(token, textInput, pageIndex, pageSize); 
        }
        [HttpGet("GetAllCustomers")]
        public async Task<ListCustomerInforModel> GetAllCustomers(string token, string status,int gender ,int pageIndex, int pageSize)
        {
            return await _mCustomerRes.getAllCustomersAsync(token, status, gender,pageIndex, pageSize);
        }
    }
}
