using Microsoft.AspNetCore.Http;
using SeaFoodShop.DataContext.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SeaFoodShop.DataContext.Models
{
    public class BlogUpdateModel
    {
        public string Title { get; set; }
        public string Thumbnail { get; set; }
        public string Content { get; set; }
    }
    public class BlogDetailModel: BlogUpdateModel
    {
        public int Id { get; set; }
        public Guid IdUser { get; set; }
       
        public DateTime PublishedDate { get; set; }
        public int Views { get; set; }
        public int Likes { get; set; }
    }


}
