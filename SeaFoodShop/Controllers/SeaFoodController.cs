using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SeaFoodShop.Models;
using SeaFoodShop.Repository;

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

        [HttpPost("id")]
        public async Task<SeaFoodDetailModel> getSeaFoodDetail(int id)
        {
            return await _seaFoodRespon.getSeaFoodDetailAsync(id);
        }

        [HttpPost("SeafoodName")]
        public async Task<List<SeaFoodModel>> searchSeaFood (string nameSeaFood)
        {
            return await _seaFoodRespon.searchSeaFoodAsync(nameSeaFood);
        }

        [HttpPost("TypeName")]
        public async Task<List<SeaFoodModel>> searchSeaFoodByType(string nameSeaFood)
        {
            return await _seaFoodRespon.searchSeaFoodByTypeAsync(nameSeaFood);
        }
    }
}
