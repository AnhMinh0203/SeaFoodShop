using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SeaFoodShop.DataContext.Models
{
    public class TypeModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public DateTime CreateDate { get; set; }
        public Guid CreateBy { get; set; }
        public DateTime UpdateDate { get; set; }
        public Guid UpdateBy { get; set; }
    }
}
