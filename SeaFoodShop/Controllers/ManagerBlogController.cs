using Microsoft.AspNetCore.Mvc;
using SeaFoodShop.DataContext.Models;
using SeaFoodShop.Models;
using SeaFoodShop.Repository.Common;
using SeaFoodShop.Repository.Repositories;

namespace SeaFoodShop.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ManagerBlogController : ControllerBase
    {
        
        private readonly ManagerBlogRespon _mngBlog;
        public ManagerBlogController (ManagerBlogRespon mngBlog  )
        {
            _mngBlog = mngBlog;
        }

        [HttpPost("UploadBlogImg")]
        public async Task<string> UploadBlogImg( IFormFile file, string idFolder)  => await _mngBlog.UploadImgBlogAsync( file, idFolder);
        [HttpPost("UploadBlogContent")]
        public async Task<string> UploadBlogContent (BlogDetailModel blog, string token) => await _mngBlog.UploadContentBlogAsync( blog, token);
        [HttpPost("UpdateBlogImg")]
        public async Task<string?> UpdateBlogImg(IFormFile file, string idFolder, string idFile) => await _mngBlog.UpdateImgBlogAsync(file, idFolder, idFile);
        [HttpPost("UpdateBlogContent")]
        public async Task<string?> UpdateBlogContent(BlogUpdateModel blog, string token, string idBlog) => await _mngBlog.UpdateBlogContentAsync(blog, token, idBlog) ;

        [HttpDelete("DeleteImage")]
        public async Task<string?> DeleteImg(string idImg) => await _mngBlog.DeleteImgBlogAsync(idImg);
         
    }
}
