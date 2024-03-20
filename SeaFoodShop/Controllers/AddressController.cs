using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SeaFoodShop.DataContext.Models;
using SeaFoodShop.Repository.Repositories;

namespace SeaFoodShop.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AddressController : ControllerBase
    {
        private readonly AddressRespon _addressRespon; 
        public AddressController (AddressRespon addressRespon)
        {
            _addressRespon = addressRespon;
        }

        [HttpPost("Add address")]
        public async Task<string> addAddress (string token, AddressModel address)
        {
            return await _addressRespon.addAddressAsync (token, address);
        }

        [HttpGet("Get Address ")]
        public async Task<List<AddressModel>?> getAddress (string token)
        {
            return await _addressRespon.getAddressAsync (token);
        }

        [HttpDelete("Get Address ")]
        public async Task<string> deleteAddress(string token, string idAddress)
        {
            return await _addressRespon.deleteAddressAsync(token, idAddress);
        }
    }
}
