using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SeaFoodShop.DataContext.Data
{
    public class Comment
    {
        public int Id { get; set; }
        public string TextComment { get; set; }
        public int LikeCount { get; set; }
        public int Dislike {  get; set; }
        public int Status { get; set; }
        public int IdFood { get; set; }
        public int IdUser { get; set; }
        public int Star { get; set; }   

    }
}
