using SeaFoodShop.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SeaFoodShop.Repository.Interface
{
    public interface ISeaFoodRespon
    {
        public Task<List<SeaFoodModel>> getSeaFoodsAsync(int pageNumber, int pageSize);
        public Task<SeaFoodDetailModel> getSeaFoodDetailAsync(int id);
        public Task<List<SeaFoodModel>> searchSeaFoodAsync(string nameSeaFood);
        public Task<List<SeaFoodModel>> searchSeaFoodByTypeAsync(string nameType);
    }
}
