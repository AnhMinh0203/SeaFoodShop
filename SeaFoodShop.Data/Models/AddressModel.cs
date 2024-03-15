using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace SeaFoodShop.DataContext.Models
{
    public class AddressModel
    {
        [JsonIgnore]
        public Guid Id { get; set; }
        public string NameAddress { get; set; }
        public string PhoneNumber { get; set; }
        public int isDefault { get; set; }
    }
}
