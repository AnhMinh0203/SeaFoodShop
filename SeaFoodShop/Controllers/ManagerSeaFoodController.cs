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
        public async Task<string> addSeaFood (SeaFoodDetailModel seafoodDeail, string token)
        {
            return await _mSeafoodRes.addSeaFoodAsync(seafoodDeail, token);
        }
    }
}
