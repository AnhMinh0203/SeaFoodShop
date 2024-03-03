using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SeaFoodShop.DataContext.Models;
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
        [HttpPost]
        public async Task<string> AddToShoppingCart(ShoppingCartModel shoppingCart, string token)
        {
            return await _shoppingCartRespon.addToShoppingCart(shoppingCart, token);
        }

        [HttpPut]
        public async Task<string> UpdateToShoppingCart (ShoppingCartModel shoppingCart,string token)
        {
            return await _shoppingCartRespon.updateSeaFoodAsync(shoppingCart,token);
        }
        // display shopping cart items controller
    }
}
