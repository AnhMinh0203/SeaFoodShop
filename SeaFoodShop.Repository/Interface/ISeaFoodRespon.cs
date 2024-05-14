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
        public Task<ListSeaFoodModel> getSeaFoodsAsync(int pageNumber, int pageSize);
        public Task<SeaFoodDetailModel?> GetSeaFoodDetailAsync(string id);
        public Task<ListSeaFoodModel> searchSeaFoodAsync(string nameSeaFood,int pageIndex, int pageSize);
        public Task<ListSeaFoodModel> searchSeaFoodByTypeAsync(string nameType, int pageIndex, int pageSize);
    }
}
