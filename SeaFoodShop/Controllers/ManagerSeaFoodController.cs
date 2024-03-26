using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SeaFoodShop.Models;
using SeaFoodShop.Repository.Repositories;

namespace SeaFoodShop.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ManagerSeaFoodController : ControllerBase
    {
        private readonly ManagerSeaFoodRespon _mSeafoodRes;
        public ManagerSeaFoodController(ManagerSeaFoodRespon mSeafoodRes)
        {
            _mSeafoodRes = mSeafoodRes;
        }
        [HttpPost("AddSeaFood")]
        public async Task<string> AddSeaFood(SeaFoodDetailModel seafoodDeail, string token) => await _mSeafoodRes.addSeaFoodAsync(seafoodDeail, token);
        [HttpPut("UpdateSeaFood")]
        public async Task<string> UpdateSeaFood(SeaFoodDetailModel seafoodDeail, string token) => await _mSeafoodRes.updateSeaFoodAsync(seafoodDeail, token);
        [HttpDelete("DeleteSeaFood")]
        public async Task<string> DeleteSeaFood(string token, string seafoodId) => await _mSeafoodRes.deleteSeaFoodAsync(token, seafoodId);
    }
}
