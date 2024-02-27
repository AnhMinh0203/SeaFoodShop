using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SeaFoodShop.DataContext.Data
{
    public class Order
    {
        public int Id { get; set; }
        public string PaymentMethod { get; set; }   
        public int IdVoucher { get; set; } 
        public int IdFood { get; set; }
        public int IdUser { get; set; }
    }
}
