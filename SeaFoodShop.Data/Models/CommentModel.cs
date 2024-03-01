using Microsoft.AspNetCore.Mvc.RazorPages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SeaFoodShop.DataContext.Models
{

    public class CommentSearchModel
    {
        public string TextComment { get; set; }
        public List<ImageModel> Images { get; set; }
    }
    public class CommentUpdateModel: CommentSearchModel
    {
        public int Id { get; set; }
        public double Star { get; set; }
    }
    public class CommentModel: CommentUpdateModel
    {
        public int LikeCount { get; set; }
        public int Dislike { get; set; }
        public int IdFood { get; set; }
        public Guid IdUser { get; set; }
    }
    public class ImageModel
    {
        public string nameImage { get; set; }
    } 


}
