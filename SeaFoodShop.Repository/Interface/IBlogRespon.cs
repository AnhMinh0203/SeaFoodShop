﻿using SeaFoodShop.DataContext.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SeaFoodShop.Repository.Interface
{
    public interface IBlogRespon
    {
        public Task<List<BlogModel>?> getBlogsAsync(int pageNumber, int pageSize);
        public Task<BlogDetailModel?> getBlogDetailAsync(string idBlog);
    }
}
