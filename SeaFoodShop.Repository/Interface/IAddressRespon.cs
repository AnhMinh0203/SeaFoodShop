using SeaFoodShop.DataContext.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SeaFoodShop.Repository.Interface
{
    public interface IAddressRespon
    {
        public Task<string> addAddressAsync(string token, AddressModel address);
        public Task<List<AddressModel>?> getAddressAsync(string token);
        public Task<string> deleteAddressAsync(string token, string idAddress);

    }
}
