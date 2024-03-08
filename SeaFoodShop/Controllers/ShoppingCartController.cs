using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SeaFoodShop.DataContext.Models;
using SeaFoodShop.Models;
using SeaFoodShop.Repository;

namespace SeaFoodShop.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ShoppingCartController : ControllerBase
    {
        private readonly ShoppingCartRespon _shoppingCartRespon;
        public ShoppingCartController(ShoppingCartRespon shoppingCartRespon)
        {
            _shoppingCartRespon = shoppingCartRespon;
        }
        [HttpPost("AddShoppingCart")]
        public async Task<string> AddToShoppingCart(ShoppingCartModel shoppingCart, string token)
        {
            return await _shoppingCartRespon.addToShoppingCart(shoppingCart, token);
        }

        [HttpPut("UpdateShoppingCart")]
        public async Task<string> UpdateToShoppingCart (ShoppingCartModel shoppingCart,string token)
        {
            return await _shoppingCartRespon.updateShoppingCartAsync(shoppingCart,token);
        }

        [HttpDelete("DeleteShoppingCart")]
        public async Task<string> DeleteShoppingCart (string idSeaFood, string token)
        {
            return await _shoppingCartRespon.deleteShoppingCartAsync(idSeaFood,token);
        }
        [HttpGet("SearchShoppingCart")]
        public async Task<List<SeaFoodModel>?> SearchShoppingCart(string? nameSeaFood, string token)
        {
             return await _shoppingCartRespon.searchShoppingCartAsync(nameSeaFood,token);
        }

        // display shopping cart items controller
    }
}
