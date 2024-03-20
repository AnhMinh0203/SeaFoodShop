using SeaFoodShop.DataContext.Models;
using SeaFoodShop.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SeaFoodShop.Repository.Interface
{
    public interface IShoppingCartRespon
    {
        public Task<string> addToShoppingCart(ShoppingCartModel shoppingCartModel, string token);
        public Task<string> updateShoppingCartAsync(ShoppingCartModel shoppingCartModel, string token);
        public Task<string> deleteShoppingCartAsync(string idSeaFood, string token);
        public Task<List<SeaFoodModel>?> searchShoppingCartAsync(string? nameSeaFood, string token);

    }
}
