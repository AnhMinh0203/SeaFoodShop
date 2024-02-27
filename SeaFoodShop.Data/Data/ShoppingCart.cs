using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SeaFoodShop.DataContext.Data
{
    public class ShoppingCart
    {
        public int IdFood { get; set; }
        public Guid IdUser { get; set; }
        public int Quantity { get; set; }
    }
}
