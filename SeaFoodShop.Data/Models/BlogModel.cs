using SeaFoodShop.DataContext.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SeaFoodShop.DataContext.Models
{
    public class BlogModel
    {
        public int Id { get; set; }
        public Guid IdUser { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }
        public DateTime PublishDate { get; set; }
        public string Thumbnail { get; set; }
        public int View {  get; set; }
        public int Like { get; set; }
    }

    public class BlogDetailModel: BlogModel
    {
        public List<ImageBlogModel> listBlogImage { get; set; }
    }

    public class ImageBlogModel
    {
        public string nameImage { get; set; }
    }
}
