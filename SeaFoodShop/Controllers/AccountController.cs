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
    public class AccountController : ControllerBase
    {
        private readonly IAccountRespon _accountRespon;
        public AccountController (IAccountRespon accountRespon)
        {
            _accountRespon = accountRespon;
        }

        [HttpPost("SignIn")]
        public async Task<CustomMessage> SignIn(SignInModel signInModel)
        {
            var result = await _accountRespon.SignInAsync(signInModel);
            return new CustomMessage { Message = result.Message, Token = result.Token };
        }

        [HttpPost("SignUp")]
        public async Task<MethodResult> SignUp (SignUpModel signUpModel)
        {
            var result = await _accountRespon.SignUpAsync(signUpModel);
            if (string.IsNullOrEmpty(result))
            {
                return MethodResult.ResultWithSuccess(result, false);
            }
            return MethodResult.ResultWithSuccess(result, true);
        }
        [HttpPut("LockAccount")]
        public async Task<string> lockAccount (string phoneNumber)
        {
            return await _accountRespon.LockAccountAsync(phoneNumber);
        }
        [HttpPut("UnLockAccount")]
        public async Task<string> unLockAccount(string phoneNumber)
        {
            return await _accountRespon.UnlockAccountAsync(phoneNumber);
        }
        [HttpGet("SearchAccount")]
        public async Task<List<AccountModel>?> searchAccount (string token, string phoneNumber, string status)
        {
            return await _accountRespon.SearchAccountAsync(token, phoneNumber, status);
        }

        [HttpPut("ChangePassword")]
        public async Task<string> changePassword(string token, ChangePasswordModel password)
        {
            return await _accountRespon.changePasswordAsync(token, password);
        }

        [HttpPut("ChangePasswordAdmin")]
        public async Task<string> changePasswordAdmin(string token, ChangePasswordAdminModel password)
        {
            return await _accountRespon.changePasswordAdminAsync(token, password);
        }
    }
}
