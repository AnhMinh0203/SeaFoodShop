using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SeaFoodShop.DataContext.Models;
using SeaFoodShop.Repository;
using SeaFoodShop.Repository.Interface;

namespace SeaFoodShop.API.Controllers
{
    
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserRespon _userRespon;
        public UserController (IUserRespon userRespon)
        {
            _userRespon = userRespon;
        }

        [HttpPost("SignIn")]
        public async Task<CustomMessage> SignIn(SignInModel signInModel)
        {
            var result = await _userRespon.SignInAsync(signInModel);
            return new CustomMessage { Message = result.Message, Token = result.Token };
        }

        [HttpPost("SignUp")]
        public async Task<MethodResult> SignUp (SignUpModel signUpModel)
        {
            var result = await _userRespon.SignUpAsync(signUpModel);
            if (string.IsNullOrEmpty(result))
            {
                return MethodResult.ResultWithSuccess(result, false);
            }
            return MethodResult.ResultWithSuccess(result, true);
        }
    }
}
