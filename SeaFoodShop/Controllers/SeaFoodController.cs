using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SeaFoodShop.DataContext.Models;
using SeaFoodShop.Models;
using SeaFoodShop.Repository.Repositories;

namespace SeaFoodShop.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SeaFoodController : ControllerBase
    {
        private readonly SeaFoodRespon _seaFoodRespon;
        public SeaFoodController (SeaFoodRespon seaFoodRes)
        {
            _seaFoodRespon = seaFoodRes;
        }
        [HttpGet]
        public async Task<List<SeaFoodModel>> getSeaFoods(int pageNumber, int pageSize)
        {
            return await _seaFoodRespon.getSeaFoodsAsync(pageNumber, pageSize);
        }

        [HttpGet("SeaFoodDetail")]
        public async Task<MethodResult> getSeaFoodDetail(int id)
        {
            var result =  await _seaFoodRespon.getSeaFoodDetailAsync(id);
            if (result == null) return MethodResult.Result(null, "Sản phẩm không tồn tại");
            return MethodResult.Result(result,null);
        }

        [HttpPost("SearchSeafood")]
        public async Task<List<SeaFoodModel>> searchSeaFood (string nameSeaFood)
        {
            return await _seaFoodRespon.searchSeaFoodAsync(nameSeaFood);
        }

        [HttpPost("TypeName")]
        public async Task<List<SeaFoodModel>> searchSeaFoodByType(string nameSeaFood)
        {
            return await _seaFoodRespon.searchSeaFoodByTypeAsync(nameSeaFood);
        }

        [HttpGet("GetFavoriteSeafoods")]
        public async Task<List<SeaFoodModel>?> getFavoriteSeafoods (string token)
        {
            return await _seaFoodRespon.getFavoriteSeafoodsAsync(token);

        }
        [HttpPost("AddFavoriteSeafood")]
        public async Task<string> addFavoriteSeafood (string token, string idSeafood)
        {
            return await _seaFoodRespon.addFavoriteSeafoodAsync(token, idSeafood);
        }
        [HttpPost("DeleteFavoriteSeafood")]
        public async Task<string> deleteFavoriteSeafood (string token, string idSeafood)
        {
            return await _seaFoodRespon.deleteFavoriteSeafoodAsync (token, idSeafood);  
        }
    }
}
