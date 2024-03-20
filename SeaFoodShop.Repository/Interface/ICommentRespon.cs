using SeaFoodShop.DataContext.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SeaFoodShop.Repository.Interface
{
    public interface ICommentRespon
    {
        public Task<bool?> postCommentAsync(CommentModel comment, string token);
        public Task<bool?> updateCommentAsync(CommentUpdateModel commentUpdate, string token);
        public Task<bool?> deleteCommentAsync(string id, string token);
        public Task<List<CommentSearchModel>?> searchCommentAsync(string text, string token);
    }
}
