using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SeaFoodShop.DataContext.Models;
using SeaFoodShop.Repository;

namespace SeaFoodShop.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CommentController : ControllerBase
    {
        private readonly CommentRespon _commentRespon;
        public CommentController(CommentRespon commentRespon)
        {
            _commentRespon = commentRespon;
        }

        [HttpPost("postComment")]
        public async Task<string> postComment(CommentModel comment, string token)
        {
            var commentResult = await _commentRespon.postCommentAsync(comment, token);
            if (commentResult == null) return "Vui lòng đăng nhập tài khoản";
            return "Bình luận thành công";
        }

        [HttpPut("updateComment")]
        public async Task<string> updateComment(CommentUpdateModel comment, string token)
        {
            var commentResult = await _commentRespon.updateCommentAsync(comment, token);
            if (commentResult == null) return "Vui lòng đăng nhập tài khoản";
            return "Cập nhật bình luận thành công";
        }

        [HttpDelete("deleteComment")]
        public async Task<string?> deleteComment(string idComment,string token)
        {
            var idCommentModel = await _commentRespon.deleteCommentAsync(idComment, token);
            if (idCommentModel==true) return "Xóa bình luận thành công";
            return "Xóa bình luận thành không thành công";
        }

        [HttpGet("searchComment")]
        public async Task<List<CommentSearchModel>?> searchComment(string text,string token)
        {
            List<CommentSearchModel> commentSearchModels = new List< CommentSearchModel >();
            commentSearchModels = await _commentRespon.searchCommentAsync(text, token);
            if (commentSearchModels == null) return null;
            return commentSearchModels;
        }
    }
}
