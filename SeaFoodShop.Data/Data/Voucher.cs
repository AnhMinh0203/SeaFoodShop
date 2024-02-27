using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SeaFoodShop.DataContext.Data
{
    public class Voucher
    {
        public int Id { get; set; }
        public string NameVoucher { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public DateTime CreateDate { get; set; }
        public Guid CreateBy { get; set; }
        public DateTime ModifyDate { get; set; }
        public Guid ModifyBy { get; set; }
    }
}
