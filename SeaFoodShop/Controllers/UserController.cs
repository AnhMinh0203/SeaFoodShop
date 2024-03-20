using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SeaFoodShop.DataContext.Data;
using SeaFoodShop.DataContext.Models;
using SeaFoodShop.Repository.Repositories;

namespace SeaFoodShop.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly UserRespon _userRespon;
        public UserController(UserRespon userRespon)
        {
            _userRespon = userRespon;
        }
        [HttpPost("UpdateProfile")]
        public async Task<string> updateUserProfile(UserProfileModel userProfileModel,string token)
        {
            return await _userRespon.updateUserProfileAsync(token, userProfileModel);
        }
        [HttpGet("GetProfile")]
        public async Task<UserProfileModel?> getUserProfile(string token)
        {
            var result = await _userRespon.getUserProfile(token);
            if(result == null) return null;
            return result;
        }

    }
}
