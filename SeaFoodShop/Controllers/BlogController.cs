using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SeaFoodShop.DataContext.Models;
using SeaFoodShop.Repository.Repositories;

namespace SeaFoodShop.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BlogController : ControllerBase
    {
        private readonly BlogRespon _blogRespon;
        public BlogController (BlogRespon blogRespon)
        {
            _blogRespon = blogRespon;
        }
        [HttpGet("getBlog")]
        public async Task<List<BlogModel>?> getBlogs (int pageNumber, int pageSize)
        {
            return await _blogRespon.getBlogsAsync (pageNumber, pageSize);
        }
        [HttpGet("getBlogDetail")]
        public async Task<BlogDetailModel?> getBlogDetail (string idBlog)
        {
            return await _blogRespon.getBlogDetailAsync(idBlog);
        }
    }
}
