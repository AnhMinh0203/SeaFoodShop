using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SeaFoodShop.DataContext.Models;
using SeaFoodShop.Repository;

namespace SeaFoodShop.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TypeController : ControllerBase
    {
        private readonly TypeRespon _typeRespon;
        public TypeController (TypeRespon typeRespon)
        {
            _typeRespon = typeRespon;
        }

        [HttpPost("AddType")]
        public async Task<string> AddSeaFoodType(TypeModel type,string token)
        {
            return await _typeRespon.addTypeSeaFoodAsync(type, token);
        }

        [HttpDelete("AddType")]
        public async Task<string> DeleteSeaFoodType (string nameType, string token)
        {
            return await _typeRespon.deleteTypeSeaFoodAsync (nameType, token);
        }

        [HttpGet]
        public async Task<List<TypeModel>> GetSeaFoodTypes (string token)
        {
            return await _typeRespon.getSeaFoodTypeAsync(token);
        }
    }
}
